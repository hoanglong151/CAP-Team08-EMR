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
    
    public partial class Detail_NhomMau
    {
        public int ID { get; set; }
        public Nullable<int> InformationExamination_ID { get; set; }
        public Nullable<int> NhomMau_ID { get; set; }
        public bool ChiDinh { get; set; }
        public string Result { get; set; }
    
        public virtual InformationExamination InformationExamination { get; set; }
        public virtual NhomMau NhomMau { get; set; }
    }
}