using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int JobId { get; set; }


        public int JobCategoryId { get; set; }

        public virtual JobCategory JobCategory { get; set; }


        public int? ApplicantId { get; set; }

        public virtual Applicant Applicant { get; set; }

        public string  JobRole { get; set; }

        public int QualificationId { get; set; }

        public virtual Qualification Qualification { get; set; }

        public int GradeId { get; set; }

        public virtual Grade Grade { get; set; }


        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string JobDescription { get; set; }

        public bool Applied { get; set; }

       
        public DateTime DeadLine { get; set; }

        public DateTime? DateApplied { get; set; }

        public bool Scheduled { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}