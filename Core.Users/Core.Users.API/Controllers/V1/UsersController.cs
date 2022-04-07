using Core.Users.DAL.Entities;
using Core.Users.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Users.Core.Service.Interface;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    //[Authorize]
#pragma warning disable CS1591
    public class UsersController : ControllerBase
    {

        private readonly IBaseService<WeatherForecasts, UserResponse> _usersService;

        public UsersController(IBaseService<WeatherForecasts, UserResponse> usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Return a User by ID
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _usersService.Get(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {(JsonConvert.SerializeObject(ex))}");
            }
        }

        /// <summary>
        /// Return All Users
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var users = await _usersService.GetAll();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    { 
        //        return BadRequest($"Unhandled error. {(JsonConvert.SerializeObject(ex))}");
        //    }
        //}
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok("Authorized!!!");
        }
    }
}
