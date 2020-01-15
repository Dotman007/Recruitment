using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class Applicant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ApplicantId { get; set; }


        public string ApplicantUniqueNumber { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9\-]+$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9\-]+$")]
        public string LastName { get; set; }

        
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage ="Invalid Phone Number")]
        public string TelephoneNo { get; set; }


        public int CountryId { get; set; }
        public virtual Country Country { get; set; }



        public int StateId { get; set; }
        public virtual State State { get; set; }

        public int GenderId { get; set; }

        public virtual Gender Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }


        public bool EmailSent { get; set; }
    }
}