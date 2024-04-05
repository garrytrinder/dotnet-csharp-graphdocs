using Azure.Identity;
using Azure.Storage.Queues;
using GraphDocsConnector;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder
            .AddJsonFile(Path.Combine(context.HostingEnvironment.ContentRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.HostingEnvironment.ContentRootPath, $"appsettings.{context.HostingEnvironment.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, s) =>
    {
        var config = context.Configuration;

        s.AddApplicationInsightsTelemetryWorkerService();
        s.ConfigureFunctionsApplicationInsights();
        s.AddSingleton(s =>
        {
            var clientId = config["AzureAd:ClientId"];
            var clientSecret = config["AzureAd:ClientSecret"];
            var tenantId = config["AzureAd:TenantId"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var handlers = GraphClientFactory.CreateDefaultHandlers();
            var httpClient = GraphClientFactory.Create(handlers, proxy: Utils.GetWebProxy());

            return new GraphServiceClient(httpClient, credential);
        });
        s.AddAzureClients(configureClients =>
        {
            configureClients
                .AddQueueServiceClient(config.GetValue<string>("AzureWebJobsStorage"))
                .ConfigureOptions(options => options.MessageEncoding = QueueMessageEncoding.Base64);
            configureClients
                .AddBlobServiceClient(config.GetValue<string>("AzureWebJobsStorage"));
            configureClients
                .AddTableServiceClient(config.GetValue<string>("AzureWebJobsStorage"));
        });
    })
    .Build();

host.Run();
