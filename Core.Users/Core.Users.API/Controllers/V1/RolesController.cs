using AutoMapper;
using Core.Users.DAL;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain;
using Core.Users.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Core.Service.Interface;

namespace Core.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
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
        [Authorize(Roles = "super-admin, admin")]
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
