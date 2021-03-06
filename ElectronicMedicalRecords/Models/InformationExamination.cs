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
            this.Bills = new HashSet<Bill>();
            this.CayMaus = new HashSet<CayMau>();
            this.Clinicals = new HashSet<Clinical>();
            this.Detail_Amniocente = new HashSet<Detail_Amniocente>();
            this.Detail_CoXuongKhop = new HashSet<Detail_CoXuongKhop>();
            this.Detail_CTMau = new HashSet<Detail_CTMau>();
            this.Detail_DaLieu = new HashSet<Detail_DaLieu>();
            this.Detail_DiagnosticsCategory = new HashSet<Detail_DiagnosticsCategory>();
            this.Detail_DongMau = new HashSet<Detail_DongMau>();
            this.Detail_HoHap = new HashSet<Detail_HoHap>();
            this.Detail_Immune = new HashSet<Detail_Immune>();
            this.Detail_Mat = new HashSet<Detail_Mat>();
            this.Detail_NgoaiKhoa = new HashSet<Detail_NgoaiKhoa>();
            this.Detail_NhomMau = new HashSet<Detail_NhomMau>();
            this.Detail_RangHamMat = new HashSet<Detail_RangHamMat>();
            this.Detail_SanPhuKhoa = new HashSet<Detail_SanPhuKhoa>();
            this.Detail_SinhHoaMau = new HashSet<Detail_SinhHoaMau>();
            this.Detail_TaiMuiHong = new HashSet<Detail_TaiMuiHong>();
            this.Detail_TamThan = new HashSet<Detail_TamThan>();
            this.Detail_ThanKinh = new HashSet<Detail_ThanKinh>();
            this.Detail_ThanTietNieu = new HashSet<Detail_ThanTietNieu>();
            this.Detail_TieuHoa = new HashSet<Detail_TieuHoa>();
            this.Detail_TuanHoan = new HashSet<Detail_TuanHoan>();
            this.Detail_Urine = new HashSet<Detail_Urine>();
            this.Detail_ViSinh = new HashSet<Detail_ViSinh>();
            this.Prescription_Detail = new HashSet<Prescription_Detail>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> DateExamine { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> UserTest_ID { get; set; }
        public Nullable<int> HeartBeat { get; set; }
        public Nullable<int> Breathing { get; set; }
        public Nullable<int> BloodPressure { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<int> PatientStatus_ID { get; set; }
        public Nullable<int> Patient_ID { get; set; }
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
        public Nullable<int> PriceTest { get; set; }
        public Nullable<int> PricePrescription { get; set; }
        public Nullable<int> PriceCTMaus { get; set; }
        public string Specialist { get; set; }
        public Nullable<bool> Examining { get; set; }
        public Nullable<bool> Resulting { get; set; }
        public Nullable<System.DateTime> DateReExamination { get; set; }
        public Nullable<bool> ExaminationType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CayMau> CayMaus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clinical> Clinicals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Amniocente> Detail_Amniocente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_CoXuongKhop> Detail_CoXuongKhop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_CTMau> Detail_CTMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_DaLieu> Detail_DaLieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_DiagnosticsCategory> Detail_DiagnosticsCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_DongMau> Detail_DongMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_HoHap> Detail_HoHap { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Immune> Detail_Immune { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Mat> Detail_Mat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_NgoaiKhoa> Detail_NgoaiKhoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_NhomMau> Detail_NhomMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_RangHamMat> Detail_RangHamMat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_SanPhuKhoa> Detail_SanPhuKhoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_SinhHoaMau> Detail_SinhHoaMau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_TaiMuiHong> Detail_TaiMuiHong { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_TamThan> Detail_TamThan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_ThanKinh> Detail_ThanKinh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_ThanTietNieu> Detail_ThanTietNieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_TieuHoa> Detail_TieuHoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_TuanHoan> Detail_TuanHoan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Urine> Detail_Urine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_ViSinh> Detail_ViSinh { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual PatientStatu PatientStatu { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription_Detail> Prescription_Detail { get; set; }
    }
}
