using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Room
    {
        
        public int ID { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string SharedWith { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string OwnerName { get; set; }
        public string OwnerToken { get; set; }
        public string ParticipantName { get; set; }
        public int? ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string ParticipantToken { get; set; }
    }
}