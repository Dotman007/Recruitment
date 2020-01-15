using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class CompanyImplementation : ICompany
    {
        public string GenerateRegNo()
        {
            string registrationNo = "";
            var random = new Random();
            registrationNo = "CMP" + random.Next(1000, 9000);
            return registrationNo;
        }

        public void UploadImage(HttpPostedFileBase httpPostedFileBase, Company company)
        {
            if (httpPostedFileBase != null)
            {
                string pic = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CompanyLogo"), pic);
                httpPostedFileBase.SaveAs(path);
                company.Logo = httpPostedFileBase.FileName;
            }
        }
    }
}