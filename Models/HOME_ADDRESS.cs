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
    
    public partial class HOME_ADDRESS
    {
        public int HomeAddressID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public string Village { get; set; }
        public string Parish { get; set; }
        public string Subcounty { get; set; }
        public string County { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public string Town { get; set; }
        public string PostalAddress { get; set; }
        public string Email { get; set; }
        public string FaxNo { get; set; }
        public string MobilePhone { get; set; }
        public string FixedTelephone { get; set; }
        public string RecordedBy { get; set; }
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}