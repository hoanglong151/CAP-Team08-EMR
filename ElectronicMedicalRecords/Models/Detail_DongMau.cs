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
    
    public partial class Detail_DongMau
    {
        public int ID { get; set; }
        public Nullable<int> InformationExamination_ID { get; set; }
        public Nullable<int> DongMau_ID { get; set; }
        public bool ChiDinh { get; set; }
        public Nullable<double> Result { get; set; }
    
        public virtual DongMau DongMau { get; set; }
        public virtual InformationExamination InformationExamination { get; set; }
    }
}
