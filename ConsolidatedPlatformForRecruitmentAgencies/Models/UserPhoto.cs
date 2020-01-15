using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class UserPhoto
    {
        
        public int UserPhotoId { get; set; }


        public int ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }

        public string UserPhotoImage { get; set; }
    }
}