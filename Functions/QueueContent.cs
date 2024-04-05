using System.Diagnostics;
using System.Text.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using GraphDocsConnector.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace GraphDocsConnector.Functions
{
    public class QueueContent
    {
        private readonly ILogger<QueueContent> _logger;
        private readonly GraphServiceClient _graphClient;
        private readonly QueueClient _queueContentClient;

        public QueueContent(GraphServiceClient graphServiceClient, QueueClient queueClient, ILogger<QueueContent> logger)
        {
            _graphClient = graphServiceClient;
            _queueContentClient = queueClient;
            _logger = logger;
        }

        [Function(nameof(QueueContent))]
        public async Task Run([QueueTrigger("queue-content", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            var contentMessage = JsonSerializer.Deserialize<ContentMessage>(message.MessageText);
            if (contentMessage is null)
            {
                _logger.LogError($"Couldn't deserialize message: {message.MessageText}");
                return;
            }

            Debug.Assert(contentMessage.Action is not null);

            switch (contentMessage.Action)
            {
                case ContentAction.Crawl:
                    await Crawl(contentMessage.CrawlType);
                    break;
                case ContentAction.Item:
                    await ProcessItem(contentMessage.ItemId, contentMessage.ItemAction);
                    break;
            }
        }

        private async Task ProcessItem(string? itemId, ItemAction? itemAction)
        {
            if (itemAction is null)
            {
                _logger.LogError("itemAction is null");
                return;
            }

            if (itemId is null)
            {
                _logger.LogError("itemId is null");
                return;
            }

            switch (itemAction)
            {
                case ItemAction.Update:
                    await UpdateItem(itemId);
                    break;
                case ItemAction.Delete:
                    await DeleteItem(itemId);
                    break;
            }
        }

        private async Task DeleteItem(string itemId)
        {
            // TODO
            throw new NotImplementedException();
        }

        private async Task UpdateItem(string itemId)
        {
            // TODO
            throw new NotImplementedException();
        }

        private async Task Crawl(CrawlType? crawlType)
        {
            if (crawlType is null)
            {
                _logger.LogError("crawlType is null");
                return;
            }

            switch (crawlType)
            {
                case CrawlType.Full:
                case CrawlType.Incremental:
                    await CrawlFullOrIncremental(crawlType);
                    break;
                case CrawlType.RemoveDeleted:
                    await RemoveDeleted();
                    break;
            }
        }

        private async Task RemoveDeleted()
        {
            // TODO
            throw new NotImplementedException();
        }

        private async Task CrawlFullOrIncremental(CrawlType? crawlType)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
