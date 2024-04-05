using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class HttpDocuments
    {
        // TODO: refactor to a CRUD API mocked with Dev Proxy
        // use to generate mocks JSON structure with file contents

        private readonly ILogger<HttpDocuments> _logger;

        public HttpDocuments(ILogger<HttpDocuments> logger)
        {
            _logger = logger;
        }

        [Function("documents")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            var contentFolder = "content";
            var contentFolderPath = Path.Combine(Directory.GetCurrentDirectory(), contentFolder);
            var files = Directory.GetFiles(contentFolder, "*.md", SearchOption.AllDirectories);

            var filesResponse = new List<FileInfo>();

            foreach (var file in files)
            {
                filesResponse.Add(new FileInfo
                {
                    Id = Path.GetRelativePath(contentFolderPath, file).Replace(Path.DirectorySeparatorChar.ToString(), "__"),
                    LastModified = File.GetLastWriteTimeUtc(file)
                });
            }

            return new OkObjectResult(filesResponse);
        }
    }
}
