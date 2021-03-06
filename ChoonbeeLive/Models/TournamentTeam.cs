//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChoonbeeLive.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TournamentTeam
    {
        public TournamentTeam()
        {
            this.Participants = new HashSet<Participant>();
        }
    
        public int TeamId { get; set; }
        public int TournamentId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
    
        public virtual School School { get; set; }
        public virtual Tournament Tournament { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
