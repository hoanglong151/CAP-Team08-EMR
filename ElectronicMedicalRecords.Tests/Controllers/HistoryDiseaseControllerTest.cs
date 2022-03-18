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
    public class HistoryDiseaseControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        HistoryDiseasesController controller = new HistoryDiseasesController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.HistoryDiseases.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateOldPaitent()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var patient = db.Patients.FirstOrDefault();
            multiplesModel.Patient = patient;
            var result = controller.CreateOldPatient(multiplesModel) as PartialViewResult;
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void TestCreateGet()
        {
            var result = controller.Create() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Create", result.ViewName);

        }
        [TestMethod]
        public void TestCreatePost()
        {
            var rand = new Random();
            var history = new HistoryDisease
            {
                ChiDinh = false,
                Name = rand.ToString(),
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(history) as JsonResult;
                Assert.IsNotNull(result);
            }

            history.Name = "Béo Phì";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(history) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Bệnh Tiền Sử đã có trong danh sách", noti.responseText);
            }

            history.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(history) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestEditGet()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var history = db.HistoryDiseases.First();
            var result1 = controller.Edit(history.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditPost()
        {
            var rand = new Random();
            var history = db.HistoryDiseases.FirstOrDefault();
            history.Name = "Thiếu Máu";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(history) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Bệnh Tiền Sử đã có trong danh sách", data.responseText);
            }
            history.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(history) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(history) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var history = db.HistoryDiseases.First();
            var result1 = controller.Delete(history.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic noti = result1.Data;
            Assert.IsNotNull(noti.data);
        }
        // [TestMethod]
        //public void TestDeletePost()
        //{
        //    var history = db.HistoryDiseases.AsNoTracking().First();
        //    using (var scope = new TransactionScope())
        //    {
        //        var result = controller.DeleteConfirmed(history.ID) as RedirectToRouteResult;
        //        Assert.IsNotNull(result);
        //        Assert.AreEqual("Index", result.RouteValues["action"]);
        //    }
        //}
    }
}
