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
    
    public partial class OTHER_RELATED_COURSES_ATTENDED
    {
        public int ORCID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<int> RankID { get; set; }
        public string FNumber { get; set; }
        public string categoryType { get; set; }
        public string categoryName { get; set; }
        public string Institution { get; set; }
        public string Award { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string RecordedBy { get; set; }
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}
