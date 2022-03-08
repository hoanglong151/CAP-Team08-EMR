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
    public class MedicationCategoriesControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        MedicationCategoriesController controller = new MedicationCategoriesController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<MedicationCategory>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.MedicationCategories.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.MedicationCategories.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var medicationCategory = new MedicationCategory
            {
                Name = rand.ToString()
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(medicationCategory) as JsonResult;
                Assert.IsNotNull(result);
            }

            medicationCategory.Name = "Ibuprofen";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(medicationCategory) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Danh mục thuốc đã có trong danh sách", noti.responseText);
            }

            medicationCategory.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(medicationCategory) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var medicationCategory = db.MedicationCategories.First();
            var result1 = controller.Edit(medicationCategory.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var medicationCategory = db.MedicationCategories.AsNoTracking().FirstOrDefault();
            medicationCategory.Name = rand.ToString();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(medicationCategory) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            medicationCategory.Name = "Ibuprofen";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(medicationCategory) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Danh mục thuốc đã có trong danh sách", data.responseText);
            }

            medicationCategory.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(medicationCategory) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var medicationCategory = db.MedicationCategories.First();
            var result1 = controller.Delete(medicationCategory.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic noti = result1.Data;
            Assert.IsNotNull(noti.data);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var medication = db.Medications.FirstOrDefault();
            var medicationCategory = db.MedicationCategories.FirstOrDefault(p => p.ID == medication.MedicationCategory_ID); ;
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(medicationCategory.ID) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti = result.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Danh mục này đã được sử dụng cho thuốc. Bạn không thể xóa nó!", noti.responseText);
            }
        }
    }
}
