using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ChoonbeeLive.Controllers
{
    public class ApiTournamentController : Controller
    {
        //
        // GET: api/Tournament/

        public ActionResult Index()
        {
            string lol = "kaka";
            return Json(lol, JsonRequestBehavior.AllowGet);
        }

    }
}
