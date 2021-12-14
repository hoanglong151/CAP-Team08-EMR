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
    public class DongMausControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        DongMausController controller = new DongMausController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<DongMau>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.DongMaus.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.DongMaus.Count(), check.data.Count);
        }

        [TestMethod]
        public void CreateOldPatientG()
        {
            var result = controller.CreateOldPatient() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_CreateOldPatient", result.ViewName);
        }

        [TestMethod]
        public void CreateG()
        {
            var result = controller.Create() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Create", result.ViewName);
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var dongmau = db.DongMaus.First();
            var result1 = controller.Edit(dongmau.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var dongMau = db.DongMaus.AsNoTracking().FirstOrDefault();
            dongMau.NameTest = rand.ToString();
            dongMau.Unit = rand.ToString().Substring(0, 9);
            dongMau.Price = rand.Next();
            dongMau.ChiDinh = false;
            dongMau.CSBT = rand.ToString();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(dongMau) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            dongMau.NameTest = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Edit(dongMau) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Không thể cập nhật giá", data.responseText);
            }
        }
    }
}
