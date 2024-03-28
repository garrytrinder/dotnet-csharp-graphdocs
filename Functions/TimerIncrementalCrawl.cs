using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace dotnet_csharp_graphdocs.Functions
{
    public class TimerIncrementalCrawl
    {
        private readonly ILogger _logger;

        public TimerIncrementalCrawl(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimerIncrementalCrawl>();
        }

        [Function("TimerIncrementalCrawl")]
        public void Run([TimerTrigger("0 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
