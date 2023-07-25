using System;
namespace PetTracerAPI.Models
{
	public class PetTracerDatabaseSettings
	{
        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;

        public string PetsCollectionName { get; set; } = null!;
    }
}