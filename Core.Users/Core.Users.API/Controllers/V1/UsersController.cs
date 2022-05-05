using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.Core.Service.Interface;
using Common.Model;
using Core.Users.Service;
using Common.Model.Search;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
        public async Task<Response<SearchResponse<UserBasicResponse>>> Search([FromQuery] UserQuery searchQuery)
        {
            var users = await _service.Search(searchQuery);

            return new Response<SearchResponse<UserBasicResponse>>(users);
        }

        /// <summary>
        /// Register an User from Admin Application
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
        [HttpPut("{id}")]
        [Authorize(Roles = "super-admin, admin")]
        public async Task<Response<UserResponse>> Update(string id, [FromBody] UpdateUserCommand cmd)
        {
            var user = await _service.Update(_hashids.DecodeSingle(id), cmd);

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

        /// <summary>
        /// SignUp an User
        /// </summary>s
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<Response<UserResponse>> SignUp([FromBody] RegisterUserCommand cmd)
        {
            // Add CUSTOMER role as default 
            cmd.Roles = new List<string>() { "customer" };

            var user = await _service.Create(cmd);

            return await Get(_hashids.Encode(user.Id));
        }

        /// <summary>
        /// Email Verification
        /// </summary>s
        [HttpPost("email-verification")]
        [AllowAnonymous]
        public async Task<Response<bool>> EmailVerification([FromBody] EmailVerificationCommand cmd)
        {
            var result = await _service.EmailVerification(cmd);

            return new Response<bool>(result);
        }

        /// <summary>
        /// Resend Email Verification
        /// </summary>s
        [HttpPost("resend-email-verification/{email}")]
        [AllowAnonymous]
        public async Task<Response<bool>> ResendEmailVerification(string email)
        {
            var token = await _service.ResendEmailVerification(email);

            return new Response<bool>(!string.IsNullOrEmpty(token));
        }

        /// <summary>
        /// Reset Password - starts/restarts reset password process
        /// </summary>s
        [HttpPost("reset-password/{email}")]
        [AllowAnonymous]
        public async Task<Response<bool>> ResetPassword(string email)
        {
            var token = await _service.ResetPassword(email);           

            return new Response<bool>(!string.IsNullOrEmpty(token));
        }

        /// <summary>
        /// Change Password - update user password
        /// </summary>s
        [HttpPost("change-password")]
        [AllowAnonymous]
        public async Task<Response<bool>> ChangePassword([FromBody] ChangePasswordCommand cmd)
        {
            var result = await _service.ChangePassword(cmd);

            return new Response<bool>(result);
        }

        /// <summary>
        /// Quick Validatate if an User with {field} having {value} exists
        /// </summary>s
        [HttpPost("quick-validation")]
        [AllowAnonymous]
        public async Task<Response<bool>> QuickValidation([FromQuery] string field, [FromQuery] string value)
        {
            var result = await _service.QuickValidation(field, value);

            return new Response<bool>(result);
        }
    }
}
