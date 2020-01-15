using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.TestModels
{
    public class Exam
    {
        public int ExamId { get; set; }
        public int ApplicantId { get; set; }
        public virtual Applicant Applicant { get; set; }
        public int ModuleId { get; set; }
        public int Attempts { get; set; }
        public int Marks { get; set; }
    }
}