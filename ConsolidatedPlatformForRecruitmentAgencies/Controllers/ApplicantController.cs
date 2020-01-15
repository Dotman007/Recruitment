using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class ApplicantController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RecruitmentContext _recruitmentContext = new RecruitmentContext();
        private readonly IApplicant _iapplicant;
        private readonly System.Web.HttpContext _httpContext = System.Web.HttpContext.Current;
        public ApplicantController(IApplicant iapplicant)
        {
            _iapplicant = iapplicant;
        }


        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.GenderId =  new SelectList(_iapplicant.ListOfGender(), "GenderId", "GenderName");
            ViewBag.CountryId = new SelectList( _iapplicant.ListOfCountry(),"CountryId","CountryName");
            //ViewBag.StateId = new SelectList(_iapplicant.ListOfState(), "StateId", "StateName");
            return View();
        }




        //View Application History
        public ActionResult ViewApplications()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var getJobsApplied = _recruitmentContext.Jobs.Where(j => j.ApplicantId == id && j.Applied == true).ToList();
            return View(getJobsApplied);
        }

        [HttpPost]
        public ActionResult Register(Applicant applicant)
        {
            try
            {
                if (applicant != null)
                {
                    try
                    {
                        int applicantId = applicant.ApplicantId;
                        applicant.ApplicantUniqueNumber = Guid.NewGuid().ToString();
                        applicant.UserName = _iapplicant.GenerateRegNo();
                        applicant.Password = RandomString(10, true);
                        _iapplicant.Register(applicant);
                        _iapplicant.SendEmail(applicant.ApplicantId);
                        return RedirectToAction("Registered");
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex);
                       
                    }
                    
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
                //ViewBag.RegisterError = "An Error Occurred :" + ex.Message + ex.TargetSite + ex.HResult;
            }
            ViewBag.GenderId = new SelectList(_iapplicant.ListOfGender(), "GenderId", "GenderName",applicant.GenderId);
            ViewBag.CountryId = new SelectList(_iapplicant.ListOfCountry(), "CountryId", "CountryName", applicant.CountryId);
            //ViewBag.StateId = new SelectList(_iapplicant.ListOfState(), "StateId", "StateName", applicant.StateId);
            return View();
        }

        public ActionResult Dashboard()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login", "Applicant");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var applicant = _recruitmentContext.Applicants.Find(id);
            return View(applicant);
        }

        public ActionResult ViewJobs()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return View("Login");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var getQualificationfromEdu = _recruitmentContext.Educations.Where(e => e.ApplicantId == id );
            foreach (var qualification in getQualificationfromEdu)
            {
                var jobs = _recruitmentContext.Jobs.Where(j => j.Qualification.QualificationName == qualification.Qualification.QualificationName
                && j.Course.CourseName == qualification.Course.CourseName
                && j.Grade.GradeName == qualification.Grade.GradeName
                && DateTime.Now < j.DeadLine
                && j.Applied == false).ToList();
                return View(jobs);
            }
            return View();
        }
        public ActionResult NoJob()
        {
            return View();
        }
        public ActionResult CurrentInterviews()
        {
            int id = (int)(_httpContext.Session["ApplicantId"]);
            Applicant applicant = _recruitmentContext.Applicants.Find(id);
            var interviews = _recruitmentContext.Interviews.Where(i => i.ApplicantId == applicant.ApplicantId);
            foreach (var interview in interviews)
            {
                var allListofInterviews = _recruitmentContext.Interviews.Where(i => i.Scheduled == true && DateTime.Now  >= interview.Date );
                if (allListofInterviews.Count() == 0)
                {
                    return RedirectToAction("NotYetScheduled");
                }
                return View(allListofInterviews);
            }

            return View();
        }

        public ActionResult NotYetScheduled()
        {
            return View();
        }

        public ActionResult NotScheduled()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return View("Login");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var notScheduled = _recruitmentContext.Interviews.Where(i => i.ApplicantId == id && i.Scheduled == false || i.Scheduled != true).ToList();
            if (notScheduled.Count() == 0)
            {
                return RedirectToAction("EmptyNotSchedule");
            }
            return View(notScheduled);
        }

        [HttpGet]
        public ActionResult SearchJob()
        {
            ViewBag.CourseId = new SelectList(_recruitmentContext.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(_recruitmentContext.Grades, "GradeId", "GradeName");
            ViewBag.JobCategoryId = new SelectList(_recruitmentContext.JobCategories, "JobCategoryId", "JobCategoryName");
            ViewBag.QualificationId = new SelectList(_recruitmentContext.Qualifications, "QualificationId", "QualificationName");
            return View();
        }
        [HttpGet]
        public ActionResult JobList()
        {
            ViewBag.CourseId = new SelectList(_recruitmentContext.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(_recruitmentContext.Grades, "GradeId", "GradeName");
            ViewBag.JobCategoryId = new SelectList(_recruitmentContext.JobCategories, "JobCategoryId", "JobCategoryName");
            ViewBag.QualificationId = new SelectList(_recruitmentContext.Qualifications, "QualificationId", "QualificationName");
            ViewBag.JobId = new SelectList(_recruitmentContext.Jobs, "JobId", "JobRole");
            return View();
        }



        [HttpPost]
        public ActionResult JobList(string jobName,int? courseName, int? gradeName, int? jobCategory, int? qualificationName)
        {
            var jobSearch = _recruitmentContext.Jobs.Where(s => s.JobRole== jobName || s.GradeId == gradeName || s.JobCategoryId == jobCategory || s.QualificationId == qualificationName).ToList();
            if (jobSearch == null)
            {
                return null;
            }
            ViewBag.CourseId = new SelectList(_recruitmentContext.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(_recruitmentContext.Grades, "GradeId", "GradeName");
            ViewBag.JobCategoryId = new SelectList(_recruitmentContext.JobCategories, "JobCategoryId", "JobCategoryName");
            ViewBag.QualificationId = new SelectList(_recruitmentContext.Qualifications, "QualificationId", "QualificationName");
            ViewBag.JobId = new SelectList(_recruitmentContext.Jobs, "JobId", "JobRole");
            return View(jobSearch);
        }
        [HttpGet]
        public ActionResult ScheduledInterview()
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return View("Login");
            }
            int id = (int)(_httpContext.Session["ApplicantId"]);
            var scheduled = _recruitmentContext.Interviews.Where(i => i.ApplicantId == id && i.Scheduled == true).ToList();
            if (scheduled.Count() == 0)
            {
                return RedirectToAction("EmptySchedule");
            }
            return View(scheduled);
        }
        public ActionResult EmptySchedule()
        {
            return View();
        }
        public ActionResult EmptyNotSchedule()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleInterview([Bind(Include = "InterviewId,Date,Venue,ApplicantId,JobId,CompanyId,DateScheduled,Scheduled")] Interview interview, int? id)
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            Applicant applicant = _recruitmentContext.Applicants.Find(id);
            var jobs = _recruitmentContext.Jobs.Where(j => j.ApplicantId == applicant.ApplicantId).ToList();
            foreach (var job in jobs)
            {
                if (ModelState.IsValid)
                {
                    int companyId = (int)(_httpContext.Session["CompanyId"]);
                    job.Scheduled = true;
                    interview.CompanyId = companyId;
                    interview.JobId = job.JobId;
                    interview.ApplicantId = applicant.ApplicantId;
                    interview.DateScheduled = DateTime.Now;
                    _recruitmentContext.Interviews.Add(interview);
                    _recruitmentContext.SaveChanges();
                    return RedirectToAction("ViewScheduledInterview");
                }
            }
            
            return View(interview);
        }


        public ActionResult ViewScheduledInterview()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            int id = (int)(_httpContext.Session["CompanyId"]);
            var scheduledInterview = _recruitmentContext.Interviews.Where(i => i.Scheduled == true && i.ApplicantId == id).ToList();
            if (scheduledInterview.Count() == 0)
            {
                return RedirectToAction("NoScheduledInterview");
            }
            return View(scheduledInterview);
        }

        public ActionResult NoScheduledInterview()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            return View();
        }


        public ActionResult Apply(int id)
        {
            if (_httpContext.Session["ApplicantId"] == null)
            {
                return RedirectToAction("Login");
            }
            int applicantId = (int)(_httpContext.Session["ApplicantId"]);
            Job job = _recruitmentContext.Jobs.Find(id);
            job.ApplicantId = applicantId;
            job.DateApplied = DateTime.Now;
            job.Applied = true;
            _recruitmentContext.SaveChanges();
            return RedirectToAction("JobSuccess");
        }

        public ActionResult JobSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            if (UserName == " ")
            {
                ViewBag.Message = "Registration No is Required";
            }
            if (Password == " ")
            {
                ViewBag.Message = "Password is Required";
            }
            if (UserName ==" " && Password==" ")
            {
                ViewBag.Message = "Registration Number and Password is Required";
            }
            var applicantInfo = _recruitmentContext.Applicants.SingleOrDefault(a => a.UserName == UserName && a.Password == Password);
            if (applicantInfo != null)
            {
                _httpContext.Session["ApplicantId"] = applicantInfo.ApplicantId;
                _httpContext.Session["ApplicantUniqueNumber"] = applicantInfo.ApplicantUniqueNumber;
                _httpContext.Session["FirstName"] = applicantInfo.FirstName;
                _httpContext.Session["UserName"] = applicantInfo.UserName;
                _httpContext.Session["Password"] = applicantInfo.Password;
                return RedirectToAction("Dashboard", "Applicant");
            }
            _httpContext.Session["ApplicantId"] = null;
            _httpContext.Session["ApplicantUniqueNumber"] = null;
            _httpContext.Session["UserName"] = null;
            _httpContext.Session["Password"] = null;

            ViewBag.Message = "Invalid Registration No. or Password";
            return View("Login");
        }

        public JsonResult GetStateByCountryId(int id)
        {
            var statesbycountryId = _iapplicant.GetStateByCountry(id);
            return Json(statesbycountryId,JsonRequestBehavior.AllowGet);
        }

        public ViewResult Registered()
        {
            return View();
        }

    }
}