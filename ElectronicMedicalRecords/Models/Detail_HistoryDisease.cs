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
    
    public partial class Detail_HistoryDisease
    {
        public int ID { get; set; }
        public Nullable<int> HistoryDisease_ID { get; set; }
        public Nullable<int> Patient_ID { get; set; }
        public string LevelFamily { get; set; }
        public bool Selected { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual HistoryDisease HistoryDisease { get; set; }
    }
}
