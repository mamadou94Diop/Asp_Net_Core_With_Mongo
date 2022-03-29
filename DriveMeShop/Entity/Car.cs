using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DriveMeShop.Entity
{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int ReleasedYear { get; set; }
        public int? LastRevisionYear { get; set; }
        public string TransmissionMode { get; set; }
        public string Color { get; set; }
        public int MaxSpeed { get; set; }
    }
}
    