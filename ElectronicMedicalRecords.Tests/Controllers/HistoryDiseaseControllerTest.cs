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
            var historyDisease = new HistoryDisease
            {
                Name = rand.ToString(),
                ChiDinh = true,
                Dangerous = false,
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(historyDisease) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Create(historyDisease) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }
        [TestMethod]
        public void TestEditGet()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var info = db.HistoryDiseases.AsNoTracking().First();
            var result1 = controller.Edit(info.ID) as ViewResult;
            Assert.IsNotNull(result1);
        }
        [TestMethod]
        public void TestEditPost()
        {
            var historyDisease = db.HistoryDiseases.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(historyDisease) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Edit(historyDisease) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }
        [TestMethod]
        public void TestDeleteGet()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var history = db.HistoryDiseases.First();
            var result1 = controller.Delete(history.ID) as ViewResult;
            Assert.IsNotNull(result1);

          
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
