using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CompanyId { get; set; }

        public string  CompanyName { get; set; }


        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public string Email { get; set; }

        public string Logo { get; set; }

        public string Website { get; set; }

        public string RegistrationNo { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}