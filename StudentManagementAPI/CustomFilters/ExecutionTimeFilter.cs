using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace StudentManagementAPI.CustomFilters
{
    public class ExecutionTimeFilter : ActionFilterAttribute
    {

        private Stopwatch _stopwatch;

        private ILogger<ExecutionTimeFilter> _logger;

        public ExecutionTimeFilter(ILogger<ExecutionTimeFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Started executing action method : {method}", context.ActionDescriptor.DisplayName);
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            _logger.LogInformation("Time taken for executing action method : {method} is {time} ms",context.ActionDescriptor.DisplayName,_stopwatch.ElapsedMilliseconds);
        }

    }
}
