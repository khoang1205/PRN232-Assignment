using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PRN232_Assignment.DoctorService.Repository.Entities
{
    public class Doctor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("fullName")]
        public string FullName { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("specialty")]
        public string Specialty { get; set; } = null!;

        [BsonElement("bio")]
        public string Bio { get; set; } = string.Empty;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
