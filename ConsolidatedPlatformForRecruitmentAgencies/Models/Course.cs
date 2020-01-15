using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Course
   {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9\-]$")]
        public string CourseName { get; set; }
    }
}