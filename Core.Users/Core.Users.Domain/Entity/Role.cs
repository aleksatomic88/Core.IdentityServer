using System.Collections.Generic;

namespace Core.Users.Domain
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
