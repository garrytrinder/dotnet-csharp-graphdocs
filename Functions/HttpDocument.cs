using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class HttpDocument
    {
        // TODO: refactor to use blob storage instead files from disk

        private readonly ILogger<HttpDocuments> _logger;

        public HttpDocument(ILogger<HttpDocuments> logger)
        {
            _logger = logger;
        }

        [Function("document")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "documents/{docId}")] HttpRequest req, string docId)
        {
            if (docId.StartsWith(".") || !docId.EndsWith(".md"))
            {
                return new BadRequestResult();
            }
            
            var contentFolder = "content";
            var contentFolderPath = Path.Combine(Directory.GetCurrentDirectory(), contentFolder);
            var filePath = Path.Combine(contentFolderPath, docId.Replace("__", Path.DirectorySeparatorChar.ToString()));
            if (!File.Exists(filePath))
            {
                return new NotFoundResult();
            }

            var content = File.ReadAllText(filePath);

            return new OkObjectResult(new FileInfo
            {
                Id = docId,
                LastModified = File.GetLastWriteTimeUtc(filePath),
                Contents = content
            });
        }
    }
}
