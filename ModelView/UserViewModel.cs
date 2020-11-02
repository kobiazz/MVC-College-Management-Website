using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.ModelView
{
    public class UserViewModel
    {

        public User User { get; set; }
        public List<User> Users { get; set; }

    }
}