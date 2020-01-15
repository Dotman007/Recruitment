using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class SDPMessage
    {
        
        public int ID { get; set; }

        public string SDP { get; set; }

        public bool isProcessed { get; set; }

        public string RoomToken { get; set; }

        public string Sender { get; set; }

        public int? ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}