using Azure.Storage.Queues;
using GraphDocsConnector.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class TimerIncrementalCrawl
    {
        private readonly ILogger _logger;
        private readonly QueueClient _queueContentClient;

        public TimerIncrementalCrawl(QueueServiceClient queueClient, ILoggerFactory loggerFactory)
        {
            _queueContentClient = queueClient.GetQueueClient("queue-content");
            _logger = loggerFactory.CreateLogger<TimerIncrementalCrawl>();
        }

        [Function("TimerIncrementalCrawl")]
        public async Task Run([TimerTrigger("0 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Enqueueing request for incremental crawl...");
            await _queueContentClient.CreateIfNotExistsAsync();
            await Queue.StartCrawl(_queueContentClient, CrawlType.Incremental);
        }
    }
}
