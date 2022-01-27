using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: InternalsVisibleTo("ElectronicMedicalRecords.Tests")]
namespace ElectronicMedicalRecords.Models
{
    public partial class MultiplesModel
    {
        public InformationExamination InformationExamination { get; set; }
        public Patient Patient { get; set; }
        public Medication Medication { get; set; }
        public Prescription_Detail Prescription_Detail { get; set; }
        public DiagnosticsCategory DiagnosticsCategory { get; set; }
        public Detail_DiagnosticsCategory Detail_DiagnosticsCategory { get; set; }
        public List<Prescription_Detail> Prescription_Details { get; set; }
        public Detail_CTMau Detail_CTMau { get; set; }
        public List<CTMau> CTMau { get; set; }
        public List<Detail_CTMau> Detail_CTMaus { get; set; }
        public List<SinhHoaMau> SinhHoaMau { get; set; }
        public List<Detail_SinhHoaMau> Detail_SinhHoaMaus { get; set; }
        public List<DongMau> DongMau { get; set; }
        public List<Detail_DongMau> Detail_DongMaus { get; set; }
        public List<NhomMau> NhomMau { get; set; }
        public List<Detail_NhomMau> Detail_NhomMaus { get; set; }
        public List<Urine> Urine { get; set; }
        public List<Detail_Urine> Detail_Urines { get; set; }
        public List<Immune> Immune { get; set; }
        public List<Detail_Immune> Detail_Immunes { get; set; }
        public List<Amniocente> Amniocente { get; set; }
        public List<Detail_Amniocente> Detail_Amniocentes { get; set; }
        public List<ViSinh> ViSinh { get; set; }
        public List<Detail_ViSinh> Detail_ViSinhs { get; set; }
        public List<HistoryDisease> HistoryDiseases1 { get; set; }
        public List<HistoryDisease> HistoryDiseases2 { get; set; }
        public List<HistoryDisease> HistoryDiseases3 { get; set; }
        public List<Detail_HistoryDisease> Detail_HistoryDiseases1 { get; set; }
        public List<Detail_HistoryDisease> Detail_HistoryDiseases2 { get; set; }
        public List<Detail_HistoryDisease> Detail_HistoryDiseases3 { get; set; }
        public List<MedicalHistory> MedicalHistories { get; set; }
        public List<Detail_MedicalHistory> Detail_MedicalHistories { get; set; }
        public Clinical Clinical { get; set; }

        public List<TuanHoan> TuanHoan { get; set; }
        public List<HoHap> HoHap { get; set; }
        public List<TieuHoa> TieuHoa { get; set; }
        public List<ThanTietNieu> ThanTietNieu { get; set; }
        public List<CoXuongKhop> CoXuongKhop { get; set; }
        public List<ThanKinh> ThanKinh { get; set; }
        public List<TamThan> TamThan { get; set; }
        public List<SanPhuKhoa> SanPhuKhoa { get; set; }
        public List<NgoaiKhoa> NgoaiKhoa { get; set; }
        public List<Mat> Mat { get; set; }
        public List<Tai> Tai { get; set; }
        public List<Mui> Mui { get; set; }
        public List<Hong> Hong { get; set; }
        public List<RangHamMat> RangHamMat { get; set; }
        public List<DaLieu> DaLieu { get; set; }
    }
}