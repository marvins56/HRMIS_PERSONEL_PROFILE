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
    
    public partial class Desertion_Table
    {
        public int Dessertion_ID { get; set; }
        public int EmpID { get; set; }
        public string FNumber { get; set; }
        public string Status { get; set; }
        public System.DateTime Date { get; set; }
        public int NumberDays { get; set; }
        public string State { get; set; }
    
        public virtual PERSONAL_INFORMATION PERSONAL_INFORMATION { get; set; }
    }
}
