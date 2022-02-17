using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class Detail_MedicalHistoriesControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        Detail_MedicalHistoryController controller = new Detail_MedicalHistoryController();

        [TestMethod]
        public void TestIndex()
        {
            var patient = db.Patients.FirstOrDefault();
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestDetail()
        {
            var info = db.Detail_MedicalHistory.AsNoTracking().First();
            var list = controller.Details(info.ID) as ViewResult;
            Assert.IsNotNull(list);


            var infoError = controller.Details(0) as HttpNotFoundResult;
            Assert.IsNotNull(infoError);

        }
        [TestMethod]
        public void TestDetailIE()
        {
            var info = db.Detail_MedicalHistory.AsNoTracking().First();
            var list = controller.DetailIE(info.ID) as PartialViewResult;
            Assert.IsNotNull(list);
            Assert.AreEqual("_DetailIE", list.ViewName);
      
        }
        [TestMethod]
        public void BillCheck()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            using (var scope = new TransactionScope())
            {
                var result = controller.BillCheck(multiplesModel) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_BillCheck", result.ViewName);
            }
        }
    }
}
