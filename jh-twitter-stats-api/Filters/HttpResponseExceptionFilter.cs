using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace jh_twitter_stats_api.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;
        private ILogger<HttpResponseExceptionFilter> _logger;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context != null)
            {
                _logger = context.HttpContext.RequestServices.GetService<ILogger<HttpResponseExceptionFilter>>();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context != null)
            {
                if (context.Exception is TwitterStatsWebHttpResponseException exception)
                {
                    context.Result = new ObjectResult(exception.Value)
                    {
                        StatusCode = exception.Status,
                    };
                    if (_logger != null)
                    {
                        _logger.LogError($"{nameof(TwitterStatsWebHttpResponseException)}: {exception.Status}");
                    }
                }
                else if (context.Exception != null)
                {
                    context.Result = new ObjectResult(context.Exception)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Value = context.Exception.Message
                    };
                    if (_logger != null)
                    {
                        _logger.LogError($"{(int)HttpStatusCode.BadRequest}: {context.Exception.Message}");
                    }
                }
                context.ExceptionHandled = true;
            }
        }
    }

    /// <summary>
    /// Custom Exception Responses
    /// </summary>
    public class TwitterStatsWebHttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}
