using EDD.Domain.Model;
using Prato.IdentityProvider.Entities;
using System.Collections.Generic;

namespace Identity.Domain.Model
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
