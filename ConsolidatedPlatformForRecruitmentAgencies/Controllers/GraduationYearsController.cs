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
    public class GraduationYearsController : Controller
    {
        private RecruitmentContext db = new RecruitmentContext();

        // GET: GraduationYears
        public ActionResult Index()
        {
            return View(db.GraduationYears.ToList());
        }

        // GET: GraduationYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraduationYear graduationYear = db.GraduationYears.Find(id);
            if (graduationYear == null)
            {
                return HttpNotFound();
            }
            return View(graduationYear);
        }

        // GET: GraduationYears/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GraduationYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GraduationYearId,GraduationYearName")] GraduationYear graduationYear)
        {
            if (ModelState.IsValid)
            {
                db.GraduationYears.Add(graduationYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(graduationYear);
        }

        // GET: GraduationYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraduationYear graduationYear = db.GraduationYears.Find(id);
            if (graduationYear == null)
            {
                return HttpNotFound();
            }
            return View(graduationYear);
        }

        // POST: GraduationYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GraduationYearId,GraduationYearName")] GraduationYear graduationYear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(graduationYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(graduationYear);
        }

        // GET: GraduationYears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraduationYear graduationYear = db.GraduationYears.Find(id);
            if (graduationYear == null)
            {
                return HttpNotFound();
            }
            return View(graduationYear);
        }

        // POST: GraduationYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GraduationYear graduationYear = db.GraduationYears.Find(id);
            db.GraduationYears.Remove(graduationYear);
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
