using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Audit.API.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Title { get; set; }
        public string RawData { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}