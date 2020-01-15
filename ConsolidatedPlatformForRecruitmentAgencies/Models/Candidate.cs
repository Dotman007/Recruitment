using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Candidate
    {
        public int ID { get; set; }

        public int? ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string Label { get; set; }

        public string RoomToken { get; set; }

        public string Candid { get; set; }

        public string Sender { get; set; }

        public bool IsProcessed { get; set; }

    }
}