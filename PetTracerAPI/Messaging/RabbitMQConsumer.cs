using System;
using Newtonsoft.Json;
using PetTracerAPI.Services;

namespace PetTracerAPI.Messaging
{
	public class RabbitMQConsumer
	{
        private readonly PetsService _petsservice;

        public RabbitMQConsumer(PetsService petsService)
		{
			_petsservice = petsService;
		}

		public async void ConsumeMessage(string messageBody)
		{
            // Get The pet by ID, then update it with the file name
            PetUpdateDTO petRecord = JsonConvert.DeserializeObject<PetUpdateDTO>(messageBody)!;

			if(petRecord != null)
			{
				var pet = _petsservice.GetAsync(petRecord.petId!).Result;
				if(pet != null)
				{
					pet.StlFileName = petRecord.fileName!;
					await _petsservice.UpdateAsync(petRecord.petId!, pet);
				}
			}
        }
	}
}

