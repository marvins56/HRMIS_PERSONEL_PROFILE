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
    using System.ComponentModel.DataAnnotations;

    public partial class NEXT_OF_KIN1
    {
        public int NextOfKin1ID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Village { get; set; }
        public string Parish { get; set; }
        public string Town { get; set; }
        public string Subcounty { get; set; }
        public string County { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public string Address { get; set; }
        public string TelephoneNo { get; set; }
        public string RecordedBy { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}
