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
    
    public partial class WarrantUserlog
    {
        public long logID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Time { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string ActionTaken { get; set; }
        public string ComputerName { get; set; }
    }
}
