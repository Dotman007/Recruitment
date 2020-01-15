using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ConsolidatedPlatformForRecruitmentAgencies.DAL.RecruitmentContext _recruitmentContext;
        public HomeController(DAL.RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
        }
        public ActionResult Index()
        {
            var jobs = _recruitmentContext.Jobs.ToList();
            return View(jobs);
        }

        

       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}