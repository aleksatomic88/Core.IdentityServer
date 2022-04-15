using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.Core.Service.Interface;
using Common.Model;
using Core.Users.Service;
using Common.Model.Search;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    // [Authorize]
#pragma warning disable CS1591
    public class UsersController : ControllerBase
    {

        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        /// <summary>
        /// Return an User by ID
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        public async Task<Response<UserResponse>> Get(int id)
        {
            var user = await _service.Get(id);

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
        //[Authorize(Roles = "super-admin, admin")]
        public async Task<Response<UserResponse>> Register([FromBody] RegisterUserCommand cmd)
        {
            var user = await _service.Create(cmd);

            return await Get(user.Id);
        }

        /// <summary>
        /// Delete an User
        /// </summary>s
        [HttpDelete("{id}")]
        //[Authorize(Roles = "super-admin, admin")]
        public async Task<Response<bool>> Delete(int id)
        {
            var result = await _service.Delete(id);

            return new Response<bool>(result);
        }
    }
}
