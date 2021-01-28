using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChoonbeeLive.Models;

namespace ChoonbeeLive.Controllers
{
    public class TournamentController : Controller
    {
        private league_pimaEntities db = new league_pimaEntities();

        //
        // GET: /Tournament/

        public ActionResult Index()
        {
            ViewBag.Past = db.Tournaments.Where(t => t.DateHeld < DateTime.Now).OrderByDescending(t => t.DateHeld).ToList();
            ViewBag.Future = db.Tournaments.Where(t => t.DateHeld >= DateTime.Now).OrderBy(t => t.DateHeld).ToList();
            return View();
        }

    }
}
