using MongoDB.Driver;
using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Domain.Core
{
    public interface IDatabase
    {
        IMongoCollection<T> GetCollection<T>() where T : AggregateRoot;

        IMongoCollection<Jogador> Jogadores { get; }
    }
}
