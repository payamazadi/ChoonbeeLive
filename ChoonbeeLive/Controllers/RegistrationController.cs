using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChoonbeeLive.Models;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Shared;

namespace ChoonbeeLive.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string username, string password)
        {
            if (MongoWrapper.db.GetCollection<User>("users").AsQueryable<User>().Where(u => u.Id == username && u.Password == password).Count() == 1)
            {
                Session["Authenticated"] = true;
                Session["Username"] = username;
            }
            return Redirect("/");
        }


        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(string username, string password, bool over18 = false)
        {
            //if email exists
                //redirect with failure message
            var userCollection = MongoWrapper.db.GetCollection<User>("users");
            if(userCollection.AsQueryable<User>().Where(u => u.Id == username).Count() > 0)
            {
                ModelState.AddModelError("UsernameTaken", "That email is already in use.<br>If you have participated in a DMV Sport Karate League tournament in the past, an email was sent to the email address on file with your password. <br>Click <a href=\"/Registration/ResetPassword\">here</a> to reset password.<br>Click <a href=\"/Registration/SignIn\">here</a> to sign in.<br>If you continue to have trouble, dial xxx-xxx-xxxx<br><br>");
            }
            else if(over18 == false)
            {
                ModelState.AddModelError("Error18", "You must be over 18 to create an account.");
                return View();
            }
            else
            {
                //create and redirect home
                var user = new User(username);
                user.Password = password;
                user.Participants = new List<Participant>();
                userCollection.Insert(user);
            }
            
            return View();
        }

        public ActionResult Logout()
        {
            Session["Authenticated"] = false;
            Session["Username"] = "";
            return Redirect("/");
        }

    }
}
