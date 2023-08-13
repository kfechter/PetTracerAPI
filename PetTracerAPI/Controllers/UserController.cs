using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracerAPI.Models;
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

        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            var user = await _usersService.GetUserByEmailAsync(email);
            if(user == null)
            {
                return NotFound();
            }

            var pets = await _petsservice.GetByOwnerIdAsync(user.Id!);
            if(pets != null)
            {
                foreach(var userPet in pets)
                {
                    await _petsservice.RemoveAsync(userPet.Id!);
                }
            }

            await _usersService.RemoveAsync(user.Id!);

            return NoContent();
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetOrRegisterUser(string email)
        {
            var user = await _usersService.GetUserByEmailAsync(email);

            if(user == null)
            {
                // Register the user
                User newUser = new()
                {
                    Email = email,
                    NewUser = true
                };

                await _usersService.CreateAsync(newUser);
                return newUser;
            }

            user.NewUser = false;
            return user!;
        }
    }


}

