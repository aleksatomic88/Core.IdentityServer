using AutoMapper;
using Core.Users.DAL;
using Core.Users.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "super-admin, admin")]
#pragma warning disable CS1591
    public class RolesController : ControllerBase
    {

        private readonly UsersDbContext _ctx;
        private readonly IMapper _mapper;

        public RolesController(UsersDbContext ctx,
                               IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        /// <summary>
        /// Return All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _ctx.Roles.ToListAsync();

                var roles = _mapper.Map<List<RoleResponse>>(result);

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error. {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}
