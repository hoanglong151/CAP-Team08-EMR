using ElectronicMedicalRecords;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        DashboardController controller = new DashboardController();
        [TestMethod]
        public void GetData()
        {
            var result = controller.GetData() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Dashboard()
        {
            var result = controller.Dashboard() as ViewResult;
            Assert.IsNotNull(result);
        }
    }

}
