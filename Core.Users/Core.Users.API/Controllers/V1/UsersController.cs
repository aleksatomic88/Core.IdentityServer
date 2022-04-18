using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.Core.Service.Interface;
using Common.Model;
using Core.Users.Service;
using Common.Model.Search;
using HashidsNet;
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

        private readonly IUserService _service;
        private readonly IHashids _hashids;

        public UsersController(IUserService userService,
                               IHashids hashids)
        {
            _service = userService;
            _hashids = hashids;
        }

        /// <summary>
        /// Return an User by ID
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        public async Task<Response<UserResponse>> Get(string id)
        {
            var user = await _service.Get(_hashids.DecodeSingle(id));

            return new Response<UserResponse>(user);
        }

        /// <summary>
        /// Return All Users
        /// </summary>s
        [HttpGet]
        public async Task<Response<SearchResponse<UserBasicResponse>>> Search([FromQuery]UserQuery searchQuery)
        {
            var users = await _service.Search(searchQuery);

            return new Response<SearchResponse<UserBasicResponse>>(users);            
        }

        /// <summary>
        /// Register an User
        /// </summary>s
        [HttpPost]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<UserResponse>> Create([FromBody] RegisterUserCommand cmd)
        {
            var user = await _service.Create(cmd);

            return await Get(_hashids.Encode(user.Id));
        }

        /// <summary>
        /// Update an User
        /// </summary>s
        [HttpPut]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<UserResponse>> Update([FromBody] UpdateUserCommand cmd)
        {
            var user = await _service.Update(_hashids.DecodeSingle(cmd.Hid), cmd);

            return await Get(_hashids.Encode(user.Id));
        }

        /// <summary>
        /// Delete an User
        /// </summary>s
        [HttpDelete("{id}")]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<bool>> Delete(string id)
        {
            var result = await _service.Delete(_hashids.DecodeSingle(id));

            return new Response<bool>(result);
        }
    }
}
