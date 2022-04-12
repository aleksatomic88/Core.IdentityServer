using Core.Users.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Users.Core.Service;
using Users.Core.Service.Interface;
using Common.Model;
using System.Collections.Generic;
using Core.Users.Service;
using Microsoft.AspNetCore.Authorization;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
#pragma warning disable CS1591
    public class UsersController : ControllerBase
    {

        private readonly IBaseService<User, UserResponse> _service;

        public UsersController(IBaseService<User, UserResponse> userService)
        {
            _service = userService;
        }

        /// <summary>
        /// Return a User by ID
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Response<UserResponse>>> Get(int id)
        {
            try
            {
                var user = await _service.Get(id, new string[] { "UserRoles.Role" });

                return new Response<UserResponse>(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {JsonConvert.SerializeObject(ex)}");
            }
        }

        /// <summary>
        /// Return All Users
        /// </summary>s
        [HttpGet]
        public async Task<ActionResult<Response<List<UserResponse>>>> GetAll()
        {
            try
            {
                var users = await _service.GetAll(new string[] { "UserRoles.Role" });

                return new Response<List<UserResponse>>(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {JsonConvert.SerializeObject(ex)}");
            }
        }

        /// <summary>
        /// Register an User
        /// </summary>s
        [HttpPost]
        //[Authorize(Roles = "super-admin, admin")]
        public async Task<ActionResult<Response<UserResponse>>> Register([FromBody] RegisterUserCommand cmd)
        {
            var user = await ((UserService)_service).Create(cmd);

            return await Get(user.Id);
        }
    }
}
