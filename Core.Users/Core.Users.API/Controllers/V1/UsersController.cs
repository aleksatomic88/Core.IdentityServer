using Core.Users.Domain.Model;
using Core.Users.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Users.Core.Service;
using Users.Core.Service.Interface;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
#pragma warning disable CS1591
    public class UsersController : ControllerBase
    {

        private readonly IBaseService<User, UserResponse> _userService;

        public UsersController(IBaseService<User, UserResponse> userService)
        {
            _userService = userService;
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
                var user = await _userService.Get(id, new string[] { "UserRoles.Role" });
                var userCustom = await ((UserService)_userService).Get(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {JsonConvert.SerializeObject(ex)}");
            }
        }

        /// <summary>
        /// Return All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll(new string[] { "UserRoles.Role" });

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}
