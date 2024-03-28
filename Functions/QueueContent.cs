using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class QueueContent
    {
        private readonly ILogger<QueueContent> _logger;

        public QueueContent(ILogger<QueueContent> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueContent))]
        public void Run([QueueTrigger("contentQueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
