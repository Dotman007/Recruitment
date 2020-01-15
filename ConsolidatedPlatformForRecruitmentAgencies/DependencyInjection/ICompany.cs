using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface ICompany
    {
        void UploadImage(HttpPostedFileBase httpPostedFileBase, Company company);
        string GenerateRegNo();
    }
}
