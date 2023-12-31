﻿using System;
using PetTracerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PetTracerAPI.Services
{
	public class UsersService
	{
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(IOptions<PetTracerDatabaseSettings> petTracerDatabaseSettings)
		{
            var mongoClient = new MongoClient($"mongodb://{Environment.GetEnvironmentVariable("MongoDBUsername")}:{Environment.GetEnvironmentVariable("MongoDBPassword")}@{Environment.GetEnvironmentVariable("MongoServer")}:27017");
            var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));
            _usersCollection = mongoDatabase.GetCollection<User>(petTracerDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() => await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserByEmailAsync(string email) => await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetAsync(string id) => await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) => await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) => await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) => await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}