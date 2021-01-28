using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoonbeeLive.Models
{
    public class Standing<T>
    {
        public T MemberKey { get; set; }
        public int Score { get; set; }

        public Standing()
        {
        }
    }

    public class GeneralStanding
    {
        public Tournament T {get; set;}
        public Division D { get; set; }
        public DivisionWinner Dw { get; set; }
        public Participant P { get; set; }
        public School S { get; set; }

        public GeneralStanding()
        {
        }
    }

    public class PopDivisionWinner
    {
        public Division D { get; set; }
        public Participant P { get; set; }
        public int Count { get; set; }
    }

    /*
     * FriendlyId = grp.Key.FriendlyId,
                                        AgeGroupName = grp.Max(x => x.ag.Name),
                                        MinAge = grp.Max(x => x.ag.MinAge),
                                        RankGroupName = grp.Max(x => x.rg.Name),
                                        RankGroupSortOrder = grp.Max(x => x.rg.SortOrder),
                                        Gender = grp.Max(x => x.Genders),
                                        FirstName = grp.Key.p.FirstName,
                                        LastName = grp.Key.p.LastName,
                                        Points = grp.Sum(x => x.dw.Points)
     */
    public class CompleteStanding
    {
        public string FriendlyId { get; set; }
        public string AgeGroupName { get; set; }
        public int MinAge { get; set; }
        public string RankGroupName { get; set; }
        public int RankGroupSortOrder { get; set; }
        public string Gender { get; set; }
        public Participant Participant { get; set; }
        public int Points { get; set; }
    }
}