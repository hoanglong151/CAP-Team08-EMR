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
    }
}