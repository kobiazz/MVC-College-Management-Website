using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Dal;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SeeScheduleStudent()
        {

            ViewBag.message = Session["Student"].ToString();

            dal dal = new dal();
            string sesseionStudentId = Session["Student"].ToString();

            //List<Course> coursesUser = (from z in dal.Courses
            //                            where (z.Id == usr.UserName)
            //                            select z).ToList<User>();


            List<int> coureseIdList = (from u in dal.Grades
                                       where (u.StudentId == sesseionStudentId && u.mode==true)
                                       select u.CourseId).ToList<int>();

            //List<Grades> coureseIdList = (from u in dal.Grades
            //                              where (u.StudentId== sesseionStudentId)
            //                           select u).ToList<Grades>();



           // string a = "";
            List<Course> coursesUser = new List<Course>();

            for (int i = 0; i < coureseIdList.Count; i++)
            {
                //a += coureseIdList[i].ToString() + " ";
                int currentCourseId = coureseIdList[i];
                List<Course> temp = (from z in dal.Courses
                                            where (z.Id == currentCourseId && z.mode==true)
                                            select z).ToList<Course>();
                coursesUser.Add(temp[0]);
            }
            List<String> Schudele = new List<string>();

            for (int i = 0; i < coursesUser.Count; i++)
            {
                string currentCourseDeatils = "Course Name: " + coursesUser[i].Name.ToString() + " , Day: " + coursesUser[i].Day.ToString() + " ,Lecture Start Hour: " + coursesUser[i].Hours.ToString() + ":00 " ;
                Schudele.Add(currentCourseDeatils);
            }
            //for (int i = 0; i < coureseIdList.Count; i++)
            //    a += coureseIdList[i].CourseId.ToString() + " ";

          //  ViewBag.message = Session["Student"].ToString() + " " + a;
            ViewBag.Schudele = Schudele;

            return View("SeeScheduleStudent");
        }


        public ActionResult SeeExamSchedule()
        {


            dal dal = new dal();
            string sesseionStudentId = Session["Student"].ToString();

            //List<Course> coursesUser = (from z in dal.Courses
            //                            where (z.Id == usr.UserName)
            //                            select z).ToList<User>();


            List<int> coureseIdList = (from u in dal.Grades
                                       where (u.StudentId == sesseionStudentId && u.mode == true)
                                       select u.CourseId).ToList<int>();

            //List<Grades> coureseIdList = (from u in dal.Grades
            //                              where (u.StudentId== sesseionStudentId)
            //                           select u).ToList<Grades>();



            string a = "";
            List<Course> coursesUser = new List<Course>();

            for (int i = 0; i < coureseIdList.Count; i++)
            {
                a += coureseIdList[i].ToString() + " ";
                int currentCourseId = coureseIdList[i];
                List<Course> temp = (from z in dal.Courses
                                     where (z.Id == currentCourseId && z.mode == true)
                                     select z).ToList<Course>();
                coursesUser.Add(temp[0]);

            }
            List<String> Exam = new List<string>();

            List<String> m1 = new List<string>();
            List<String> m2 = new List<string>();

            for (int i = 0; i < coureseIdList.Count; i++)
            {
                int currentCourseId = coureseIdList[i];
                List<String> temp = (from z in dal.Grades
                                     where (z.CourseId == currentCourseId && z.mode == true)
                                     select z.GradeA).ToList<String>();
                List<String> temp2 = (from z in dal.Grades
                                     where (z.CourseId == currentCourseId && z.mode == true)
                                     select z.GradeB).ToList<String>();
                m1.Add(temp[0]);
                m2.Add(temp2[0]);
            }


            for (int i = 0; i < coursesUser.Count; i++)
            {
                string currentCourseDeatils = "Course Name: " + coursesUser[i].Name.ToString() + " , Moad A: " + coursesUser[i].MoadA.ToShortDateString() + " ,Grade A: " + m1[i] + "\r\n" + "Moad B: " + coursesUser[i].MoadB.ToShortDateString() + " ,Grade B: " + m2[i];

                Exam.Add(currentCourseDeatils);

            }
;



            ViewBag.Exam = Exam;




            return View("SeeExamSchedule");
        }



    }
}