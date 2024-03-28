using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphDocsConnector.Functions
{
    public class TimerRemoveDeleted
    {
        private readonly ILogger _logger;

        public TimerRemoveDeleted(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimerRemoveDeleted>();
        }

        [Function("TimerRemoveDeleted")]
        public void Run([TimerTrigger("10 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
