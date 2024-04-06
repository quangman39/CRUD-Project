using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUD.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IActionFilter
    {
        private ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string Key;
        private readonly string Value;

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
        {
            _logger = logger;
            Key = key;
            Value = value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilternName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilternName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
