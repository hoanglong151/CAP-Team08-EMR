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
    
    public partial class SanPhuKhoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPhuKhoa()
        {
            this.Detail_SanPhuKhoa = new HashSet<Detail_SanPhuKhoa>();
        }
    
        public int ID { get; set; }
        public bool ChiDinh { get; set; }
        public string Name { get; set; }
        public bool Dangerous { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_SanPhuKhoa> Detail_SanPhuKhoa { get; set; }
    }
}
