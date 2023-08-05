using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracerAPI.Messaging;
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



	}
}