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
    
    public partial class WCardTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WCardTable()
        {
            this.WIssueTables = new HashSet<WIssueTable>();
        }
    
        public int CardID { get; set; }
        public int WarrantID { get; set; }
        public int EmpID { get; set; }
        public int CardNo { get; set; }
        public string CardStatus { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual WarrantTable WarrantTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WIssueTable> WIssueTables { get; set; }
    }
}
