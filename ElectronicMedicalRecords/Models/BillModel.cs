using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: InternalsVisibleTo("ElectronicMedicalRecords.Tests")]
namespace ElectronicMedicalRecords.Models
{
    public partial class BillModel
    {
        public Patient patient { get; set; }
        public InformationExamination informationExamination { get; set; }

        public string priceExamination { get; set; }
        public string pricePrescription { get; set; }
        public string priceSubclinical { get; set; }
    }
}