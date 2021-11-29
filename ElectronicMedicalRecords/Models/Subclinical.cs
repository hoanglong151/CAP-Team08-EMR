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
    
    public partial class Subclinical
    {
        public int ID { get; set; }
        public Nullable<double> NumRedBloodCells { get; set; }
        public Nullable<double> NumWhiteBloodCells { get; set; }
        public Nullable<double> NumPlatelet { get; set; }
        public Nullable<double> BloodSugar { get; set; }
        public Nullable<double> URE { get; set; }
        public Nullable<double> Creatinin { get; set; }
        public Nullable<double> ASAT { get; set; }
        public Nullable<double> ALAT { get; set; }
        public string OtherBlood { get; set; }
        public Nullable<double> Sugar { get; set; }
        public Nullable<double> Protein { get; set; }
        public string OtherUrine { get; set; }
        public string ImageDiagnostic { get; set; }
        public Nullable<int> Bill { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> InformationExamination_ID { get; set; }
    
        public virtual User User { get; set; }
        public virtual InformationExamination InformationExamination { get; set; }
    }
}
