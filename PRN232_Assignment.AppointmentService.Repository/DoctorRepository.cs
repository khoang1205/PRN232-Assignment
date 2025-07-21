using MongoDB.Bson;
using MongoDB.Driver;
using PRN232_Assignment.DoctorService.Repository.Entities;
using PRN232_Assignment.DoctorService.Repository.IRepository;

namespace PRN232_Assignment.DoctorService.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IMongoCollection<Doctor> _collection;

        public DoctorRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Doctor>("Doctors");
        }

        public Task<List<Doctor>> GetAllAsync() =>
            _collection.Find(_ => true).ToListAsync();

        public Task CreateAsync(Doctor doctor) =>
            _collection.InsertOneAsync(doctor);

        public async Task<Doctor?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, Doctor doctor)
        {
            doctor.Id = id;
            var result = await _collection.ReplaceOneAsync(d => d.Id == id, doctor);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(d => d.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<Doctor>> SearchAsync(string? name, string? specialty)
        {
            var filters = new List<FilterDefinition<Doctor>>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                filters.Add(Builders<Doctor>.Filter.Regex(d => d.FullName, new BsonRegularExpression(name, "i")));
            }

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                filters.Add(Builders<Doctor>.Filter.Regex(d => d.Specialty, new BsonRegularExpression(specialty, "i")));
            }

            var filter = filters.Count > 0
                ? Builders<Doctor>.Filter.And(filters)
                : Builders<Doctor>.Filter.Empty;

            return await _collection.Find(filter).ToListAsync();
        }

    }
}
