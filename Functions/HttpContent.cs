using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace dotnet_csharp_graphdocs.Functions
{
    public class HttpContent
    {
        private readonly ILogger<HttpContent> _logger;

        public HttpContent(ILogger<HttpContent> logger)
        {
            _logger = logger;
        }

        [Function("crawl")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
