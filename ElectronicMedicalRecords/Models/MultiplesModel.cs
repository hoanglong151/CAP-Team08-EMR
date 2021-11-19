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
    }
}