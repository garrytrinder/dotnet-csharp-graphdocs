using Azure.Storage.Queues;
using GraphDocsConnector.Messages;
using System.Text.Json;

namespace GraphDocsConnector
{
    internal static class Queue
    {
        public static async Task EnqueueCheckStatus(QueueClient queueClient, string location)
        {
            var message = new ConnectionMessage
            {
                Action = ConnectionMessageAction.Status,
                Location = location
            };

            await queueClient.SendMessageAsync(
                JsonSerializer.Serialize(message),
                TimeSpan.FromSeconds(int.Parse(Environment.GetEnvironmentVariable("GRAPH_SCHEMA_STATUS_INTERVAL") ?? "60"))
            );
        }

        public static async Task StartCrawl(QueueClient queueClient, CrawlType crawlType)
        {
            throw new NotImplementedException();
        }
    }
}
