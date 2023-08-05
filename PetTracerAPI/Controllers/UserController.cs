using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracerAPI.Services;

namespace PetTracerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly PetsService _petsservice;
        private readonly ILogger<UserController> _logger;

        public UserController(UsersService usersService, PetsService petsService, ILogger<UserController> logger)
		{
			_usersService = usersService;
			_petsservice = petsService;
			_logger = logger;
		}
	}


}

