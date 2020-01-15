using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class VideoInterviewController : Controller
    {
        public ActionResult GroupVideoInterview()
        {
            return View();
        }
    }
}

