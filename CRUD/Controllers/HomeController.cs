using CRUD.Filters.ActionFilters;
using CRUD.Filters.ExceptionFilters;
using CRUD.Filters.ResourceFilters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD.Controllers
{
    [TypeFilter<HanldeExceptionFilters>]
    public class HomeController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICountriesService countriesServic, IPersonService personService, ILogger<HomeController> logger)
        {
            _personService = personService;
            _countriesService = countriesServic;
            _logger = logger;
        }

        [TypeFilter(typeof(PersonsListActionFilter))]
        [TypeFilter(typeof(ResponseHeaderActionFilter),
            Arguments = new object[] { "I-CustomkEy", "I-CustomValue" })]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonReponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            _logger.LogDebug("searchBy " + searchBy + " searchString" + sortBy + " sortBy" + sortOrder + " sortOrder");

            //Search
            List<PersonReponse> list_person = await _personService.GetFilterPerson(searchBy, searchString);

            //Sort
            List<PersonReponse> list_person_get_Sorted = await _personService.GetSortedPerson(list_person, sortBy, sortOrder);

            return View(list_person_get_Sorted);
        }


        public async Task<IActionResult> Create()
        {
            List<CountryReponse> countries = await _countriesService.GetAll();

            ViewBag.Coutries = countries.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName,
            });

            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(FeatureDisableResourceFilters), Arguments = new object[] { false })]
        [TypeFilter<PersonCreateAndEditPostActionFilter>]

        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {

            //call service method
            PersonReponse personReponse = await _personService.AddPerson(personRequest);

            //navigate to Index() action method 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid personID)
        {
            //call method 
            PersonReponse? person_reponse = await _personService.GetPersonById(personID);
            if (person_reponse == null) return new NotFoundResult();

            //convert to personUpdate
            PersonUpdateRequest personUpdate = person_reponse.ToPersonUpdateRequest();
            List<CountryReponse> countries = await _countriesService.GetAll();
            ViewBag.Coutries = countries.Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName,
            });

            return View(personUpdate);
        }

        [HttpPost]
        [TypeFilter<PersonCreateAndEditPostActionFilter>]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            //callmethod

            PersonReponse personReponse = await _personService.UpdatePerson(personRequest);

            //navigate to Index
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid personID)
        {
            //call method 
            await _personService.DeletePerosn(personID);

            return RedirectToAction("Index");
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlePath = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlePath != null && exceptionHandlePath.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlePath.Error.Message;
            }
            return View();
        }

    }




}
