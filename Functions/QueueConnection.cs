using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class QueueConnection
    {
        private readonly ILogger<QueueConnection> _logger;

        public QueueConnection(ILogger<QueueConnection> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueConnection))]
        public void Run([QueueTrigger("connectionQueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
