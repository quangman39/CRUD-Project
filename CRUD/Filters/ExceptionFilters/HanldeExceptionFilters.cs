using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUD.Filters.ExceptionFilters
{
    public class HanldeExceptionFilters : IExceptionFilter
    {
        private readonly ILogger<HanldeExceptionFilters> logger;
        private readonly IHostEnvironment _hostEnvironment;

        public HanldeExceptionFilters(ILogger<HanldeExceptionFilters> logger, IHostEnvironment hostEvironment)
        {
            this.logger = logger;
            _hostEnvironment = hostEvironment;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError("Exception Filter Name {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",
                nameof(HanldeExceptionFilters), nameof(OnException), context.Exception.GetType().ToString(),
                context.Exception.Message);

        }
    }
}

