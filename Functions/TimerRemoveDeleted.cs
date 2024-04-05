using Azure.Storage.Queues;
using GraphDocsConnector.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class TimerRemoveDeleted
    {
        private readonly ILogger _logger;
        private readonly QueueClient _queueContentClient;

        public TimerRemoveDeleted(QueueClient queueClient, ILoggerFactory loggerFactory)
        {
            _queueContentClient = queueClient;
            _logger = loggerFactory.CreateLogger<TimerRemoveDeleted>();
        }

        [Function("TimerRemoveDeleted")]
        public async Task Run([TimerTrigger("10 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Enqueueing request for incremental crawl...");
            await Queue.StartCrawl(_queueContentClient, CrawlType.Incremental);
        }
    }
}
