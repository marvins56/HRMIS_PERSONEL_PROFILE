//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMIS_PERSONEL_PROFILE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RECORD_ON_PROMOTION
    {
        public int PromotionID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public Nullable<int> RankID { get; set; }
        public Nullable<System.DateTime> DatePromoted { get; set; }
        public string RecordedBy { get; set; }
        public Nullable<System.DateTime> DateRecorded { get; set; }
        public Nullable<short> state { get; set; }
        public string currentStatus { get; set; }
    }
}
