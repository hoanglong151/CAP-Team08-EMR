using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: InternalsVisibleTo("ElectronicMedicalRecords.Tests")]
namespace ElectronicMedicalRecords.Models
{
    public partial class StatisticModel
    {
        public User usersStatis { get; set; }
        public DiagnosticsCategory diagnosticsCategory { get; set; }
        public PatientStatu patientStatu { get; set; }
        public int countPatient { get; set; }

        public int priceExamination { get; set; }
        public int pricePrescription { get; set; }
        public int priceSubclinical { get; set; }
    }
}