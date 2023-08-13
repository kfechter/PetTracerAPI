using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracerAPI.Messaging;
using PetTracerAPI.Models;
using PetTracerAPI.Services;

namespace PetTracerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PetController : ControllerBase
	{
        private readonly PetsService _petsservice;
        private readonly ILogger<PetController> _logger;
        private readonly IMessageProducer _messagePublisher;


        public PetController(PetsService petsService, ILogger<PetController> logger, IMessageProducer messagePublisher)
		{
            _petsservice = petsService;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterPet([FromBody] Pet newPet)
        {
            newPet.PetHash = "";
            await _petsservice.CreateAsync(newPet);
            _messagePublisher.SendMessage(newPet);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePet(string id)
        {
            var petToDelete = await _petsservice.GetAsync(id);
            if(petToDelete == null)
            {
                return NotFound();
            }

            await _petsservice.RemoveAsync(id);
            return NoContent();
        }


    }
}