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

    public partial class DEPENDANT
    {
        public int DependantID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string RecordedBy { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}
