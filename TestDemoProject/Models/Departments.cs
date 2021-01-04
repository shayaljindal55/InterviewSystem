using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewSystem.Models
{
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Deaprtment Name")]
        public string DepartmentName { get; set; }
    }
}