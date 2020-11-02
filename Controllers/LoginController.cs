using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Dal;
using System.Web.Security;
using WebApplication3.ModelView;
using WebApplication3.Classes;

namespace WebApplication3.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["Student"] = null;
            Session["Administrator"] = null;
            Session["Lecturer"] = null;
            return RedirectToAction("Login");
        }


        public ActionResult Authenticate(User usr)
        {
            bool flag = false;
            dal dal = new dal();
            Encryption enc = new Encryption();
            if (ModelState.IsValid)
            {

                //code to check if user==password and exist on db
                //transfer to homepage



                List<User> userValidated = (from u in dal.Users
                                            where (u.UserName == usr.UserName)
                                            select u).ToList<User>();
                if (userValidated != null)
                {
                    //if (enc.ValidatePassword(usr.Password, userValidated[0].Password))
                    if (userValidated.Count > 0 && usr.Password == userValidated[0].Password)
                    {
                        ViewBag.message = "Login Successfully!";
                        flag = true;
                    }
                    else
                        ViewBag.message = "Incorrect Username/Password!";


                }
                if (flag == true)
                {
                    flag = false;
                    if (userValidated[0].UserType == "Student")
                    {
                        //user authenticated successfully
                        FormsAuthentication.SetAuthCookie("cookie", true);
                        Session["Student"] = userValidated[0].Id;
                    }
                    else if  (userValidated[0].UserType == "Lecturer")
                    {
                        FormsAuthentication.SetAuthCookie("cookie", true);
                        Session["Lecturer"] = userValidated[0].Id;
                    }
                    else if (userValidated[0].UserType == "Administrator")
                    {
                        FormsAuthentication.SetAuthCookie("cookie", true);
                        Session["Administrator"] = userValidated[0].Id;
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Login", usr);
                }

            }

            else
            {
                ViewBag.message = "Invalid Username/Password!";
                return View("Login", usr);
            }


        }
        //Galit's Change
        public ActionResult Register()
        {
            dal dal = new dal();
            User usr = new User();
            return View("AddUser", usr);

        }
        [HttpPost]
        public ActionResult Submit()
        {

            User objUser = new User();
            objUser.UserName = Request.Form["UserName"].ToString();
            objUser.Password = Request.Form["Password"].ToString();
            dal dal = new dal();
            Encryption enc = new Encryption();

            if (ModelState.IsValid)
            {
                string hashedPassword = enc.CreateHash(objUser.Password);
                objUser.Password = hashedPassword;
                try
                {
                    dal.Users.Add(objUser);
                    dal.SaveChanges();
                    ViewBag.message = "User was added Successfuly!";
                }
                catch (Exception)
                {
                    TempData["error"] = "There was a problem in registration, possible reason: user already exist.\n"; // print error message
                    return View("AddUser");
                }
            }
            else
                ViewBag.message = "Error in registration!";




            return View("Success");
        }

    }
}