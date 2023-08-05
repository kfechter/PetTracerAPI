using System;
namespace PetTracerAPI.Models
{
	public class PetTracerDatabaseSettings
	{
        public string UsersCollectionName { get; set; } = null!;

        public string PetsCollectionName { get; set; } = null!;
    }
}