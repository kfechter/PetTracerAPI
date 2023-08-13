using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetTracerAPI.Models
{
	public class Notification
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? OwnerId { get; set; }

        public string? MessageBody { get; set; }

        public bool Unread { get; set; }
    }
}

