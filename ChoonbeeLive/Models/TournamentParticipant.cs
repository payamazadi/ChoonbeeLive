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
    
    public partial class TournamentParticipant
    {
        public int TournamentId { get; set; }
        public int ParticipantId { get; set; }
    
        public virtual Participant Participant { get; set; }
        public virtual Tournament Tournament { get; set; }
        public virtual Tournament Tournament1 { get; set; }
    }
}
