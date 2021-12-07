using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectronicMedicalRecords.Models
{
    public partial class MultiplesModel
    {
        public InformationExamination InformationExamination { get; set; }
        public Patient Patient { get; set; }
        public Medication Medication { get; set; }
        public Prescription_Detail Prescription_Detail { get; set; }
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
        public Clinical Clinical { get; set; }
        public CayMau CayMau { get; set; }
    }
}