using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.Domain.Command.User
{
    public class RegisterUserCommand // : BaseCommand
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public List<int> Roles { get; set; } = new List<int>();
    }
}
