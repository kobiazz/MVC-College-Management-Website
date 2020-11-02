using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.ModelView
{
    public class CourseViewModel
    {

        public Course Course { get; set; }
        public List<Course> Courses { get; set; }
        public int CourseId { get; set; }
        public string StudentId { get; set; }
        public string gradeA { get; set; }
        public string gradeB { get; set; }
    }
}