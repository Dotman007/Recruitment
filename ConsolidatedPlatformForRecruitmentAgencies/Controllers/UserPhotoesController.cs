using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.Controllers
{
    public class UserPhotoesController : Controller
    {
        private readonly System.Web.HttpContext _httpContext = System.Web.HttpContext.Current;
        private RecruitmentContext db = new RecruitmentContext();
        private readonly IUserPhoto _userPhoto;
        public UserPhotoesController(IUserPhoto userPhoto)
        {
            _userPhoto = userPhoto;
        }
        // GET: UserPhotoes
        public ActionResult Index()
        {
            var userPhoto = db.UserPhoto.Include(u => u.Applicant);
            return View(userPhoto.ToList());
        }

        // GET: UserPhotoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPhoto userPhoto = db.UserPhoto.Find(id);
            if (userPhoto == null)
            {
                return HttpNotFound();
            }
            return View(userPhoto);
        }

        // GET: UserPhotoes/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: UserPhotoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserPhotoId,ApplicantId,UserPhotoImage")] HttpPostedFileBase fileBase, UserPhoto userPhoto)
        {
            if (ModelState.IsValid)
            {
                if (_httpContext.Session["ApplicantId"] == null)
                {
                    return View();
                }
                int id = (int)(_httpContext.Session["ApplicantId"]);
                userPhoto.ApplicantId = id;
                _userPhoto.UploadPhoto(fileBase, userPhoto);
                db.UserPhoto.Add(userPhoto);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", userPhoto.ApplicantId);
            return View(userPhoto);
        }

        // GET: UserPhotoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPhoto userPhoto = db.UserPhoto.Find(id);
            if (userPhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", userPhoto.ApplicantId);
            return View(userPhoto);
        }

        // POST: UserPhotoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserPhotoId,ApplicantId,UserPhotoImage")] UserPhoto userPhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPhoto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "ApplicantUniqueNumber", userPhoto.ApplicantId);
            return View(userPhoto);
        }

        // GET: UserPhotoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPhoto userPhoto = db.UserPhoto.Find(id);
            if (userPhoto == null)
            {
                return HttpNotFound();
            }
            return View(userPhoto);
        }

        // POST: UserPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPhoto userPhoto = db.UserPhoto.Find(id);
            db.UserPhoto.Remove(userPhoto);
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
