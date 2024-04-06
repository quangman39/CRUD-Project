using CRUD.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUD.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {

        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ILogger<PersonCreateAndEditPostActionFilter> logger, ICountriesService countriesService)
        {
            _logger = logger;
            _countriesService = countriesService;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //logic before

            if (context.Controller is HomeController homeController)
            {

                if (!homeController.ModelState.IsValid)
                {
                    List<CountryReponse> countries = await _countriesService.GetAll();
                    homeController.ViewBag.Coutries = countries.Select(x => new SelectListItem()
                    {
                        Value = x.CountryId.ToString(),
                        Text = x.CountryName,
                    });
                    homeController.ViewBag.Errors = homeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personRequest"];


                    context.Result = homeController.View(personRequest);// short-circuits or skip subsequen action filter & action method;
                }
                else
                {
                    await next(); // invokes subsequens filters
                }


            }
            else
            {
                await next();

            }


            //logic after 
            _logger.LogInformation("In after logic of PersonsCreateAndEdit Action filter");
        }
    }
}
