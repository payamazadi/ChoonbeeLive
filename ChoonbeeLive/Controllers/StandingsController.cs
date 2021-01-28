using System;
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

        public enum StandingsType
        {
            TOURNAMENT,
            SEASON,
            LIFETIME
        }

        private IEnumerable<GeneralStanding> GetGeneralStandings(Dictionary<string, int> filters)
        {
            var MinTournamentId = filters["MinTournamentId"];
            var MaxTournamentId = filters["MaxTournamentId"];
            var MinSeasonId = filters["MinSeasonId"];
            var MaxSeasonId = filters["MaxSeasonId"];
            var MinSchoolId = filters["MinSchoolId"];
            var MaxSchoolId = filters["MaxSchoolId"];
            var MinLeague = Convert.ToBoolean(filters["MinLeague"]);
            var MaxLeague = Convert.ToBoolean(filters["MaxLeague"]);
            var MinDivisionTypeId = filters["MinDivisionTypeId"];
            var MaxDivisionTypeId = filters["MaxDivisionTypeId"];

            var standing = from t in db.Tournaments
                           join d in db.Divisions on t.TournamentId equals d.TournamentId
                           join dw in db.DivisionWinners on d.DivisionId equals dw.DivisionId
                           join p in db.Participants on dw.ParticipantId equals p.ParticipantId
                           join s in db.Schools on p.SchoolId equals s.SchoolId
                           where d.DivisionTypeId >= MinDivisionTypeId && d.DivisionTypeId <= MaxDivisionTypeId &&
                                        t.TournamentId >= MinTournamentId && t.TournamentId <= MaxTournamentId &&
                                        t.SeasonId >= MinSeasonId && t.SeasonId <= MaxSeasonId &&
                                        p.SchoolId >= MinSchoolId && p.SchoolId <= MaxSchoolId &&
                                        (p.League == MinLeague || p.League == MaxLeague)
                           select new GeneralStanding
                           {
                               T = t,
                               D = d,
                               Dw = dw,
                               P = p,
                               S = s
                           };

            return standing;
        }

        private IEnumerable<Standing<School>> GetSchoolStandings(Dictionary<string, int> filters)
        {
            var generalStandings = GetGeneralStandings(filters);
            var grouped = from x in generalStandings
                          group x.Dw by x.P.School into gp
                          select new Standing<School>
                          {
                              MemberKey = gp.Key,
                              Score = gp.Sum(y => y.Points)
                          };

            return grouped.OrderByDescending(x => x.Score);
        }



        private IEnumerable<Standing<PopDivisionWinner>> GetPopularWinners(Dictionary<string, int> filters)
        {
            var standingList = new List<Standing<PopDivisionWinner>>();
            var MinTournamentId = filters["MinTournamentId"];
            var MaxTournamentId = filters["MaxTournamentId"];
            var MinSeasonId = filters["MinSeasonId"];
            var MaxSeasonId = filters["MaxSeasonId"];
            var MinSchoolId = filters["MinSchoolId"];
            var MaxSchoolId = filters["MaxSchoolId"];
            var MinLeague = Convert.ToBoolean(filters["MinLeague"]);
            var MaxLeague = Convert.ToBoolean(filters["MaxLeague"]);
            var MinDivisionTypeId = filters["MinDivisionTypeId"];
            var MaxDivisionTypeId = filters["MaxDivisionTypeId"];

            var popTournaments = from t in db.Tournaments
                                 join d in db.Divisions on t.TournamentId equals d.TournamentId
                                 join dp in db.DivisionParticipants on d.DivisionId equals dp.DivisionId
                                 join p in db.Participants on dp.ParticipantId equals p.ParticipantId
                                 where d.DivisionTypeId >= MinDivisionTypeId && d.DivisionTypeId <= MaxDivisionTypeId && 
                                        t.TournamentId >= MinTournamentId && t.TournamentId <= MaxTournamentId &&
                                        t.SeasonId >= MinSeasonId && t.SeasonId <= MaxSeasonId &&
                                        p.SchoolId >= MinSchoolId && p.SchoolId <= MaxSchoolId &&
                                        (p.League == MinLeague || p.League == MaxLeague)
                                 group d by d into dg
                                 where dg.Count() > 1
                                 select new { Division = dg.Key, Counter = dg.Count() };
            popTournaments = popTournaments.OrderByDescending(x => x.Counter).Take(10);

            var db2 = new league_pimaEntities();
            
            foreach (var popTournament in popTournaments)
            {
                var x = (from dw in db2.DivisionWinners
                         where dw.Rank == 1 && dw.DivisionId == popTournament.Division.DivisionId
                         select dw.Participant);

                if (x.Count() > 0)
                {
                    var p = x.First();
                    var pdw = new PopDivisionWinner
                    {
                        D = popTournament.Division,
                        P = p,
                        Count = popTournament.Counter
                    };

                    standingList.Add(new Standing<PopDivisionWinner>
                    {
                        MemberKey = pdw,
                        Score = 5
                    });
                }
                
            }

            return standingList;
        }
         

        /*
        public IEnumerable<CompleteStanding> GetCompleteStandingsGeneral(int divisionTypeId)
        {
            var completeStandings = from d in db.Divisions
                                    join t in db.Tournaments on d.TournamentId equals t.TournamentId
                                    join dw in db.DivisionWinners on d.DivisionId equals dw.DivisionId
                                    join ag in db.AgeGroups on d.AgeGroupId equals ag.AgeGroupId
                                    join rg in db.RankGroups on d.RankGroupId equals rg.RankGroupId
                                    join p in db.Participants on dw.ParticipantId equals p.ParticipantId
                                    where d.DivisionTypeId == divisionTypeId
                                    orderby t.DateHeld descending, ag.MinAge, rg.SortOrder, d.Genders, d.FriendlyId, dw.Points descending
                                    select new CompleteStanding 
                                    { DivisionId = d.DivisionId, SeasonId = t.SeasonId, TournamentId = d.TournamentId, FriendlyId = d.FriendlyId, AgeGroupName = ag.Name, RankGroupName = rg.Name, RankGroupId = rg.RankGroupId, Participant = p };

            return completeStandings;
        }
        */
        public Dictionary<string, int> setFilterParameters(Dictionary<string, int> filters)
        {
            var filter = new Dictionary<string, int>
            {
                {"MinTournamentId", 0},
                {"MaxTournamentId", Int32.MaxValue},
                {"MinSeasonId", 0},
                {"MaxSeasonId", Int32.MaxValue},
                {"MinSchoolId", 0},
                {"MaxSchoolId", Int32.MaxValue},
                {"MinLeague", 0},
                {"MaxLeague", 1},
                {"MinDivisionTypeId", 0},
                {"MaxDivisionTypeId", Int32.MaxValue}
            };

            SetFilterRanges(filter, filters, "TournamentId");
            SetFilterRanges(filter, filters, "SeasonId");
            SetFilterRanges(filter, filters, "SchoolId");
            SetFilterRanges(filter, filters, "League");
            SetFilterRanges(filter, filters, "DivisionTypeId");

            return filter;
        }

        private void SetFilterRanges(Dictionary<string, int> filter, Dictionary<string, int> filters, string key)
        {
            if (filters.ContainsKey(key))
            {
                filter["Min" + key] = filters[key];
                filter["Max" + key] = filters[key];
            }
        }

        public IEnumerable<CompleteStanding> GetCompleteStanding(Dictionary<string, int> filters)
        {
            /*TODO stupid LINQ to Entities won't let me inline these dictionary evaluations.. */
            var MinTournamentId = filters["MinTournamentId"];
            var MaxTournamentId = filters["MaxTournamentId"];
            var MinSeasonId = filters["MinSeasonId"];
            var MaxSeasonId = filters["MaxSeasonId"];
            var MinSchoolId = filters["MinSchoolId"];
            var MaxSchoolId = filters["MaxSchoolId"];
            var MinLeague = Convert.ToBoolean(filters["MinLeague"]);
            var MaxLeague = Convert.ToBoolean(filters["MaxLeague"]);
            var MinDivisionTypeId = filters["MinDivisionTypeId"];
            var MaxDivisionTypeId = filters["MaxDivisionTypeId"];

            var completeStandings = from d in db.Divisions
                                    join t in db.Tournaments on d.TournamentId equals t.TournamentId
                                    join dw in db.DivisionWinners on d.DivisionId equals dw.DivisionId
                                    join ag in db.AgeGroups on d.AgeGroupId equals ag.AgeGroupId
                                    join rg in db.RankGroups on d.RankGroupId equals rg.RankGroupId
                                    join p in db.Participants on dw.ParticipantId equals p.ParticipantId
                                    where d.DivisionTypeId >= MinDivisionTypeId && d.DivisionTypeId <= MaxDivisionTypeId && 
                                        t.TournamentId >= MinTournamentId && t.TournamentId <= MaxTournamentId &&
                                        t.SeasonId >= MinSeasonId && t.SeasonId <= MaxSeasonId &&
                                        p.SchoolId >= MinSchoolId && p.SchoolId <= MaxSchoolId &&
                                        (p.League == MinLeague || p.League == MaxLeague)
                                    group new { d.Genders, ag, rg, dw, p } by new { d.FriendlyId, p } into grp
                                    select new CompleteStanding { 
                                        FriendlyId = grp.Key.FriendlyId,
                                        AgeGroupName = grp.Max(x => x.ag.Name),
                                        MinAge = grp.Max(x => x.ag.MinAge),
                                        RankGroupName = grp.Max(x => x.rg.Name),
                                        RankGroupSortOrder = grp.Max(x => x.rg.SortOrder),
                                        Gender = grp.Max(x => x.Genders),
                                        Participant = grp.Key.p,
                                        Points = grp.Sum(x => x.dw.Points)
                                    };

            completeStandings = completeStandings.OrderBy(x => x.MinAge).
                ThenBy(x => x.RankGroupSortOrder).
                ThenBy(x => x.Gender).
                ThenBy(x => x.FriendlyId).
                ThenByDescending(x => x.Points);

            return completeStandings;
        }

        public ActionResult Index()
        {
            ViewBag.SchoolStanding = GetSchoolStandings(setFilterParameters(new Dictionary<string, int> {{"League", 1}, {"SeasonId", 3}}));
            ViewBag.PopularWinners = GetPopularWinners(setFilterParameters(new Dictionary<string, int>{{"SeasonId", 3}}));

            ViewBag.CompleteStandingsWeapons = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 2 }, { "SeasonId", 3 } }));
            ViewBag.CompleteStandingsForms = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 3 }, { "SeasonId", 3 } }));
            ViewBag.CompleteStandingsSparring = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 1 }, { "SeasonId", 3 } }));

            ViewBag.Archives = TournamentArchives();
            return View();
        }

        public ActionResult Lifetime()
        {
            ViewBag.SchoolStanding = GetSchoolStandings(setFilterParameters(new Dictionary<string, int> { { "League", 1 } }));
            ViewBag.PopularWinners = GetPopularWinners(setFilterParameters(new Dictionary<string, int> ()));

            ViewBag.CompleteStandingsWeapons = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 2 } }));
            ViewBag.CompleteStandingsForms = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 3 } }));
            ViewBag.CompleteStandingsSparring = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 1 } }));

            ViewBag.Archives = TournamentArchives();
            return View();
        }

        public ActionResult Season(int id)
        {
            ViewBag.SchoolStanding = GetSchoolStandings(setFilterParameters(new Dictionary<string, int> { { "League", 1 }, {"SeasonId", id} }));
            ViewBag.PopularWinners = GetPopularWinners(setFilterParameters(new Dictionary<string, int>()));

            ViewBag.CompleteStandingsWeapons = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 2 }, {"SeasonId", id} }));
            ViewBag.CompleteStandingsForms = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 3 }, { "SeasonId", id } }));
            ViewBag.CompleteStandingsSparring = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 1 }, { "SeasonId", id } }));

            var season = db.Seasons.Find(id);
            ViewBag.Heading = season.Name + " Champions (" + season.Description + ")";
            ViewBag.Archives = TournamentArchives();
            return View();
        }

        public ActionResult Tournament(int id)
        {
            ViewBag.SchoolStanding = GetSchoolStandings(setFilterParameters(new Dictionary<string, int> { { "League", 1 }, {"TournamentId", id} }));
            ViewBag.PopularWinners = GetPopularWinners(setFilterParameters(new Dictionary<string, int>()));

            ViewBag.CompleteStandingsWeapons = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 2 }, { "TournamentId", id } }));
            ViewBag.CompleteStandingsForms = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 3 }, { "TournamentId", id } }));
            ViewBag.CompleteStandingsSparring = GetCompleteStanding(setFilterParameters(new Dictionary<string, int> { { "DivisionTypeId", 1 }, { "TournamentId", id } }));

            var tournament = db.Tournaments.Find(id);
            ViewBag.Heading = tournament.Name + " Champion (" + tournament.Season.Name + ")";
            ViewBag.Archives = TournamentArchives();
            return View();
        }

        public string TournamentArchives()
        {
            var ret = "<h1><a href=\"/Standings/Lifetime\">Grand Champions To-Date</a></h1>";
            var seasons = from s in db.Seasons
                          orderby s.Name descending
                          select s;
            var db2 = new league_pimaEntities();

            foreach (var season in seasons)
            {
                ret += "<h3><a href=\"/Standings/Season/" + season.SeasonId.ToString() + "\">" + season.Name + " (" + season.Description + "</a>)</h3>";
                var tournaments = from t in db2.Tournaments
                                  where t.SeasonId == season.SeasonId
                                  orderby t.DateHeld descending
                                  select t;

                foreach (var tournament in tournaments)
                    ret += "---<a href=\"/Standings/Tournament/" + tournament.TournamentId + "\">" + tournament.Name + "</a><br>";
            }

            return ret;
        }


    }
}
