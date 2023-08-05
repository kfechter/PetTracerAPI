using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PetTracerAPI.Models;

namespace PetTracerAPI.Services
{
	public class PetsService
	{
        private readonly IMongoCollection<Pet> _petsCollection;

        public PetsService(IOptions<PetTracerDatabaseSettings> petTracerDatabaseSettings)
		{
            var mongoClient = new MongoClient($"mongodb://{Environment.GetEnvironmentVariable("MongoServer")}:27017");
            var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));
            _petsCollection = mongoDatabase.GetCollection<Pet>(petTracerDatabaseSettings.Value.PetsCollectionName);
        }

        public async Task<List<Pet>> GetAsync() => await _petsCollection.Find(_ => true).ToListAsync();

        public async Task<List<Pet>> GetByOwnerIdAsync(string ownerId) => await _petsCollection.Find(x => x.OwnerId == ownerId).ToListAsync();

        public async Task<Pet?> GetAsync(string id) => await _petsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Pet newPet) => await _petsCollection.InsertOneAsync(newPet);

        public async Task UpdateAsync(string id, Pet updatedPet) => await _petsCollection.ReplaceOneAsync(x => x.Id == id, updatedPet);

        public async Task RemoveAsync(string id) => await _petsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

