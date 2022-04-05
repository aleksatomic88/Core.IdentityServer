using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Domain.Model
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }

        public List<User> RoleUsers { get; set; }
    }
}
