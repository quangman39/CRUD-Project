using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public HomeController(ICountriesService countriesServic, IPersonService personService)
        {
            _personService = personService;
            _countriesService = countriesServic;
        }


        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonReponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonReponse.PersonName),"Person Name"  },
                {nameof(PersonReponse.Email),"Email"  },
                {nameof(PersonReponse.DateOfBirth),"Date of Birth"  },
                {nameof(PersonReponse.Gender),"Gender"  },
                {nameof(PersonReponse.Country),"Country"  },
            };

            List<PersonReponse> list_person = await _personService.GetFilterPerson(searchBy, searchString);
            ViewBag.CurrentSeachBy = searchBy;
            ViewBag.CurrentSeachString = searchString;

            //Sort
            List<PersonReponse> list_person_get_Sorted = await _personService.GetSortedPerson(list_person, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
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
        public async Task<IActionResult> Create(PersonAddRequest personAdd)
        {
            if (!ModelState.IsValid)
            {
                List<CountryReponse> countries = await _countriesService.GetAll();
                ViewBag.Coutries = countries.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = x.CountryName,
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            //call service method
            PersonReponse personReponse = await _personService.AddPerson(personAdd);

            //navigate to Index() action method 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid idPerson)
        {
            //call method 
            PersonReponse? person_reponse = await _personService.GetPersonById(idPerson);
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
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {
            //Validate 
            if (!ModelState.IsValid)
            {
                List<CountryReponse> countries = await _countriesService.GetAll();
                ViewBag.Coutries = countries.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = x.CountryName,
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
            }

            //callmethod

            PersonReponse personReponse = await _personService.UpdatePerson(person);

            //navigate to Index
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid idPerson)
        {
            //call method 
            await _personService.DeletePerosn(idPerson);

            return RedirectToAction("Index");
        }

    }

}
