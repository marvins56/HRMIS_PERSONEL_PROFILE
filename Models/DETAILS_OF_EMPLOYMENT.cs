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

    public partial class DETAILS_OF_EMPLOYMENT
    {
        public int EmptStatusID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string FNumber { get; set; }
        public string EmploymentStatus { get; set; }
        public System.DateTime DateOfEnlistment { get; set; }
        public string PlaceOfEnlistment { get; set; }
        public string TypeOfEnlistment { get; set; }
        public Nullable<int> InstitutionID { get; set; }
        public string DurationOfTraining { get; set; }
        public string Duration { get; set; }
        public Nullable<System.DateTime> DateOfPosting { get; set; }
        public string PlaceOfPosting { get; set; }
        public string RecordedBy { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateRecorded { get; set; }
    }
}
