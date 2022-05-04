using System.Threading.Tasks;
using User.Functions.Domain.Model;

namespace User.Functions.Domain.Interfaces
{
    public interface IMailJetService
    {
        public Task SendMailAsync(UserVerificationEmail emailObject);
    }
}
