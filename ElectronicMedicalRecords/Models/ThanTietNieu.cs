//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicMedicalRecords.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThanTietNieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThanTietNieu()
        {
            this.Detail_ThanTietNieu = new HashSet<Detail_ThanTietNieu>();
        }
    
        public int ID { get; set; }
        public Nullable<bool> ChiDinh { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Dangerous { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_ThanTietNieu> Detail_ThanTietNieu { get; set; }
    }
}
