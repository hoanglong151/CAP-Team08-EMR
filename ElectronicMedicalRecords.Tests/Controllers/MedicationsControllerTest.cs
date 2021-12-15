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
    public class MedicationsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        MedicationsController controller = new MedicationsController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<Medication>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.Medications.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.Medications.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var medication = new Medication
            {
                Name = rand.ToString(),
                Unit = rand.ToString(),
                Price = rand.Next(),
                MedicationCategory_ID = rand.Next(1, 166)
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(medication) as JsonResult;
                Assert.IsNotNull(result);
            }

            medication.Name = "Humulin 70/30";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(medication) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Tên thuốc đã có trong danh sách", noti.responseText);
            }

            medication.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(medication) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var medication = db.Medications.First();
            var result1 = controller.Edit(medication.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var medication = db.Medications.AsNoTracking().FirstOrDefault();
            medication.Name = rand.ToString();
            medication.Unit = rand.ToString();
            medication.Price = rand.Next();
            medication.MedicationCategory_ID = rand.Next(1, 166);
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(medication) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            medication.Name = "Tegrucil-1";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(medication) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Tên thuốc đã có trong danh sách", data.responseText);
            }

            medication.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(medication) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var medication = db.Medications.First();
            var result1 = controller.Delete(medication.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic noti = result1.Data;
            Assert.IsNotNull(noti.data);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var medication = db.Medications.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(medication.ID) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti = result.Data;
                Assert.AreEqual(true, noti.success);
            }
        }

        [TestMethod]
        public void TestFindMedication()
        {
            var medication = db.Medications.First();
            var result = controller.FindMedication(medication.ID) as JsonResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateG()
        {
            var result = controller.Create() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Create", result.ViewName);
        }
    }
}
