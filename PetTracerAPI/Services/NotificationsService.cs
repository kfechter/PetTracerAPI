using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PetTracerAPI.Models;

namespace PetTracerAPI.Services
{
	public class NotificationsService
	{
        private readonly IMongoCollection<Notification> _notificationsCollection;

        public NotificationsService(IOptions<PetTracerDatabaseSettings> petTracerDatabaseSettings)
		{
            var mongoClient = new MongoClient($"mongodb://{Environment.GetEnvironmentVariable("MongoServer")}:27017");
            var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));
            _notificationsCollection = mongoDatabase.GetCollection<Notification>(petTracerDatabaseSettings.Value.NotificationsCollectionName);
        }

        public async Task<List<Notification>> GetAsync() => await _notificationsCollection.Find(_ => true).ToListAsync();

        public async Task<List<Notification>> GetByOwnerIdAsync(string ownerId) => await _notificationsCollection.Find(x => x.OwnerId == ownerId).ToListAsync();

        public async Task<Notification?> GetAsync(string id) => await _notificationsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Notification newNotification) => await _notificationsCollection.InsertOneAsync(newNotification);

        public async Task UpdateAsync(string id, Notification updatedNotification) => await _notificationsCollection.ReplaceOneAsync(x => x.Id == id, updatedNotification);

        public async Task RemoveAsync(string id) => await _notificationsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

