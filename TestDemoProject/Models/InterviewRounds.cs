using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{
    public class InterviewRounds
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int Stage { get; set; }
        //public string Position { get; set; }
        public virtual Departments Department { get; set; }


    }
}