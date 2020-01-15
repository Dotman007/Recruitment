using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly RecruitmentContext _recruitmentContext; 
        private readonly System.Web.HttpContext _httpContext = System.Web.HttpContext.Current;
        private readonly ICompany _icompany;
        public CompaniesController(ICompany icompany, RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
            _icompany = icompany;
        }

        public ActionResult ViewJobApplication()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            int id = (int)(_httpContext.Session["CompanyId"]);
            var postedJobs = _recruitmentContext.Jobs.Where(j => j.CompanyId == id && j.Applied == true).ToList();
            if (postedJobs.Count() == 0)
            {
                return RedirectToAction("NoPostedJob");
            }
            return View(postedJobs);
        }

        public ActionResult NoPostedJob()
        {
            return View();
        }


        // POST: Interviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.




        public ActionResult ViewPostedJobs()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            int id = (int)(_httpContext.Session["CompanyId"]);
            var postedJobs = _recruitmentContext.Jobs.Where(j => j.CompanyId == id);
            return View(postedJobs);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "CompanyId,CompanyName,Address,Email,Logo,Website,RegistrationNo,Password")] Company company,HttpPostedFileBase logofile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    company.RegistrationNo = _icompany.GenerateRegNo();
                    company.Password = RandomPassword();
                    _icompany.UploadImage(logofile, company);
                    _recruitmentContext.Companies.Add(company);
                    _recruitmentContext.SaveChanges();
                    return RedirectToAction("SuccessRegCompany");
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
            
            return View(company);
        }

        public ActionResult Create()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            ViewBag.CourseId = new SelectList(_recruitmentContext.Courses, "CourseId", "CourseName");
            ViewBag.GradeId = new SelectList(_recruitmentContext.Grades, "GradeId", "GradeName");
            ViewBag.JobCategoryId = new SelectList(_recruitmentContext.JobCategories, "JobCategoryId", "JobCategoryName");
            ViewBag.QualificationId = new SelectList(_recruitmentContext.Qualifications, "QualificationId", "QualificationName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobId,JobCategoryId,JobRole,QualificationId,GradeId,CourseId,JobDescription,Deadline,Applied,CompanyId")] Job job)
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    int id = (int)(_httpContext.Session["CompanyId"]);
                    job.CompanyId = id;
                    _recruitmentContext.Jobs.Add(job);
                    _recruitmentContext.SaveChanges();
                    return RedirectToAction("Index");
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
            ViewBag.CourseId = new SelectList(_recruitmentContext.Courses, "CourseId", "CourseName", job.CourseId);
            ViewBag.GradeId = new SelectList(_recruitmentContext.Grades, "GradeId", "GradeName", job.GradeId);
            ViewBag.JobCategoryId = new SelectList(_recruitmentContext.JobCategories, "JobCategoryId", "JobCategoryName", job.JobCategoryId);
            ViewBag.QualificationId = new SelectList(_recruitmentContext.Qualifications, "QualificationId", "QualificationName", job.QualificationId);
            return View(job);
        }

        public ActionResult SuccessRegCompany()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string RegistrationNo, string Password)
        {
            if (RegistrationNo == "")
            {
                ViewBag.RegNoRequired = "Registration No is Required";
            }
            if (Password == "")
            {
                ViewBag.PasswordRequired = "Password is Required";
            }
            if (RegistrationNo == ""  && Password == "")
            {
                ViewBag.PasswordAndRegNo = "Password and Reg No is Required";
            }
            var company = _recruitmentContext.Companies.SingleOrDefault(c => c.RegistrationNo == RegistrationNo && c.Password == Password);
            if (company != null)
            {
                _httpContext.Session["CompanyId"] = company.CompanyId;
                _httpContext.Session["CompanyName"] = company.CompanyName;
                _httpContext.Session["RegistrationNo"] = company.RegistrationNo;
                _httpContext.Session["Email"] = company.Email;
                return RedirectToAction("CompanyDashboard","Companies");
            }
            _httpContext.Session["CompanyId"] = null;
            _httpContext.Session["CompanyName"] = null;
            _httpContext.Session["RegistrationNo"] = null;
            _httpContext.Session["Email"] = null;
            ViewBag.UserDoesNotExist = "Registration Number and Password is Invalid";
            return View("Login");
        }


        public ActionResult CompanyDashboard()
        {
            if (_httpContext.Session["CompanyId"] == null)
            {
                return RedirectToAction("Login", "Companies");
            }
            int companyId = (int)(_httpContext.Session["CompanyId"]);
            var company = _recruitmentContext.Companies.Find(companyId);
            return View(company);
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
            builder.Append(RandomNumber(10000, 90000));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // GET: Companies
        public ActionResult Index()
        {
            return View(_recruitmentContext.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _recruitmentContext.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Companies/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CompanyId,CompanyName,Address,CompanyLogo,Email,Logo,Website,RegistrationNo,Password")] Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _recruitmentContext.Companies.Add(company);
        //        _recruitmentContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(company);
        //}

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _recruitmentContext.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,CompanyName,Address,Email,Logo,Website,RegistrationNo,Password")] Company company)
        {
            if (ModelState.IsValid)
            {
                _recruitmentContext.Entry(company).State = EntityState.Modified;
                _recruitmentContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _recruitmentContext.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = _recruitmentContext.Companies.Find(id);
            _recruitmentContext.Companies.Remove(company);
            _recruitmentContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _recruitmentContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
