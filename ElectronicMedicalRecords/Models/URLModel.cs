using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: InternalsVisibleTo("ElectronicMedicalRecords.Tests")]
namespace ElectronicMedicalRecords.Models
{
    public partial class URLModel
    {
        public int currentInfo { get; set; }
    }
}