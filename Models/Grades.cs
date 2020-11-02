using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Grades
    {
        [Required]
        [Key]
        [RegularExpression("[0-9]+", ErrorMessage = "Can use numbers ")]
        public string StudentId { get; set; }

        
        public int Grade { get; set; }

   

        [Required]
        
        public int CourseId { get; set; }

        public bool mode { get; set; }
        
        public string GradeA { get; set; }

        
        public string GradeB { get; set; }
    }
}