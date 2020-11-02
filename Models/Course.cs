using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3   .Models
{
    public class Course
    {
        [Required]
        [Key]
        [RegularExpression("[0-9]+", ErrorMessage = "Can use numbers ")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Points { get; set; }


        [Required]
        public string LecId { get; set; }

        [Required]
        [Range(8, 19, ErrorMessage = "start of lecturer must be 8-19 ")]
        public string Hours { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        public string ClassRoom { get; set; }

        [Required]
        public DateTime MoadA { get; set; }


        [Required]
        public DateTime MoadB { get; set; }

        public bool mode { get; set; }


    }
}