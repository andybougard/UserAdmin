using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserAdmin.MongoEntities
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        [BsonElement("movie_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? movie_id { get; set; }
        public string? text { get; set; }
        public DateTime? date { get; set; }
    }
}