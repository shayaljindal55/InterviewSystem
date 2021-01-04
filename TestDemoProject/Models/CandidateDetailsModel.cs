using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{
    public class CandidateDetailsModel
    {
        public CandidateDetailsModel()
        {
            CandidateDetails = new CandidateDetails();
        }
        public CandidateDetails CandidateDetails { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}