using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class JobsController : Controller
    {
        private RecruitmentContext db = new RecruitmentContext();

        // GET: Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.Course).Include(j => j.Grade).Include(j => j.JobCategory).Include(j => j.Qualification);
            return View(jobs.ToList());
        }

        public ActionResult JobListPartial()
        {
            var jobs = db.Jobs.ToList();
            return PartialView(jobs);
        }

        public ActionResult TopRatedJobListPartial()
        {
            var jobs = db.Jobs.ToList();
            return PartialView(jobs);
        }
        public ActionResult Search(string grade, string course, string qualification)
        {
            var searchResult = db.Jobs.Where(j => j.Grade.GradeName == grade || j.Course.CourseName == course || j.Qualification.QualificationName == qualification);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchJob()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName");
            return View();
        }

        [HttpPost]
        public ActionResult SearchJob(Job job)
        {
            var jobs = db.Jobs.Where(j => j.CourseId == job.CourseId || j.GradeId == job.GradeId || j.JobCategoryId == job.JobCategoryId || j.QualificationId == job.QualificationId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", job.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", job.GradeId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", job.QualificationId);
            return View(jobs);
        }
        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName");
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobId,JobCategoryId,JobRole,QualificationId,GradeId,CourseId,JobDescription,Deadline,Applied")] Job job)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Jobs.Add(job);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
            

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", job.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", job.GradeId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", job.QualificationId);
            return View(job); 
        }





        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", job.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", job.GradeId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", job.QualificationId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobId,JobCategoryId,JobRole,QualificationId,GradeId,CourseId,JobDescription,Applied")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", job.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", job.GradeId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", job.QualificationId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
