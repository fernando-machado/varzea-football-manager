using MongoDB.Driver;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VarzeaFootballManager.Domain.Core;

namespace VarzeaFootballManager.Persistence.Repositorios
{
    /// <summary>
    /// repository implementation for mongo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : AggregateRoot
    {
        #region MongoSpecific

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mongoDb">Mongo Database</param>
        public RepositoryAsync(IDatabase mongoDb)
        {
            Collection = mongoDb.GetCollection<T>();
        }

        /// <summary>
        /// mongo collection
        /// </summary>
        public IMongoCollection<T> Collection { get; }

        /// <summary>
        /// filter for collection
        /// </summary>
        public FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        /// <summary>
        /// projector for collection
        /// </summary>
        public ProjectionDefinitionBuilder<T> Project => Builders<T>.Projection;

        /// <summary>
        /// updater for collection
        /// </summary>
        public UpdateDefinitionBuilder<T> Updater => Builders<T>.Update;

        private IFindFluent<T, T> Query()
        {
            return Collection.Find(Filter.Empty);
        }

        private IFindFluent<T, T> Query(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter);
        }

        private IOrderedFindFluent<T, T> Query(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query().Skip(pageIndex * size).Limit(size);
            return isDescending ? query.SortByDescending(order) : query.SortBy(order);
        }

        private IOrderedFindFluent<T, T> Query(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query(filter).Skip(pageIndex * size).Limit(size);
            return isDescending ? query.SortByDescending(order) : query.SortBy(order);
        }
        #endregion MongoSpecific

        #region CRUD

        #region Delete

        /// <summary>
        /// delete aggregateRoot
        /// </summary>
        /// <param name="aggregateRoot">aggregateRoot</param>
        public async Task DeleteAsync(T aggregateRoot)
        {
            await DeleteAsync(aggregateRoot.Id);
        }

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        public virtual async Task DeleteAsync(string id)
        {
            await Retry(() =>
            {
                return Collection.DeleteOneAsync(i => i.Id == id);
            });
        }

