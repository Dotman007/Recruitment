using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Interview
    {
        public int InterviewId { get; set; }

        public DateTime Date { get; set; }

        public string Venue { get; set; }

        public int? ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }
        public int? JobId { get; set; }
        public virtual Job Job { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public DateTime DateScheduled { get; set; }

        public bool? Scheduled { get; set; }


        public bool? Accept { get; set; }

        public bool? NotInterested { get; set; }

        public bool? AboutTo { get; set; }

    }
}