using System.Web;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using Gnostice.StarDocsSDK;
namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class EducationImplementation : IEducation
    {
        private readonly RecruitmentContext _recruitmentContext;
        public EducationImplementation(RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
        }
        public void AddEducation(Education education)
        {
            
            //var check = education.Course.CourseName == checkInfo.Course.CourseName ? ViewBag.CourseExist = "The Course Already Exists" 
            //    : education.University.UniversityName == checkInfo.University.UniversityName ? ViewBag.UniversityExist = "The University Exists"
            //    : education.Qualification.QualificationName == checkInfo.Qualification.QualificationName ? ViewBag.QualificationExist = "The Qualification Exists"
            //    : education.Grade.GradeName == checkInfo.Grade.GradeName ? ViewBag.GradeExist = "The Grade Exists":
            //     education.GraduationYear.GraduationYearName == checkInfo.GraduationYear.GraduationYearName ? ViewBag.GraduationYear = "The Graduation Year Exists"
            //    : "Continue";

            _recruitmentContext.Educations.Add(education);
            _recruitmentContext.SaveChanges();
        }

        //public void DisplayResume(HttpPostedFileBase resumefileDisplay, Education displayeducationresume)
        //{
        //    DocX myDocument = DocX.Load(resumefileDisplay.FileName);
        //}

        public void UploadDegreeCertificate(HttpPostedFileBase filecertificate,Education educationcert)
        {
            if (filecertificate != null)
            {
                string pic = System.IO.Path.GetFileName(filecertificate.FileName);
                string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"), filecertificate.FileName );
                filecertificate.SaveAs(path);
                educationcert.DegreeCertificate = filecertificate.FileName;
            }
        }



        public void UploadNyscCertificate(HttpPostedFileBase filenysc,  Education edunysc)
        {
            if (filenysc != null)
            {
                string pic = System.IO.Path.GetFileName(filenysc.FileName);
                string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"), pic);
                filenysc.SaveAs(path);
                edunysc.NyscCertificate = filenysc.FileName;
            }
            
        }

        public void UploadResume(HttpPostedFileBase fileresume,Education educationresume)
        {
            if (fileresume != null)
            {
                string pic = System.IO.Path.GetFileName(fileresume.FileName);
                string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads"), pic);
                fileresume.SaveAs(path);
                educationresume.Resume = fileresume.FileName;
               
            }
           
        }
    }
}