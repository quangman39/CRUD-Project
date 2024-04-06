using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUD.Filters.ResultFilters
{
    public class homeListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<homeListResultFilter> logger;

        public homeListResultFilter(ILogger<homeListResultFilter> logger)
        {
            this.logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            logger.LogInformation($"{nameof(homeListResultFilter)} before");

            await next();

            //before logic
            logger.LogInformation($"{nameof(homeListResultFilter)} after");


        }
    }
}
