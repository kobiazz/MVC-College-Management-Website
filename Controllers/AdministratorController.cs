using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Dal;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPerson(User usr)
        {
            //if (Session["Administrator"] != null)
            //    return RedirectToAction("Login");


            dal dal = new dal();

            if (usr.Id != null)
            {
                List<User> userslist = (from u in dal.Users
                                        where (u.mode == true)
                                        select u).ToList<User>();
                User find = userslist.FirstOrDefault(x => x.Id == usr.Id);
                if (find == null)
                {
                    usr.mode =true;
                   
                    dal.Users.Add(usr);
                    dal.SaveChanges();
                    ViewBag.message13 = "User Added Successfully! ";
                    ViewBag.message14 = "Course Added Successfully! ";
                    ModelState.Remove("Id");
                    ModelState.Remove("Name");
                    ModelState.Remove("Birthday");
                    ModelState.Remove("Phone");
                    ModelState.Remove("UserName");
                    ModelState.Remove("Password");
                    ModelState.Remove("UserType");
                   
                }
                else
                {
                    ViewBag.message13 = "Invalid Person ID, This Id is Taken.";
                    return View("AddPerson");
                }
            }

            return View("AddPerson");
        }

        public ActionResult AssignStudentCourse(Grades grade)
        {
            dal dal = new dal();
            ViewBag.success2 = "";
            List<Course> coursesName = (from u in dal.Courses //gives all courses name
                                        where (u.mode == true)
                                        select u).ToList<Course>();
            if (grade != null)//if object is null because no id input
            {
                List<User> userslist = (from u in dal.Users //find if there such user
                                        where (u.Id == grade.StudentId && u.mode == true && u.UserType == "Student")
                                        select u).ToList<User>();
                if (userslist.Count > 0) //check if there is a user
                {
                    List<Grades> gradesList = (from u in dal.Grades//give all grade table list
                                               where (u.mode == true)
                                               select u).ToList<Grades>();

                    List<Course> courseidSelectedList = (from u in dal.Courses //gives selected course
                                                      where (u.Id == grade.CourseId && u.mode == true)
                                                      select u).ToList<Course>();

                    //string coursHour = courseidSelectedList[0].Hours;

                    List<int> userCourses = (from u in dal.Grades //gives All courses that studeny teach
                                                         where (u.StudentId == grade.StudentId && u.mode == true)
                                                         select u.CourseId).ToList<int>();


                    bool flagStudentHour = false; 
                    //IF THIS STUDENT TAKES OTHER COURSES AT SAME TIME

                    for (int i=0; i<userCourses.Count;i++)
                    {
                        int currtentId= userCourses[i];
                        List<Course> currentcourse = (from u in dal.Courses //gives All courses that studeny teach
                                         where (u.Id == currtentId && u.mode == true)
                                         select u).ToList<Course>();



                        int temphour = int.Parse(currentcourse[0].Hours);
                        string tempDay = currentcourse[0].Day;
                        int d2 = Convert.ToInt32(courseidSelectedList[0].Hours);

                        if (tempDay== courseidSelectedList[0].Day)
                        { 
                        if (temphour== d2 || temphour == d2+1 || temphour == d2 + 2 || temphour+1 == d2  || temphour+2 == d2  )
                        { 
                            //    if (   temphour == int.Parse(courseidSelectedList[0].Hours) && courseidSelectedList[0].Day==) 
                            flagStudentHour = true;
                        }
                        }
                    }


                    //int courseidSelected = courseidSelectedList.FirstOrDefault();

                    //List<string> studentSelected = (from u in dal.Grades
                    //                                where (u.StudentId == grade.StudentId && u.mode == true)
                    //                                select u.StudentId).ToList<string>();

                    //List<string> studentCourses = (from u in dal.Grades
                    //                               where (u.StudentId == grade.StudentId && u.CourseId == courseidSelected && u.mode == true)
                    //                               select u.StudentId).ToList<string>();

                    if(flagStudentHour)
                    {

                        ViewBag.Error = "FAILED! This Student Alread Learn other course at this TIME!";


                    }


                    if (!flagStudentHour)
                    {
                        grade.CourseId = Convert.ToInt32(courseidSelectedList[0].Id);
                        grade.Grade = 0;
                        grade.GradeA = "Grade A is Not Given Yet";
                        grade.GradeB = "GradeB Not Given Yet";
                        grade.mode = true;

                        dal.Grades.Add(grade);
                        dal.SaveChanges();
                        ViewBag.success2 = "Student assigned to the course successfuly";
                        ModelState.Remove("GradeA");
                        ModelState.Remove("GradeB");


                    }
                    //else if (studentCourses.Count >= 1) ViewBag.Error = "The Student have this course already";
                    //else ViewBag.Error = "The Student have 2 courses already";
                  
                }
                else ViewBag.Error = "No such student, add the student first";
            }

            ViewBag.CoursesList = coursesName;
            return View("AssignStudentCourse");
           
        }
        public ActionResult AddCourse(Course crs)
        {
            dal dal1 = new dal();

            if (crs.ClassRoom != null)
            {
                List<Course> courseslist = (from u in dal1.Courses
                                            where (u.mode == true)
                                            select u).ToList<Course>();

                List<string> lecturerList = (from u in dal1.Users
                                             where (u.UserType == "Lecturer")
                                             select u.Id).ToList<string>();

                bool hasLecturer = false;

                for (int i = 0; i < lecturerList.Count; i++)
                {
                    if (crs.LecId == lecturerList[i])
                        hasLecturer = true;

                }


                if (hasLecturer == false)  // IF not LECTURER
                {
                    ViewBag.message13 = "This Person is Not Lecturer! Cant Assign Coursr.";
                    return View("AddCourse");
                }

                Course find = courseslist.FirstOrDefault(x => x.Id == crs.Id); // IF there IS THIS COURSe ALREady
                bool flagMoad = false;
                bool flagClassroom = false;



                List<Course> courseslistDays = (from u in dal1.Courses      // ALL COURSES THAT IN THIS DAY
                                                where (u.mode == true )
                                                select u).ToList<Course>();

                List<Course> courseslistLec = (from u in dal1.Courses      // ALL COURSES THAT IN THIS DAY
                                                where (u.mode == true && u.LecId==crs.LecId)
                                                select u).ToList<Course>();

                bool flagLecturer = false;


                for (int i = 0; i < courseslistDays.Count; i++)       // CHeck
                {

                    if((courseslistDays[i].Day == crs.Day) && (courseslistDays[i].ClassRoom == crs.ClassRoom )&& ( ( int.Parse(courseslistDays[i].Hours) == int.Parse(crs.Hours)) || (int.Parse(courseslistDays[i].Hours) == int.Parse(crs.Hours)+1) || (int.Parse(courseslistDays[i].Hours) == int.Parse(crs.Hours)+2) || (int.Parse(courseslistDays[i].Hours) == int.Parse(crs.Hours) -1)  || (int.Parse(courseslistDays[i].Hours) == int.Parse(crs.Hours) - 2)))
                        flagClassroom = true;
                
                }


                for(int i=0;i< courseslistLec.Count;i++)
                { 
                if (courseslistLec[i].Day == crs.Day)
                {
                    if (((int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours)) || (int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours) + 1) || (int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours) + 2) || (int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours) - 1) || (int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours) + 3) || (int.Parse(courseslistLec[i].Hours) == int.Parse(crs.Hours) - 2)))
                        flagLecturer = true;
                }
                }



                if (flagLecturer)
                {
                    ViewBag.message13 = "FAILED! This Lecturer IS Teaching At this Day At This TIME! !";
                    return View("AddCourse");
                }

                if (flagClassroom)
                {
                    ViewBag.message13 = "FAILED! In This Class At this time already Have COURSE!";
                    return View("AddCourse");
                }


                if (find == null && hasLecturer)
                {
                    if (crs.MoadB < crs.MoadA)
                        flagMoad = true;
                    if (flagMoad)
                    {
                        ViewBag.message13 = "FAILED! Moad B Cant be before Moad A";
                        return View("AddCourse");
                    }


                    crs.mode = true;
                    dal1.Courses.Add(crs);
                    dal1.SaveChanges();
                    ViewBag.message13 = "Course Added Successfully! ";
                    ModelState.Remove("Id");
                    ModelState.Remove("Name");
                    ModelState.Remove("Points");
                    ModelState.Remove("LecId");
                    ModelState.Remove("Hours");
                    ModelState.Remove("Day");
                    ModelState.Remove("ClassRoom");
                    ModelState.Remove("MoadA");
                    ModelState.Remove("MoadB");

                }
                else
                {
                    ViewBag.message13 = "Invalid Course ID, This Course Id is Taken.";
                    return View("AddCourse");
                }
            }





            return View("AddCourse");
        }
        


            public ActionResult ManageCourseSchedule(Course crs)
            {
                dal dal = new dal();
                ViewBag.success5 = "";
            
                int flag = 0;

          
              List<Course> coursesList = (from u in dal.Courses //gives all courses
                                            where (u.mode == true)
                                            select u).ToList<Course>();

            List<Grades> gradesList = (from u in dal.Grades //gives all grades
                                        where (u.mode == true)
                                        select u).ToList<Grades>();


            ViewBag.courseslist = coursesList;


            if (crs.Day != null)
                {


                List<string> lecidx = (from u in dal.Courses //gives all courses
                                      where (u.mode == true && u.Id == crs.Id)
                                      select u.LecId).ToList<string>();

                List<Course> coursesLecList= new List<Course>();

                for (int i = 0; i < coursesList.Count; i++)
                {
                    if(coursesList[i].LecId== lecidx[0])
                    { 
                    coursesLecList.Add(coursesList[i]);
                    }

                }
                   

                List<string> lecHoursTech=new List<string>() ;

                for(int i = 0; i < coursesLecList.Count; i++)
                {
                    string temp_hour = coursesLecList[i].Hours;

                    lecHoursTech.Add(temp_hour);


                }

                bool flag2 = false;

                Course selected = coursesLecList.Find(x => x.Id == crs.Id);

                if(gradesList!=null)
                { 
                     for (int i = 0; i < gradesList.Count; i++)
                {
                        if (gradesList[i].CourseId == selected.Id)
                        {
                            flag2 = true;
                        }
                }
                }



                //List<int> bbbb = new List<int>();
                //     bbbb = (from u in dal.Grades 
                //                           where (u.CourseId == selected.Id)
                //                           select u.Grade).ToList<int>();
                //bbbb[0] += 1;

                //    if (bbbb[0]>1)
                //    {
                //        flag2 = true; // IF THERE IS STUDENT IN GRADES TBLE IN THIS COURSE

                //    }

                if (flag2)
                {
                    ViewBag.Error4 = "FAILED: in This Course There Registred Students Already, cant change";
                   return View("ManageCourseSchedule");


                }
                else
                {
                    ViewBag.success5 = "Date are update";
                    ModelState.Remove("Day");
                    ModelState.Remove("Hours");
                    return View("ManageCourseSchedule");

                }
                      
                


                for (int k=0;k< lecHoursTech.Count;k++)
                { 

                    for (int i = 0; i < 3; i++)
                    {
                        int h = int.Parse(lecHoursTech[i]);
                        int h2 = int.Parse(crs.Hours);
                        if (h2 + i == h) flag = 1;
                    }


                }



                if (selected != null)
                    {
                        if ( flag == 0)
                        {
                            selected.Day = crs.Day;
                            selected.Hours = crs.Hours;

                            dal.SaveChanges();
                            ViewBag.success5 = "Date are update";
                        ModelState.Remove("Day");
                        ModelState.Remove("Hours");

                    }
                    else if ( flag == 1) ViewBag.Error4 = "FAILED: This Lecturer This Course Is TEACHING At This Time At other Course!";
                    }
                }


               

                return View("ManageCourseSchedule");
            }
        
        public ActionResult ManageExamSchedule(Course crs)
        {
                dal dal = new dal();
                ViewBag.success4 = "";

            bool flagMoad = false;

            List<Course> coursesList = (from u in dal.Courses //gives all courses
                                            where (u.mode == true)
                                            select u).ToList<Course>();

                List<string> coursesNameList = (from u in dal.Courses //gives all courses name
                                                where (u.mode == true)
                                                select u.Name).ToList<string>();
                List<DateTime> moada = (from u in dal.Courses //gives all moad a dates
                                        where (u.MoadA == crs.MoadA && u.mode == true)
                                        select u.MoadA).ToList<DateTime>();
                List<DateTime> moadb = (from u in dal.Courses //gives all moad b dates
                                        where (u.MoadB == crs.MoadB && u.mode == true)
                                        select u.MoadB).ToList<DateTime>();




            ViewBag.coursesName = coursesNameList;

            if (crs != null)
                {
                    Course selected = coursesList.FirstOrDefault(x => x.Name == crs.Name);
                    if (selected != null)
                    {
                        if (moada.Count < 1 && moadb.Count < 1)
                        {
                            selected.MoadA = crs.MoadA;
                            selected.MoadB = crs.MoadB;
                        if (crs.MoadB < crs.MoadA)
                            flagMoad = true;
                        if (flagMoad)
                        {
                             ViewBag.Error = "FAILED! Moad B Cant be before Moad A";
                            return View("ManageExamSchedule");
                        }


                            dal.SaveChanges();
                            ViewBag.success4 = "Date are update";
                        ModelState.Remove("MoadA");
                        ModelState.Remove("MoadB");
                    }
                        else if (moadb.Count > 0) ViewBag.Error = "moad b allready exist for other exam";
                        else if (moada.Count > 0) ViewBag.Error2 = "moad a allready exist for other exam";
                    }
                }


              

                return View("ManageExamSchedule");
            }
        
        public ActionResult UpdateCourseGrade()
        {
            return View("UpdateCourseGrade");
        }
    }
}