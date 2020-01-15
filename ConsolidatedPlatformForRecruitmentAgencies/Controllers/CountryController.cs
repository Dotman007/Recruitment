using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountry _icountry;
        public CountryController(ICountry icountry)
        {
            _icountry = icountry;
        }

        public ActionResult GetAllCountries()
        {
            return View(_icountry.GetAllCountries());
        }

        

        [HttpGet]
        public ActionResult AddCountry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCountry(Country country)
        {
            try
            {
                if (country != null)
                {
                    _icountry.AddCountry(country);
                    return RedirectToAction("CountryAdded");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Unexpected = "This Error Occured :" + ex.Message;
            }
            
            return View();
        }

        public ActionResult CountryAdded()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditCountry(int id)
        {
            var country = _icountry.EditCountryById(id);
            return View(country);

        }

        [HttpPost]
        public ActionResult EditCountry(Country country)
        {
            _icountry.EditCountry(country);
            return RedirectToAction("CountrySuccess");
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var country = _icountry.FindCountryById(id);
            return View(country);

        }
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult DeleteCountry(int id)
        {
            _icountry.DeleteConfirm(id);
            return RedirectToAction("CountryDeleted");
        }

        public ActionResult CountryDeleted()
        {
            return View();
        }

    }
}