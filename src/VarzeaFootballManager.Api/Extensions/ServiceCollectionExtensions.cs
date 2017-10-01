using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VarzeaFootballManager.Domain.Core;
using VarzeaFootballManager.Persistence.InfraMongoDb;

namespace VarzeaFootballManager.Api.Extensions
{
    /// <summary>
    /// Classe com métodos de extensão utlilizados na classe Startup
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adiciona a injecao de dependência necessária para o MongoDb
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returs <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            //services.AddSingleton<MongoDB.Driver.IMongoClient>(provider =>
            //{
            //    var config = provider.GetService<IConfiguration>();

            //    var mongoConnectionString = config.GetSection("MongoDb").GetValue<string>("ConnectionString");

            //    var mongoSettings = MongoDB.Driver.MongoClientSettings.FromUrl(MongoDB.Driver.MongoUrl.Create(mongoConnectionString));

            //    return new MongoDB.Driver.MongoClient(mongoSettings);
            //});


            //services.AddSingleton<MongoDB.Driver.IMongoDatabase>(provider =>
            //{
            //    var config = provider.GetService<IConfiguration>();
            //    var mongoClient = provider.GetService<MongoDB.Driver.IMongoClient>();
                
            //    var mongoDatabaseName = config.GetSection("MongoDb").GetValue<string>("DatabaseName");

            //    return mongoClient.GetDatabase(mongoDatabaseName);
            //});


            return services.AddSingleton<IDatabase>(provider => 
            {
                var config = provider.GetService<IConfiguration>();

                var configsMongoDb = config.GetSection("MongoDb");
                var connectionString = configsMongoDb.GetValue<string>("ConnectionString");
                var databaseName = configsMongoDb.GetValue<string>("DatabaseName");

                return new MongoDatabase(connectionString, databaseName);
            });
        }
    }
}
