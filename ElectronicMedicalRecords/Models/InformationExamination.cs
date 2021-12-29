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
    
    public partial class InformationExamination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InformationExamination()
        {
            this.CayMaus = new HashSet<CayMau>();
            this.Clinicals = new HashSet<Clinical>();
            this.Detail_Amniocente = new HashSet<Detail_Amniocente>();
            this.Detail_CTMau = new HashSet<Detail_CTMau>();
            this.Detail_DongMau = new HashSet<Detail_DongMau>();
            this.Detail_Immune = new HashSet<Detail_Immune>();
            this.Detail_NhomMau = new HashSet<Detail_NhomMau>();
            this.Detail_SinhHoaMau = new HashSet<Detail_SinhHoaMau>();
            this.Detail_Urine = new HashSet<Detail_Urine>();
            this.Detail_ViSinh = new HashSet<Detail_ViSinh>();
            this.Prescription_Detail = new HashSet<Prescription_Detail>();
            this.Bills = new HashSet<Bill>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> DateExamine { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> HeartBeat { get; set; }
        public Nullable<int> Breathing { get; set; }
        public Nullable<int> BloodPressure { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<int> PatientStatus_ID { get; set; }
        public Nullable<int> Patient_ID { get; set; }
        public Nullable<int> DiagnosticCategory_ID { get; set; }
        public Nullable<bool> ResultCTMau { get; set; }
        public Nullable<bool> ResultSHM { get; set; }
        public Nullable<bool> ResultDMau { get; set; }
        public Nullable<bool> ResultNhomMau { get; set; }
        public Nullable<bool> ResultNuocTieu { get; set; }
        public Nullable<bool> ResultMienDich { get; set; }
        public Nullable<bool> ResultDichChocDo { get; set; }
        public Nullable<bool> ResultViSinh { get; set; }
        public Nullable<bool> New { get; set; }
        public Nullable<int> PriceExamination { get; set; }
        public Nullable<int> PriceCTMaus { get; set; }
        public Nullable<int> PriceTest { get; set; }
        public Nullable<int> PricePrescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CayMau> CayMaus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clinical> Clinicals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Amniocente> Detail_Amniocente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_CTMau> Detail_CTMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_DongMau> Detail_DongMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Immune> Detail_Immune { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_NhomMau> Detail_NhomMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_SinhHoaMau> Detail_SinhHoaMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Urine> Detail_Urine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_ViSinh> Detail_ViSinh { get; set; }
        public virtual DiagnosticsCategory DiagnosticsCategory { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual PatientStatu PatientStatu { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription_Detail> Prescription_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
