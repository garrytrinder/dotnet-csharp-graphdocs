using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class HttpDocuments
    {
        private readonly ILogger<HttpDocuments> _logger;

        public HttpDocuments(ILogger<HttpDocuments> logger)
        {
            _logger = logger;
        }

        [Function("documents")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
