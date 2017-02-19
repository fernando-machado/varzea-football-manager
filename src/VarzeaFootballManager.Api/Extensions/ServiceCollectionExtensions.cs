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
        /// <param name="self"></param>
        /// <param name="config">Configuracoes do AppSettings</param>
        /// <returns>Returs <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddMongoDb(this IServiceCollection self, IConfigurationRoot config)
        {
            var configsMongoDb = config.GetSection("ConfiguracoesMongoDb");
            var connectionString = configsMongoDb.GetValue<string>("ConnectionString");
            var databaseName = configsMongoDb.GetValue<string>("DatabaseName");

            return self.AddSingleton<IDatabase>(new MongoDatabase(connectionString, databaseName));
        }
    }
}
