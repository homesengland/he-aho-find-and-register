using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Find_Register.Filters
{
    public class UnhandledExceptionFilter:  IExceptionFilter
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;

        }

        public void OnException(ExceptionContext context)
        {
            string id = Guid.NewGuid().ToString();
            var exceptionMessage = context.Exception.Message;
            var internalMessage = context.Exception.InnerException?.Message ?? string.Empty;
            
            var result = new ViewResult { ViewName = "InternalServerError" };
            
            var message = GenerateMessage(id, exceptionMessage, internalMessage);
            
            _logger.LogError(message);

            var stackTraceMessage = GenerateMessage(id, context.Exception.StackTrace ?? string.Empty);
            
            _logger.LogCritical(stackTraceMessage);
            
            context.ExceptionHandled = true;
            context.Result = result;
        }

        private string GenerateMessage(params string[] message)
        {
            string result = string.Empty;
            
            foreach (var msg in message)
            {
                result = string.Concat(result, "-", msg);
            }

            return result;
        }
    }
}