using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUD.Filters.ResourceFilters
{
    public class FeatureDisableResourceFilters : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisableResourceFilters> _logger;
        private readonly bool _disable;
        public FeatureDisableResourceFilters(ILogger<FeatureDisableResourceFilters> logger, bool disable = true)
        {
            _logger = logger;
            _disable = disable;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            //before
            _logger.LogInformation("{FitlerName}.{MethodName}-before", nameof(FeatureDisableResourceFilters), nameof(OnResourceExecutionAsync));

            if (_disable)
            {
                context.Result = new NotFoundResult();
            }

            await next();

            //after 
            _logger.LogInformation("{FitlerName}.{MethodName}-after", nameof(FeatureDisableResourceFilters), nameof(OnResourceExecutionAsync));



        }
    }
}
