using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{
    public class InterviewerDetailsModel
    {
        public InterviewerDetails InterviewerDetails { get; set; }
        public InterviewerDetailsModel()
        {
            InterviewerDetails = new InterviewerDetails();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}