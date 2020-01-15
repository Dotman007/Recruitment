using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.TestModels
{
    public class QuestionModel
    {
        public int ModuleID { get; set; }
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string AlternateAnsOne { get; set; }
        public string AlternateAnsTwo { get; set; }
        public string AlternateAnsThree { get; set; }
        public string Answer { get; set; }

    }
}