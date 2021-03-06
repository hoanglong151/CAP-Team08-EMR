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
    
    public partial class Bill
    {
        public int ID { get; set; }
        public Nullable<int> Patient_ID { get; set; }
        public Nullable<int> InformationExamination_ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string TypePayment { get; set; }
        public Nullable<int> UserPayment_ID { get; set; }
    
        public virtual InformationExamination InformationExamination { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual User User { get; set; }
    }
}
