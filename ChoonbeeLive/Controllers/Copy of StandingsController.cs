/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChoonbeeLive.Models;

namespace ChoonbeeLive.Controllers
{
    public class StandingsController : Controller
    {
        private league_pimaEntities db = new league_pimaEntities();

        //
        // GET: /Standings/

        private IEnumerable<GeneralStanding> GetGeneralStandings(bool filter = false)
        {
            var standing = from t in db.Tournaments
                            join d in db.Divisions on t.TournamentId equals d.TournamentId
                            join dw in db.DivisionWinners on d.DivisionId equals dw.DivisionId
                            join p in db.Participants on dw.ParticipantId equals p.ParticipantId
                            join s in db.Schools on p.SchoolId equals s.SchoolId
                            where p.League == true
                            select new GeneralStanding
                            {
                                T = t,
                                D = d,
                                Dw = dw,
                                P = p,
                                S = s
                            };

            if (filter)
                return standing.Where(x => x.T.TournamentId == 5 || x.T.TournamentId == 6 || x.T.TournamentId == 4);
            return standing;
        }

        private IEnumerable<Standing<School>> GetSchoolStandings(bool filter = false)
        {
            var generalStandings = GetGeneralStandings(filter);
            var grouped = from x in generalStandings
                          group x.Dw by x.P.School into gp
                          select new Standing<School>
                          {
                              MemberKey = gp.Key,
                              Score = gp.Sum(y => y.Points)
                          };

            return grouped.OrderByDescending(x => x.Score);
        }

        private IEnumerable<Standing<Participant>> GetParticipantStandings(bool filter = false)
        {
            var generalStandings = GetGeneralStandings(filter);
            var grouped = from x in generalStandings
                          group x.Dw by x.P into gp
                          select new Standing<Participant>
                          {
                              MemberKey = gp.Key,
                              Score = gp.Sum(y => y.Points)
                          };
            return grouped.OrderByDescending(x => x.Score).Take(10);
        }


        public ActionResult Index()
        {
            ViewBag.LifetimeSchoolStanding = GetSchoolStandings();
            ViewBag.LifetimeParticipantStanding = GetParticipantStandings();

            ViewBag.SeasonSchoolStanding = GetSchoolStandings(true);
            ViewBag.SeasonParticipantStanding = GetParticipantStandings(true);

            return View();
        }

    }
}
*/