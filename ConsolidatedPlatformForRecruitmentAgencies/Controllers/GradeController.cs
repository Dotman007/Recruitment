using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class GradeController : Controller
    {

        private readonly IGrade _igrade = null;
        public GradeController(IGrade igrade)
        {
            _igrade = igrade;
        }


        public ActionResult GetAllGrades()
        {
            return View(_igrade.GetAllGrades());
        }

        [HttpGet]
        public ActionResult AddGrade()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGrade(Grade grade)
        {
                _igrade.AddGrade(grade);
                return RedirectToAction("GradeSuccess");
        }

        public ActionResult GradeSuccess()
        {
            return View();
        }


        [HttpGet]
        public ActionResult DeleteGrade(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = _igrade.FindGradeById(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        [HttpPost, ActionName("DeleteGrade")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            _igrade.DeleteConfirm(id);
            return RedirectToAction("GradeDeleted");
        }



        [HttpGet]
        public ActionResult EditGrade(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = _igrade.EditGradeById(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGrade(Grade grade)
        {
            _igrade.EditGrade(grade);
            return RedirectToAction("GradeEdited");
        }

        public ActionResult GradeDeleted()
        {
            return View();
        }


        public ActionResult GradeEdited()
        {
            return View();
        }
    }
}