using System;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using VarzeaFootballManager.Domain.Core;
using VarzeaFootballManager.Domain.Jogadores;
using VarzeaFootballManager.Persistence.Mapeamentos;

namespace VarzeaFootballManager.Persistence.InfraMongoDb
{
    public class MongoDatabase : IDatabase
    {
        #region Basic Context Infrastructure
        
        private IMongoDatabase Database { get; }

        /// <summary>
        /// Construtor <see cref="MongoDatabase"/> 
        /// </summary>
        public MongoDatabase(string connectionString, string databaseName, string applicationName = "VarzeaManagerApi")
        {
            var mongoUrlBuilder = new MongoUrlBuilder(connectionString)
            {
                ApplicationName = applicationName,
                DatabaseName = databaseName,
                Server = new MongoServerAddress("varzeamanager.documents.azure.com", 10250),
                Username = "varzeamanager",
                Password = "KWgMlMMb0LGTIkBQgBWsZktFhPWHD4eIshVAg4lQbBqw29cBf5zNNMVhFv8pvz245wrEcDKe3JnGFb3GsqOY8A==",
                Journal = true,
                UseSsl = true,
                VerifySslCertificate = false
            };

            var settings = MongoClientSettings.FromUrl(mongoUrlBuilder.ToMongoUrl());
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };



            //var settings = MongoClientSettings.FromUrl(MongoUrl.Create(connectionString));

            var client = new MongoClient(settings);
            Database = client.GetDatabase(databaseName);

            RegistrarConvencoes();
            RegistrarMapeamentos();
        }

        private void RegistrarConvencoes()
        {
            //var pack = new ConventionPack();
            //pack.Add(new CamelCaseElementNameConvention());


            //ConventionRegistry.Register(
            //   "My Custom Conventions",
            //   pack,
            //   t => t.FullName.StartsWith("MyNamespace."));



            // Class Stage: IClassMapConvention => Run against the class map.

            // Member Stage: IMemberMapConvention => Run against each member map discovered during the Class stage.

            // Creator Stage: ICreatorMapConvention => Run against each CreatorMap discovered during the Class stage.

            // Post Processing Stage: IPostProcessingConvention => Run against the class map.

            // If a custom implementation of an IPostProcessingConvention is registered before a customer implementation of an 
            // IClassMapConvention, the IClassMapConvention will be run first because the Class Stage is before the Post Processing Stage.

            //public class LowerCaseElementNameConvention : IMemberMapConvention
            //{
            //    public void Apply(BsonMemberMap memberMap)
            //    {
            //        memberMap.SetElementName(memberMap.MemberName.ToLower());
            //    }
            //}

            //var pack = new ConventionPack();
            //pack.AddMemberMapConvention("LowerCaseElementName", m => m.SetElementName(m.MemberName.ToLower()));
        }

        /// <summary>
        /// Registra todas classes de mapeamento das entidades do mongo Db
        /// </summary>
        /// <remarks>
        /// Isso só acontece uma vez, durante a inicialização da aplicação
        /// </remarks>
        private static void RegistrarMapeamentos()
        {
            //How you get this assembly is up to you
            //It could be this assembly
            //Or it could be a collection of assemblies, in which case, wrap this block in a foreach and iterate
            var assembly = typeof(EntityMap).GetTypeInfo().Assembly;

            //get all types that have our MongoDbClassMap as their base class
            var classMaps = assembly
                .GetTypes()
                .Where(t => t.GetTypeInfo().BaseType != null && t.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                            t.GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(BsonClassMap<>));

            //automate the new *ClassMap()
            foreach (var classMap in classMaps)
            {
                if (BsonClassMap.IsClassMapRegistered(classMap))
                    continue;

                var instance = Activator.CreateInstance(classMap) as BsonClassMap;
                if (instance == null)
                    continue;

                BsonClassMap.RegisterClassMap(instance);
            }
        }

        #endregion

        #region Mongo Collections

        public IMongoCollection<T> GetCollection<T>() where T : AggregateRoot
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }

        public IMongoCollection<Jogador> Jogadores => GetCollection<Jogador>();

        #endregion
    }
}