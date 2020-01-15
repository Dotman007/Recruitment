using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Education
    {
        public int EducationId { get; set; }

        public int? ApplicantId { get; set; }

        public int QualificationId { get; set; }
        public virtual Qualification Qualification { get; set; }

        public int UniversityId { get; set; }
        public virtual University University { get; set; }

        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }


        [Required]
        [DataType(DataType.Text)]
        public string AboutYou { get; set; }


        public string Resume{ get; set; }
        public string  NyscCertificate { get; set; }
        public string DegreeCertificate { get; set; }
        public int GraduationYearId { get; set; }
        public virtual GraduationYear GraduationYear { get; set; }


        public bool Added { get; set; }

    }
}