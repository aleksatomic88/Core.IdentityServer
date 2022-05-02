using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace User.Functions
{
    public class EmailNotification
    {
        [FunctionName("EmailNotification")]
        public void Run([ServiceBusTrigger(topicName:"%EveryTableTopicName%", subscriptionName:"%EveryTableSubscripionName%" , Connection = "EveryTableSBConnectionString")]
         object  sbMsg, ILogger log)
        {
            log.LogInformation($"C# Service bus topic trigger function processed: {sbMsg}");
        }
    }
}
