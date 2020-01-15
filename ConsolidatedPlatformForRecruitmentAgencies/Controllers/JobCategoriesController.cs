using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class JobCategoriesController : Controller
    {
        private RecruitmentContext db = new RecruitmentContext();

        // GET: JobCategories
        public ActionResult Index()
        {
            return View(db.JobCategories.ToList());
        }

        // GET: JobCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategory jobCategory = db.JobCategories.Find(id);
            if (jobCategory == null)
            {
                return HttpNotFound();
            }
            return View(jobCategory);
        }

        // GET: JobCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobCategoryId,JobCategoryName")] JobCategory jobCategory)
        {
            if (ModelState.IsValid)
            {
                db.JobCategories.Add(jobCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobCategory);
        }

        public ActionResult JobCategoryListPartial()
        {
            var jobs = db.JobCategories.ToList();
            return PartialView(jobs);
        }
        // GET: JobCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategory jobCategory = db.JobCategories.Find(id);
            if (jobCategory == null)
            {
                return HttpNotFound();
            }
            return View(jobCategory);
        }

        // POST: JobCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobCategoryId,JobCategoryName")] JobCategory jobCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobCategory);
        }

        // GET: JobCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategory jobCategory = db.JobCategories.Find(id);
            if (jobCategory == null)
            {
                return HttpNotFound();
            }
            return View(jobCategory);
        }

        // POST: JobCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCategory jobCategory = db.JobCategories.Find(id);
            db.JobCategories.Remove(jobCategory);
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
