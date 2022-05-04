using AutoMapper;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using User.Functions.Domain.Interfaces;
using User.Functions.Domain.Model;

namespace User.Functions.Domain.Services
{
    public class MailJetService : IMailJetService
    {
        private readonly ILogger<MailJetService> _logger;

        public MailJetService(ILogger<MailJetService> logging, IMapper mapper)
        {
            _logger = logging;
        }
        public async Task SendMailAsync(UserVerificationEmail email)
        {
            var client = GetMailClient();
            var request = CreateMailRequest(email);

            MailjetResponse response = await client.PostAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                _logger.LogInformation(response.GetData().ToString());
            }
            else
            {
                _logger.LogError(string.Format("StatusCode: {0}\n", response.StatusCode));
                _logger.LogError(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                _logger.LogError(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }

        private MailjetClient GetMailClient()
        {
            return new MailjetClient("70c46443720a87b58c4ef7d859de16c6",
            "efe21e85bd35ae8ad00ef51959584830");
        }
        private MailjetRequest CreateMailRequest(UserVerificationEmail email)
        {
            // var emailAdress = "milos.mijatovic88@gmail.com";
            // var name = "Milos";
            var url = "https://everytable.com/";

            return new MailjetRequest
            {
                Resource = Send.Resource,
            }
                 .Property(Send.FromEmail, "aleksa@jupiterlabs.co") // do not change
                 .Property(Send.FromName, "Everytable")
                 .Property(Send.Subject, "Email verification")
                 .Property(Send.MjTemplateID, "3882699")
                 .Property(Send.MjTemplateLanguage, "True")
                 //.Property(Send.MjTemplateErrorReporting, "aleksa@jupiterlabs.co")
                 //.Property(Send.MjTemplateErrorDeliver, "true")
                 .Property(Send.Recipients, new JArray
                 {
                    new JObject {
                                {"Email", email.Email},
                                {"Name", email.FirstName},
                                {"Vars", new JObject {{"name", email.FirstName },{"url", url}}}
                                }
                  });
        }
    }
}
