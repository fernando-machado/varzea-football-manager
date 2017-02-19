using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using VarzeaFootballManager.Domain.Core;

namespace VarzeaFootballManager.Persistence.Mapeamentos
{
    public class EntityMap : BsonClassMap<Entity>
    {
        public EntityMap()
        {
            this.AutoMap();

            this.MapIdField(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);

            this.MapMember(p => p.CreatedAt)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime));

            this.SetIgnoreExtraElements(true);
            this.MapExtraElementsMember(c => c.CatchAll);



            //this.MapMember(p => p.ModifiedAt)
            //    .SetIgnoreIfNull(true)
            //    //.SetDefaultValue(new DateTime(1900, 01, 01))
            //    //.SetSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime))
            //    //.SetShouldSerializeMethod(obj =>
            //    //{
            //    //    var entity = obj as Entity;
            //    //    return entity?.ModifiedAt != null && entity.ModifiedAt.Value > new DateTime(1900, 1, 1);
            //    //})
            //;

            //this.MapIdMember(p => p.Id)
            //    .SetOrder(0)
            //    .SetIdGenerator(StringObjectIdGenerator.Instance)
            //    //.SetSerializer(new ObjectIdSerializer(BsonType.ObjectId))
            //    ;

            //this.MapMember(p => p.CreatedOn)
            //    .SetSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime));

            //this.UnmapField(p => p.CreatedOn);

            //this.MapMember(c => c.Posicao)
            //    .SetElementName("Position")
            //    .SetSerializer(new EnumSerializer<Posicao>(BsonType.String));

            //this.MapMember(c => c.Nivel)
            //    .SetOrder(2)
            //    .SetElementName("Level");

            //this.MapExtraElementsMember(c => c.CatchAll);


            //[BsonIgnoreExtraElements]
            //this.SetIgnoreExtraElements(true);

            //[BsonExtraElements]
            //this.MapExtraElementsMember(c => c.CatchAll);

            //[BsonDiscriminator("myclass")]
            //this.SetDiscriminator("Jogador");

            //[BsonConstructor]
            //this.MapCreator(p => new Jogador(p.Name));

            //[BsonElement]
            //public string SomeProperty { get { return _someReadOnlyProperty; } }
            //this.MapProperty(c => c.SomeProperty); //When a readonly property is serialized, it value is persisted to the database, but never read back out. This is useful for storing �computed� properties.

            //[BsonDefaultValue("abc")]
            //this.MapMember(c => c.SomeProperty).SetDefaultValue("abc");

            //[BsonElement("sp", Order = 1)]
            //this.MapMember(c => c.SomeProperty).SetElementName("sp").SetOrder(1);

            //[BsonId(IdGenerator = typeof(CombGuidGenerator))]
            //this.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);

            //You could also say that you want to use the CombGuidGenerator for all Guids.
            //BsonSerializer.RegisterIdGenerator(typeof(Guid), CombGuidGenerator.Instance);

            //[BsonIgnore]
            //this.UnmapMember(c => c.SomeProperty);

            //[BsonIgnoreIfDefault]
            //this.MapMember(c => c.CreatedBy).SetIgnoreIfDefault(true);

            //[BsonDefaultValue("abc")]
            //this.MapMember(c => c.SomeProperty).SetDefaultValue("abc");

            //[BsonDateTimeOptions(DateOnly = true)]  ShouldSerializeCreatedOn() => CreatedOn > new DateTime(1900, 1, 1);
            //this.MapMember(c => c.CreatedOn)
            //    .SetSerializer(new DateTimeSerializer(dateOnly: true))
            //    .SetShouldSerializeMethod(obj => ((Jogador)obj).CreatedOn > new DateTime(1900, 1, 1));

            //[BsonDateTimeOptions(DateOnly = true)]
            //this.MapMember(c => c.DateOfBirth).SetSerializer(new DateTimeSerializer(dateOnly: true));

            //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
            //this.MapMember(c => c.AppointmentTime).SetSerializer(new DateTimeSerializer(DateTimeKind.Local));

            //[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
            //this.MapMember(c => c.Values).SetSerializer(new DictionaryInterfaceImplementerSerializer<Dictionary<string, int>>(DictionaryRepresentation.ArrayOfDocuments));

            //[BsonRepresentation(BsonType.Int32)]
            //this.MapMember(c => c.RepresentAsInt32).SetSerializer(new CharSerializer(BsonType.Int32));

            //[BsonRepresentation(BsonType.String)]
            //this.MapMember(c => c.RepresentAsString).SetSerializer(new CharSerializer(BsonType.String));

            //[BsonRepresentation(BsonType.ObjectId)]
            //this.IdMemberMap.SetRepresentation(BsonType.ObjectId);

            //[BsonRepresentation(BsonType.String)]
            //this.MapMember(c => c.FavoriteColor).SetSerializer(new EnumSerializer<Color>(BsonType.String));


            //BsonClassMap.RegisterClassMap<Animal>(cm => {
            //    cm.AutoMap();
            //    cm.SetIsRootClass(true);
            //});
            //BsonClassMap.RegisterClassMap<Cat>();
            //BsonClassMap.RegisterClassMap<Dog>();
            //BsonClassMap.RegisterClassMap<Lion>();
            //BsonClassMap.RegisterClassMap<Tiger>();
        }
    }
}
