using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9\-]+$")]
        public string StateName { get; set; }


        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }


    }
}