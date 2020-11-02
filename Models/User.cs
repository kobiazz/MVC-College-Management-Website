using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class User
    {
        
        [Key]
      //  [RegularExpression("[0-9]+", ErrorMessage = "Can use numbers ")]
        public string Id { get; set; }

        //[Required]
        public string Name { get; set; }

        //[Required]
        public DateTime Birthday { get; set; }

        //[Required]
        public int Phone { get; set; }

        [Required]
        public string Password { get; set; }

        //[Required]
        public string UserType { get; set; }

        [Required]
        public string UserName { get; set; }
   
        public bool mode { get; set; }
    }
}