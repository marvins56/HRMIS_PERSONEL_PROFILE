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
    
    public partial class ACCOMMODATION
    {
        public int AccomodationID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public string Type { get; set; }
        public string TypeOfStructure { get; set; }
        public string RecordedBy { get; set; }
        public string BarracksName { get; set; }
        public string HouseNumber { get; set; }
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}
