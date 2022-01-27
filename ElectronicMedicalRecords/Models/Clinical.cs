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
    
    public partial class Clinical
    {
        public int ID { get; set; }
        public Nullable<double> RightEyesNoGlasses { get; set; }
        public Nullable<double> LeftEyesNoGlasses { get; set; }
        public Nullable<double> RightEyesGlasses { get; set; }
        public Nullable<double> LeftEyesGlasses { get; set; }
        public Nullable<double> LeftEarSay { get; set; }
        public Nullable<double> LeftEarWhisper { get; set; }
        public Nullable<double> RightEarSay { get; set; }
        public Nullable<double> RightEarWhisper { get; set; }
        public string UpperJaw { get; set; }
        public string LowerJaw { get; set; }
        public string Dermatology { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> InformationExamination_ID { get; set; }
    
        public virtual InformationExamination InformationExamination { get; set; }
        public virtual User User { get; set; }
    }
}
