using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using User.Functions.Domain.Interfaces;
using User.Functions.Domain.Model;

namespace User.Functions.EmailNotification
{
    public class EmailNotification
    {
        private readonly IMailJetService _mailJetService;

        public EmailNotification(IMailJetService mailJetService)
        {
            _mailJetService = mailJetService;
        }

        [FunctionName("EmailNotification")]
        public async Task Run([ServiceBusTrigger(topicName: "%EveryTableTopicName%", subscriptionName: "%EveryTableSubscripionName%", Connection = "EveryTableSBConnectionString")] ServiceBusReceivedMessage message)
        {
            var email = JsonConvert.DeserializeObject<UserVerificationEmail>(Encoding.UTF8.GetString(message.Body));
            await _mailJetService.SendMailAsync(email);
        }
    }
}
