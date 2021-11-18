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
    
    public partial class Medication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medication()
        {
            this.Prescription_Detail = new HashSet<Prescription_Detail>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public Nullable<double> Price { get; set; }
        public int MedicationCategory_ID { get; set; }
    
        public virtual MedicationCategory MedicationCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription_Detail> Prescription_Detail { get; set; }
        public virtual Prescription_Detail Prescription_Details { get; set; }
    }
}
