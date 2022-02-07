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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.InformationExaminations = new HashSet<InformationExamination>();
            this.Clinicals = new HashSet<Clinical>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public string Introduction { get; set; }
        public string TrainingProcess { get; set; }
        public string WorkingProcess { get; set; }
        public string SpecializedTreatment { get; set; }
        public string UserID { get; set; }
        public string Image { get; set; }
        public bool IsShow { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> Nation_ID { get; set; }
        public Nullable<int> HomeTown_ID { get; set; }
        public string Address { get; set; }
        public Nullable<int> Phone { get; set; }
        public bool Privacy { get; set; }
        public Nullable<int> Religion_ID { get; set; }
        public Nullable<int> Gender_ID { get; set; }
        public bool ActiveAccount { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual HomeTown HomeTown { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InformationExamination> InformationExaminations { get; set; }
        public virtual Nation Nation { get; set; }
        public virtual Religion Religion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clinical> Clinicals { get; set; }
    }
}
