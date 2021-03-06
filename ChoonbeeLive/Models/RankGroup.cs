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
    
    public partial class RankGroup
    {
        public RankGroup()
        {
            this.DivisionDefaults = new HashSet<DivisionDefault>();
            this.Divisions = new HashSet<Division>();
            this.Ranks = new HashSet<Rank>();
        }
    
        public int RankGroupId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    
        public virtual ICollection<DivisionDefault> DivisionDefaults { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
