using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetTracerAPI.Models
{
	public class Pet
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string PetName { get; set; }


        public string StlFileName { get; set; }


        public string PetHash { get; set; }


        public string? OwnerId { get; set; }
    }
}

