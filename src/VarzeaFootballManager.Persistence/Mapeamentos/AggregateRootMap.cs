using MongoDB.Bson.Serialization;
using VarzeaFootballManager.Domain.Core;

namespace VarzeaFootballManager.Persistence.Mapeamentos
{
    public class AggregateRootMap : BsonClassMap<AggregateRoot>
    {
        public AggregateRootMap()
        {
            this.AutoMap();
        }
    }
}
