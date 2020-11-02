using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Dal;
using WebApplication3.Models;
using WebApplication3.ModelView;

namespace WebApplication3.Controllers
{
    public class LecturerController : Controller
    {
        // GET: Lecturer
        public ActionResult Index()
        {
            return View();
        }
        //
        public ActionResult SeeGrades(Course crs)
        {
            dal dal = new dal();
            string sesseionLecturerId = Session["Lecturer"].ToString();

            List<Course> coureseNameList = (from u in dal.Courses
                                            where (u.LecId == sesseionLecturerId && u.mode == true)
                                            select u).ToList<Course>();

            ViewBag.CourseNameForGrades = coureseNameList;


            List<string> userValidatedxxx = (from u in dal.Grades
                                             where (u.CourseId == crs.Id)
                                             select u.StudentId).ToList<string>();



            List<string> studentIdListx = new List<string>();


            List<string> currentStudentName = new List<string>();

            List<string> currentGrade1 = new List<string>();
            List<string> currentGrade2 = new List<string>();
            List<string> grades3 = new List<string>();

            string g1 = "Grade A: ";
            string g2 = "Grade B:";
            //string g3 = " ";
            for (int i = 0; i < userValidatedxxx.Count; i++)
            {
                List<Course> coursesUser = new List<Course>();  //FOR GRADES
                string currentStudentId = userValidatedxxx[i];
                currentStudentName = (from z in dal.Users
                                      where (z.Id == currentStudentId && z.mode == true)
                                      select z.Name).ToList<string>();
                currentStudentName[0] += ": ";
                currentGrade1 = (from z in dal.Grades
                                 where (z.StudentId == currentStudentId && z.CourseId == crs.Id && z.mode == true)
                                 select z.GradeA).ToList<string>();

                currentGrade2 = (from z in dal.Grades
                                 where (z.StudentId == currentStudentId && z.CourseId == crs.Id && z.mode == true)
                                 select z.GradeB).ToList<string>();
                grades3.Add(currentStudentName[0]);
                if (currentGrade1[0] != "Grade A is Not Given Yet")
                    grades3.Add(g1);
                grades3.Add(currentGrade1[0]);
                if (currentGrade2[0] != "GradeB Not Given Yet")
                    grades3.Add(g2);
                grades3.Add(currentGrade2[0]);



                //FOR GRADES
                List<Course> temp = (from x in dal.Courses
                                     where (x.Id == crs.Id)
                                     select x).ToList<Course>();
                coursesUser.Add(temp[0]);

                List<String> Grades = new List<string>();

                //for (int t = 0; t < coursesUser.Count; t++)
                //{
                //    string currentCourseDeatils = "Course Name: " + coursesUser[t].Name.ToString() + " || Moad A Date: " + coursesUser[t].MoadA.ToString() + " Grade : " + coursesUser[t].GradeA.ToString() + " || Moad B Date: " + coursesUser[t].MoadB.ToString() + " ,Grade: " + coursesUser[t].GradeB.ToString() + " .";
                //    studentIdListx.Add(currentCourseDeatils);
                //}


            }

            ViewBag.userValidatedxxxForGrades = grades3;





            return View("SeeGrades");
        }


        public ActionResult SeeScheduleLecturer()
        {

            dal dal = new dal();
            string sesseionLecturerId = Session["Lecturer"].ToString();



            List<Course> coureseList = (from u in dal.Courses
                                        where (u.LecId == sesseionLecturerId && u.mode == true)
                                        select u).ToList<Course>();



            List<Course> coursesUser = new List<Course>();


            List<String> Schudele = new List<string>();

            for (int i = 0; i < coureseList.Count; i++)
            {
                string currentCourseDeatils = "Course Name: " + coureseList[i].Name.ToString() + " , Day: " + coureseList[i].Day.ToString() + " ,Hours: " + coureseList[i].Hours.ToString();
                Schudele.Add(currentCourseDeatils);
            }

            ViewBag.SchudeleLec = Schudele;




            return View("SeeScheduleLecturer");
        }




