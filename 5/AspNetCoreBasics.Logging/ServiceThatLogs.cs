using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBasics.Logging.Services
{
    class ServiceThatLogs
    {
        private readonly ILogger<Logging> _logger;

        public ServiceThatLogs(ILogger<Logging> logger)
        {
            _logger = logger;

            _logger.LogInformation("Service that logs was created!");
        }


        public void LogSomethingInDebug()
        {
            _logger.LogDebug("This is random and useless info");
        }

        public void LogSomethingInInfo()
        {
            _logger.LogInformation("This is some vital information");

        }

        public void LogSomethingInWarning()
        {
            _logger.LogWarning("This is a warning due to residual wastes spreaded arround");
        }

        public void LogSomethingInError()
        {
            _logger.LogError("This is not supposed to happen, and you should be aware of it");

        }

        public void LogSomethingInCritical()
        {
            _logger.LogCritical("This is Critical, solve me");
        }

        public void LogAnException()
        {
            var ex = new Exception("Today I feel like going to party", new Exception("Lets take a nap", new Exception("I rest very awefully today")));

            _logger.LogError(ex, "Who is going to party? {name} At what time? {time}", "Me", "tonight");
        }
    }
}
