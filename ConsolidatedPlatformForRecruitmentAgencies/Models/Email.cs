using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Email
    {
        public int EmailId { get; set; }

        public string RecieverEmail { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string SenderEmail { get; set; }

        public int ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }
    }
}