        /// <summary>
        /// delete items with filter
        /// </summary>
        /// <param name="filter">expression filter</param>
        public virtual async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            await Retry(() =>
            {
                return Collection.DeleteManyAsync(filter);
            });
        }

        #endregion Delete

        #region Find

        /// <summary>
        /// find entities
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>collection of aggregateRoot</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await Query(filter).ToListAsync();
        }

        /// <summary>
        /// find entities with paging
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of aggregateRoot</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return await FindAsync(filter, i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// find entities with paging and ordering
        /// default ordering is descending
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of aggregateRoot</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return await FindAsync(filter, order, pageIndex, size, true);
        }

        /// <summary>
        /// find entities with paging and ordering in direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of aggregateRoot</returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return await Retry(() =>
            {
                return Query(filter, order, pageIndex, size, isDescending).ToListAsync();
            });
        }

        #endregion Find

        #region FindAll

        /// <summary>
        /// fetch all items in collection
        /// </summary>
        /// <returns>collection of aggregateRoot</returns>
        public virtual async Task<IEnumerable<T>> FindAllAsync()
        {
            return await Retry(() =>
            {
                return Query().ToListAsync();
            });
        }

        /// <summary>
        /// fetch all items in collection with paging
        /// </summary>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of aggregateRoot</returns>
        public async Task<IEnumerable<T>> FindAllAsync(int pageIndex, int size)
        {
            return await FindAllAsync(i => i.Id, pageIndex, size);
        }

        /// <summary>
        /// fetch all items in collection with paging and ordering
        /// default ordering is descending
        /// </summary>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of aggregateRoot</returns>
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return await FindAllAsync(order, pageIndex, size, true);
        }

        /// <summary>
        /// fetch all items in collection with paging and ordering in direction
        /// </summary>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of aggregateRoot</returns>
        public virtual async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return await Retry(() =>
            {
                return Query(order, pageIndex, size, isDescending).ToListAsync();
            });
        }

        #endregion FindAll

        #region First

        /// <summary>
        /// get first item in collection
        /// </summary>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public virtual async Task<T> FirstAsync()
        {
            return await Retry(() =>
            {
                return Query(i => i.Id, 0, 1, false).FirstOrDefaultAsync();
            });
        }

        /// <summary>
        /// get first item in query
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> filter)
        {
            return await FirstAsync(filter, i => i.Id);
        }

        /// <summary>
        /// get first item in query with order
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return await FirstAsync(filter, order, false);
        }

        /// <summary>
        /// get first item in query with order and direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return await Retry(() =>
            {
                return Query(filter, order, 0, 1, isDescending).SingleOrDefaultAsync();
            });
        }

        #endregion First

        #region Get

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public virtual async Task<T> GetAsync(string id)
        {
            return await Retry(() =>
            {
                return Query(i => i.Id == id).FirstOrDefaultAsync();
            });
        }

        #endregion Get

        #region Insert

        /// <summary>
        /// insert aggregateRoot
        /// </summary>
        /// <param name="aggregateRoot">aggregateRoot</param>
        public virtual async Task InsertAsync(T aggregateRoot)
        {
            await Retry(() =>
            {
                Collection.InsertOneAsync(aggregateRoot);
                return Task.FromResult(true);
            });
        }

        /// <summary>
        /// insert aggregateRoot collection
        /// </summary>
        /// <param name="entities">collection of entities</param>
        public virtual async Task InsertAsync(IEnumerable<T> entities)
        {
            await Retry(() =>
            {
                Collection.InsertManyAsync(entities);
                return Task.FromResult(true);
            });
        }

        #endregion Insert

        #region Last

        /// <summary>
        /// get first item in collection
        /// </summary>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> LastAsync()
        {
            return await Query(i => i.Id, 0, 1, true).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get last item in query
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> LastAsync(Expression<Func<T, bool>> filter)
        {
            return await LastAsync(filter, i => i.Id);
        }

        /// <summary>
        /// get last item in query with order
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return await LastAsync(filter, order, false);
        }

        /// <summary>
        /// get last item in query with order and direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>aggregateRoot of <typeparamref name="T"/></returns>
        public async Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return await FirstAsync(filter, order, !isDescending);
        }

        #endregion Last

        #region Replace

        /// <summary>
        /// replace an existing aggregateRoot
        /// </summary>
        /// <param name="aggregateRoot">aggregateRoot</param>
        public virtual async Task ReplaceAsync(T aggregateRoot)
        {
            await Retry(() =>
            {
                return Collection.ReplaceOneAsync(i => i.Id == aggregateRoot.Id, aggregateRoot);
            });
        }

        /// <summary>
        /// replace collection of entities
        /// </summary>
        /// <param name="entities">collection of entities</param>
        public async Task ReplaceAsync(IEnumerable<T> entities)
        {
            var tasks = new List<Task>();

            foreach (var aggregateRoot in entities)
            {
                tasks.Add(ReplaceAsync(aggregateRoot));
            }

            await Task.WhenAll(tasks);
        }

        #endregion Replace

        #region Update

        /// <summary>
        /// update an aggregateRoot with updated fields
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync(string id, params UpdateDefinition<T>[] updates)
        {
            return await UpdateAsync(Filter.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// update an aggregateRoot with updated fields
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync(string id, params Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>>[] updates)
        {
            return await UpdateAsync(id, updates.Select(update => update.Invoke(Updater)).ToArray());
        }

        /// <summary>
        /// update an aggregateRoot with updated fields
        /// </summary>
        /// <param name="aggregateRoot">aggregateRoot</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync(T aggregateRoot, params UpdateDefinition<T>[] updates)
        {
            return await UpdateAsync(aggregateRoot.Id, updates);
        }

        /// <summary>
        /// update an aggregateRoot with updated fields
        /// </summary>
        /// <param name="aggregateRoot">aggregateRoot</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync(T aggregateRoot, params Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>>[] updates)
        {
            return await UpdateAsync(aggregateRoot, updates.Select(update => update.Invoke(Updater)).ToArray());
        }

        /// <summary>
        /// update a property field in an aggregateRoot
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="aggregateRoot">aggregateRoot</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync<TField>(T aggregateRoot, Expression<Func<T, TField>> field, TField value)
        {
            return await UpdateAsync(aggregateRoot, Updater.Set(field, value));
        }

        /// <summary>
        /// update a property field in entities
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="filter">filter</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        public async Task<bool> UpdateAsync<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value)
        {
            return await UpdateAsync(filter, Updater.Set(field, value));
        }

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public virtual async Task<bool> UpdateAsync(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates)
        {
            return await Retry(async () =>
            {
                var update = Updater.Combine(updates).CurrentDate(i => i.ModifiedAt);
                return (await Collection.UpdateManyAsync(filter, update.CurrentDate(i => i.ModifiedAt))).IsAcknowledged;
            });
        }

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public virtual async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates)
        {
            return await Retry(async () =>
            {
                var update = Updater.Combine(updates).CurrentDate(i => i.ModifiedAt);
                return (await Collection.UpdateManyAsync(filter, update)).IsAcknowledged;
            });
        }

        #endregion Update

        #endregion CRUD

        #region Simplicity

        /// <summary>
        /// validate if filter result exists
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>true if exists, otherwise false</returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await Retry(() =>
            {
                return Query(filter).AnyAsync();
            });
        }

        #endregion Simplicity

        #region RetryPolicy
        /// <summary>
        /// retry operation for three times if IOException occurs
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="action">action</param>
        /// <returns>action result</returns>
        /// <example>
        /// return Retry(() => 
        /// { 
        ///     do_something;
        ///     return something;
        /// });
        /// </example>
        protected TResult Retry<TResult>(Func<TResult> action)
        {
            return RetryPolicy
                .Handle<MongoConnectionException>(i => i.InnerException.GetType() == typeof(IOException))
                .Retry(3)
                .Execute(action);
        }
        #endregion
    }
}
