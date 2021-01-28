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
    public class AccountController : Controller
    {
        private league_pimaEntities db = new league_pimaEntities();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            var userCollection = MongoWrapper.db.GetCollection<User>("users");
            var user = userCollection.AsQueryable<User>().Single(u => u.Id.Equals((string)Session["Username"]));

            var dbParticipants = new List<Participant>();
            foreach(var participant in user.Participants)
                dbParticipants.Add(db.Participants.Find(participant.ParticipantId));

            ViewBag.Competitors = dbParticipants;
            return View();
        }

        public ActionResult EnterCompetitor()
        {
            var userCollection = MongoWrapper.db.GetCollection<User>("users");
            var user = userCollection.AsQueryable<User>().Single(u => u.Id.Equals((string)Session["Username"]));

            db.Configuration.ProxyCreationEnabled = false;

            //user.Participants.Add(db.Participants.Find(31));
            //userCollection.Save(user);
            return RedirectToAction("Index");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((string)Session["Username"] != "")
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = new RedirectResult("Home");
        }
    }
}
