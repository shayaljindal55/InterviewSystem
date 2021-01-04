using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{
    public class InterviewDetail
    {
        [Key]
        public int ID { get; set; }
        public virtual ApplicationUser Interviewer { get; set; }
        public virtual ApplicationUser Candidate { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime InterviewDate { get; set; }
        public virtual InterviewRounds Round { get; set; }
        [Required]
        public virtual Departments InterviewForPost { get; set; }
    }

    public class GiveRating
    {
        [Required]
        public string CandidateId { get; set; }
        [Required]
        public int Rate { get; set; }

        public int Rounds { get; set; }
        public string ErrorMessage { get; set; }
        public string InterviewForPost { get; set; }

    }

    public class InterviewerDetails
    {
        [Key]
        public int ID { get; set; }
        public string Image { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public DateTime DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{10}$")]
        [StringLength(32)]
        public string Phone { get; set; }
        public string Designation { get; set; }
        //public virtual Departments InterviewForPost { get; set; }
        //[DataType(DataType.Password)]
        //[StringLength(255, MinimumLength = 8)]
        //public string Password { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class CandidateDetails
    {
        [Key]
        public int ID { get; set; }
        public string Image { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{10}$")]
        [StringLength(32)]
        public string Phone { get; set; }
        public string Degree { get; set; }
        public decimal DegreeAggregate { get; set; }
        public virtual Departments ApplyingForPost { get; set; }
        //[DataType(DataType.Password)]
        //[StringLength(255, MinimumLength = 8)]
        //public string Password { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Address { get; set; }
        public int PinCode { get; set; }
        public string YearOfPassing { get; set; }
        public int StandingBackLogs { get; set; }
        public bool GapInEducation { get; set; }
        public int NumberOfGapYears { get; set; }
        public int ExperienceInYears { get; set; }
        public string PreviousOrganization { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


}
