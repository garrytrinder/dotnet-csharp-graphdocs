using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace dotnet_csharp_graphdocs.Functions
{
    public class HttpNotification
    {
        private readonly ILogger<HttpNotification> _logger;

        public HttpNotification(ILogger<HttpNotification> logger)
        {
            _logger = logger;
        }

        [Function("notification")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
