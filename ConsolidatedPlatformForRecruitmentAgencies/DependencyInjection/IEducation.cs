using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface IEducation
    {
        void AddEducation(Education education);
        void UploadResume(HttpPostedFileBase fileresume,  Education educationresume);
        //void DisplayResume(HttpPostedFileBase resumefileDisplay, Education displayeducationresume);
        void UploadNyscCertificate(HttpPostedFileBase filenysc, Education edunysc);
        void UploadDegreeCertificate(HttpPostedFileBase filecertificate, Education educert);
    }
}
