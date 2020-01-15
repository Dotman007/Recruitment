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
    public class InterviewsController : Controller
    {
        private RecruitmentContext db = new RecruitmentContext();
        private readonly System.Web.HttpContext _httpContext = System.Web.HttpContext.Current;

        // GET: Interviews
        public ActionResult Index()
        {
            var interviews = db.Interviews.Include(i => i.Applicant).Include(i => i.Company).Include(i => i.Job);
            return View(interviews.ToList());
        }

        // GET: Interviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // GET: Interviews/Create
        public ActionResult Create()
        {
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber");
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName");
            ViewBag.JobId = new SelectList(db.Jobs, "JobId", "JobRole");
            return View();
        }


        public ActionResult ViewInterviews()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            var scheduledInterview = db.Interviews.Where(i => i.ApplicantId == applicantId && i.Scheduled == true && DateTime.Now < i.Date);
            return View(scheduledInterview);
        }
       
        public ActionResult NoInterest()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            var scheduledInterview = db.Interviews.Where(i => i.ApplicantId == applicantId && i.Scheduled == true);
            foreach (var interview in scheduledInterview)
            {
                interview.NotInterested = true;
                //db.Interviews.Add(interview);
                db.SaveChanges();
                return View(scheduledInterview);
            }
            return View();
        }

        public ActionResult Accept()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            var scheduledInterview = db.Interviews.Where(i => i.ApplicantId == applicantId && i.Scheduled == true).ToList();
            foreach (var interview in scheduledInterview)
            {
                interview.Accept = true;
                db.SaveChanges();
                return RedirectToAction("AcceptedSchedule");
            }
            return View();
        }
        public ActionResult AcceptedSchedule()
        {
            return View();
        }

        public ActionResult AcceptedInterview()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            var checkSchedule = db.Jobs.Where(j => j.ApplicantId == applicantId && j.Scheduled == true);
            foreach (var scheduled in checkSchedule)
            {
                var acceptedInterview = db.Interviews.Where(i => i.ApplicantId == applicantId && i.Accept == true).ToList();
                if (acceptedInterview == null)
                {
                    return RedirectToAction("NoAcceptedInterview");
                }
                return View(acceptedInterview);
            }
            return View();
        }

        public ActionResult RejectedInterview()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            var rejectedInterview = db.Interviews.Where(i => i.ApplicantId == applicantId && i.NotInterested == true).ToList();
            if (rejectedInterview == null)
            {
                return RedirectToAction("NoRejectedInterview");
            }
            return View(rejectedInterview);
        }
        public ActionResult NoAcceptedInterview()
        {
            return View();
        }

        public ActionResult NoRejectedInterview()
        {
            return View();
        }
        // POST: Interviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterviewId,Date,Venue,ApplicantId,JobId,CompanyId,DateScheduled,Scheduled")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Interviews.Add(interview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", interview.ApplicantId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", interview.CompanyId);
            ViewBag.JobId = new SelectList(db.Jobs, "JobId", "JobRole", interview.JobId);
            return View(interview);
        }

        // GET: Interviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", interview.ApplicantId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", interview.CompanyId);
            ViewBag.JobId = new SelectList(db.Jobs, "JobId", "JobRole", interview.JobId);
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InterviewId,Date,Venue,ApplicantId,JobId,CompanyId,DateScheduled")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", interview.ApplicantId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", interview.CompanyId);
            ViewBag.JobId = new SelectList(db.Jobs, "JobId", "JobRole", interview.JobId);
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }


        public ActionResult InterviewRules()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var interviewrules = db.Interviews.Where(i => i.ApplicantId == id).SingleOrDefault();
            if (interviewrules != null)
            {
                interviewrules.AboutTo = true;
                db.SaveChanges();
                return RedirectToAction("Rules");
            }

            return View();

        }
        public ActionResult CameraInterview()
        {
            return View();
        }


        public ActionResult Rules()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            return View();
        }
        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interview interview = db.Interviews.Find(id);
            //db.Interviews.Remove(interview);
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
