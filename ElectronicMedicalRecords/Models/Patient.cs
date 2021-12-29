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
    
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.InformationExaminations = new HashSet<InformationExamination>();
            this.Bills = new HashSet<Bill>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Gender_ID { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Address { get; set; }
        public Nullable<int> HomeTown_ID { get; set; }
        public Nullable<int> Nation_ID { get; set; }
        public Nullable<int> Phone { get; set; }
        public string InsuranceCode { get; set; }
        public string MedicalHistory { get; set; }
        public string HistoryDisease { get; set; }
        public Nullable<int> Nation1_ID { get; set; }
        public string MaBN { get; set; }
    
        public virtual Gender Gender { get; set; }
        public virtual HomeTown HomeTown { get; set; }
        public virtual Nation Nation { get; set; }
        public virtual Nation1 Nation1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InformationExamination> InformationExaminations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
