using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication3.Models;

namespace WebApplication3.Dal
{
    public class dal:DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("tblCourse");
            modelBuilder.Entity<User>().ToTable("tblUser");
            modelBuilder.Entity<Grades>().ToTable("tblGrades");


        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<User> Users { get; set; }
      
    }




}
