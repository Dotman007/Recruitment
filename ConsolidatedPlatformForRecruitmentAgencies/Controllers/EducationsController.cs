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
using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using Gnostice.StarDocsSDK;
using NLog;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class EducationsController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private RecruitmentContext db = new RecruitmentContext();
        private readonly IEducation _ieducation;
        private readonly HttpContext _httpContext = System.Web.HttpContext.Current;
        public EducationsController(IEducation ieducation)
        {
            _ieducation = ieducation;
        }

        // GET: Educations
        public ActionResult Index()
        {
            var educations = db.Educations.Include(e => e.Course).Include(e => e.Grade).Include(e => e.GraduationYear).Include(e => e.Qualification).Include(e => e.University);
            return View(educations.ToList());
        }

        // GET: Educations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }
        public ActionResult ViewAddedEducation(int? id)
        {
            id = (int)(_httpContext.Session["ApplicantId"]);
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            var education = db.Educations.Where(e=>e.ApplicantId == id);
            return View(education);
        }


       
        // GET: Educations/Create
        public ActionResult AddEducation()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
            ViewBag.GraduationYearId = new SelectList(db.GraduationYears, "GraduationYearId", "GraduationYearName");
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName");
            ViewBag.UniversityId = new SelectList(db.Universities, "UniversityId", "UniversityName");
            return View();
        }

        // POST: Educations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEducation([Bind(Include = "EducationId,ApplicantId,QualificationId,UniversityId,GradeId,CourseId,AboutYou,GraduationYearId,Added,Resume,DegreeCertificate,NyscCertificate")]Education education,HttpPostedFileBase filedegree, HttpPostedFileBase filenysc, HttpPostedFileBase fileresume)
        {
            
            if ( education != null)
            {
                try
                {
                    int id = (int)(_httpContext.Session["ApplicantId"]);
                    if (_httpContext.Session["ApplicantId"] == null)
                    {
                        return RedirectToAction("Login", "Applicant");
                    }
                    Education edun = db.Educations.Find(id);
                    if (edun == null)
                    {
                        education.ApplicantId = id;
                        _ieducation.UploadDegreeCertificate(filedegree, education);
                        _ieducation.UploadNyscCertificate(filenysc, education);
                        _ieducation.UploadResume(fileresume, education);
                        education.Added = true;
                        db.Educations.Add(education);
                        db.SaveChanges();
                        //_ieducation.AddEducation(education);
                        return RedirectToAction("Dashboard", "Applicant");
                    }
                    //Applicant  app = db.Educations.SingleOrDefault(c => c.ApplicantId == id);
                    if (edun.Added == true)
                    {
                        ViewBag.CheckInfo = "Multiple Academic Information cannot be Saved..Kindly Edit if you want any modification";
                        ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
                        ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
                        ViewBag.GraduationYearId = new SelectList(db.GraduationYears, "GraduationYearId", "GraduationYearName");
                        ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName");
                        ViewBag.UniversityId = new SelectList(db.Universities, "UniversityId", "UniversityName");
                        return View();
                    }
                    
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join(",", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, fullErrorMessage);
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", education.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", education.GradeId);
            ViewBag.GraduationYearId = new SelectList(db.GraduationYears, "GraduationYearId", "GraduationYearName", education.GraduationYearId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", education.QualificationId);
            ViewBag.UniversityId = new SelectList(db.Universities, "UniversityId", "UniversityName", education.UniversityId);
            return View(education);
        }

        // GET: Educations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", education.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", education.GradeId);
            ViewBag.GraduationYearId = new SelectList(db.GraduationYears, "GraduationYearId", "GraduationYearName", education.GraduationYearId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", education.QualificationId);
            ViewBag.UniversityId = new SelectList(db.Universities, "UniversityId", "UniversityName", education.UniversityId);
            return View(education);
        }

        // POST: Educations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EducationId,ApplicantId,QualificationId,UniversityId,GradeId,CourseId,AboutYou,GraduationYearId")] Education education)
        {
            if (ModelState.IsValid)
            {
                db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", education.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", education.GradeId);
            ViewBag.GraduationYearId = new SelectList(db.GraduationYears, "GraduationYearId", "GraduationYearName", education.GraduationYearId);
            ViewBag.QualificationId = new SelectList(db.Qualifications, "QualificationId", "QualificationName", education.QualificationId);
            ViewBag.UniversityId = new SelectList(db.Universities, "UniversityId", "UniversityName", education.UniversityId);
            return View(education);
        }

        // GET: Educations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
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
