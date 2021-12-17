using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: InternalsVisibleTo("ElectronicMedicalRecords.Tests")]
namespace ElectronicMedicalRecords.Models
{
    public partial class DashboardModel
    {
        public List<AspNetUser> userBS { get; set; }
        public List<AspNetUser> userGD { get; set; }
        public List<AspNetUser> userKTV { get; set; }
        public List<AspNetUser> userQTV { get; set; }
        public List<AspNetUser> userTN { get; set; }
        public List<AspNetUser> userYTa { get; set; }
        public List<Patient> patients { get; set; }
        public List<User> users { get; set; }
    }
}