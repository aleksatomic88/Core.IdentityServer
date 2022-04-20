using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace User.Functions
{
    public class EmailNotification
    {
        [FunctionName("EmailNotification")]
        public void Run([QueueTrigger("myqueue-items", Connection = "queue-conn-string")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
