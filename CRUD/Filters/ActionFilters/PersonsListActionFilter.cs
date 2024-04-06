using CRUD.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUD.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // To do: add after  logic here

            _logger.LogInformation("{FilternName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            HomeController homeController = (HomeController)context.Controller;

            homeController.ViewData["searchBy"] = context.HttpContext.Items["argument"];
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (parameters != null)
            {
                if (parameters.ContainsKey("searchBy"))
                {
                    homeController.ViewData["CurrentSeachBy"] = parameters["searchBy"];
                }

                if (parameters.ContainsKey("searchString"))
                {
                    homeController.ViewData["CurrentSeachString"] = parameters["searchString"];
                }

                if (parameters.ContainsKey("sortBy"))
                {
                    homeController.ViewData["CurrentSortBy"] = parameters["sortBy"];
                }

                if (parameters.ContainsKey("sortOrder"))
                {
                    homeController.ViewData["CurrentSortOrder"] = parameters["sortOrder"];
                }

            }

            homeController.ViewBag.SearchFields = new Dictionary<string, string>()
                                  {
                                    { nameof(PersonReponse.PersonName), "Person Name" },
                                    { nameof(PersonReponse.Email), "Email" },
                                    { nameof(PersonReponse.DateOfBirth), "Date of Birth" },
                                    { nameof(PersonReponse.Address), "Address" },
                                    { nameof(PersonReponse.CountryId), "Country" }
                                  };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;


            // To do: add before  logic here
            _logger.LogInformation("{FilternName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuting));


            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                if (string.IsNullOrEmpty(searchBy))
                {
                    var SearchFields = new List<string>()
                    {
                        nameof(PersonReponse.PersonName),
                        nameof(PersonReponse.Email),
                        nameof(PersonReponse.DateOfBirth),
                        nameof(PersonReponse.Gender),
                        nameof(PersonReponse.CountryId),
                        nameof(PersonReponse.Address),
                    };

                    if (SearchFields.Any(temp => temp == searchBy) == false)
                    {

                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonReponse.PersonName);
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                    }



                }
            }
        }
    }
}
