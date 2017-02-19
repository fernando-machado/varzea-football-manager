using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Persistence.Mapeamentos.Jogadores
{
    public class JogadorMap : BsonClassMap<Jogador>
    {
        public JogadorMap()
        {
            this.AutoMap();

            this.MapMember(x => x.Posicao)
                .SetSerializer(new EnumSerializer<Posicao>(BsonType.String));

            this.MapMember(c => c.Nivel)
                .SetSerializer(new EnumSerializer<Nivel>(BsonType.String));
        }
    }
}