        public ActionResult SeeStudentsCourse(Course crs)
        {


            dal dal = new dal();
            string sesseionLecturerId = Session["Lecturer"].ToString();

            //List<Course> coursesUser = (from z in dal.Courses
            //                            where (z.Id == usr.UserName)
            //                            select z).ToList<User>();


            //List<int> coureseIdList = (from u in dal.Courses
            //                           where (u.LecId == sesseionLecturerId && u.mode == true)
            //                           select u.Id).ToList<int>();


            List<Course> coureseNameList = (from u in dal.Courses
                                            where (u.LecId == sesseionLecturerId && u.mode == true)
                                            select u).ToList<Course>();

            //for (int i = 0; i < coureseIdList.Count; i++)
            //    a += coureseIdList[i].CourseId.ToString() + " ";

            //  ViewBag.message = Session["Student"].ToString() + " " + a;
            ViewBag.CourseName = coureseNameList;
            //  ViewBag.coureseIdList = coureseIdList;
            // ViewBag.studentIdListx = studentIdListx;


            List<string> userValidatedxxx = (from u in dal.Grades
                                             where (u.CourseId == crs.Id)
                                             select u.StudentId).ToList<string>();


            List<string> studentIdListx = new List<string>();


            List<string> currentStudentName = new List<string>();


            for (int i = 0; i < userValidatedxxx.Count; i++)
            {

                string currentStudentId = userValidatedxxx[i];
                currentStudentName = (from z in dal.Users
                                      where (z.Id == currentStudentId && z.mode == true)
                                      select z.Name).ToList<string>();
                studentIdListx.Add(currentStudentName[0]);
            }

            ViewBag.userValidatedxxx = studentIdListx;
            return View("SeeStudentsCourse");


        }


        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public ActionResult UpdateGrades(CourseViewModel crs)
        {
            string sesseionLecturerId;// IF THIS ADMINISTRATOR
            List<Course> coureseNameList;
            dal dal = new dal();
            if (Session["Administrator"] != null)       // IF THIS ADMINISTRATOR
            {
                sesseionLecturerId = Session["Administrator"].ToString();
                coureseNameList = (from u in dal.Courses
                                                where ( u.mode == true)
                                                select u).ToList<Course>();

            }

            else  // IF LECTURER- LECTURERS COURSES
            { 
             sesseionLecturerId = Session["Lecturer"].ToString(); // IF THIS ADMINISTRATOR




             coureseNameList = (from u in dal.Courses
                                            where (u.LecId == sesseionLecturerId && u.mode == true)
                                            select u).ToList<Course>();

            }
            ViewBag.CourseNameForUpdateGrades = coureseNameList;
            ViewBag.NamesStudentList = new List<User>();


            List<string> userValidatedxxx = (from u in dal.Grades
                                             where (u.CourseId == crs.CourseId)
                                             select u.StudentId).ToList<string>();

            List<User> usersToShowInDDL = new List<User>();

            for (int i = 0; i < userValidatedxxx.Count; i++)
            {
                string currentStudentId = userValidatedxxx[i];
                List<User> currentUser = (from z in dal.Users
                                          where (z.Id == currentStudentId && z.mode == true)
                                          select z).ToList<User>();
                usersToShowInDDL.Add(currentUser[0]);
            }
            ViewBag.NamesStudentList = usersToShowInDDL;

            //grades of student
            if (crs.StudentId != null)
            {
                List<Grades> gradeStudent = (from z in dal.Grades
                                             where (z.CourseId == crs.CourseId && z.StudentId == crs.StudentId && z.mode == true)
                                             select z).ToList<Grades>();

                string deafultgradeA = "Grade A is Not Given Yet";
                string deafultgradeB = "GradeB Not Given Yet";

                bool flag3 = true;
               
              
                if (crs.gradeA != null || crs.gradeB != null)
                {
                   

                    if (crs.gradeA != null )
                    {
                        if(int.Parse(crs.gradeA) > 0 && (int.Parse(crs.gradeA) < 100))
                        { 
                        gradeStudent[0].GradeA = crs.gradeA;
                        //if there is no grade b - update the final grade
                        if (gradeStudent[0].GradeB.Equals(deafultgradeB))
                            gradeStudent[0].Grade = int.Parse(crs.gradeA);
                        }
                        else flag3 = false;
                    }
                    
                    if ((crs.gradeB != null) && flag3)
                    {
                       if( int.Parse(crs.gradeB) > 0 && (int.Parse(crs.gradeB) < 100))
                        { 
                        gradeStudent[0].GradeB = crs.gradeB;
                        gradeStudent[0].Grade = int.Parse(crs.gradeB);
                        }
                        else flag3 = false;

                    }
                  

                    if (flag3)
                    {
                        dal.SaveChanges();
                    }
                   

                    //change the dal
                    

                    //clear the textbox
                    ModelState.Remove("gradeA");
                    ModelState.Remove("gradeB");

                }

                ViewBag.gradeA = gradeStudent[0].GradeA;
                ViewBag.gradeB = gradeStudent[0].GradeB;


                ViewBag.gradeExist = null;

                //if there is grade A - you can change grade b
                if (!(gradeStudent[0].GradeA.Equals(deafultgradeA)))
                {
                    ViewBag.gradeExist = "A";
                }
                if (!flag3) ViewBag.Error = "grade must be 0 - 100";
            }

            return View("UpdateGrades");
        }
    }
}