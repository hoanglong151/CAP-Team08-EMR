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
    public class MedicalHistoriesControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        MedicalHistoriesController controller = new MedicalHistoriesController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

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
            var medicalHistories = new MedicalHistory
            {
                Name = rand.ToString(),
                ChiDinh = false,
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(medicalHistories) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Create(medicalHistories) as ViewResult;
                Assert.IsNotNull(result1);
            }
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
        public void TestEditGet()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var info = db.MedicalHistories.AsNoTracking().First();
            var result1 = controller.Edit(info.ID) as JsonResult;
            Assert.IsNotNull(result1);
        }
        [TestMethod]
        public void TestEditPost()
        {
            var medicalHistories = db.MedicalHistories.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(medicalHistories) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Edit(medicalHistories) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }
        [TestMethod]
        public void TestDeleteGet()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var medicalHistories = db.MedicalHistories.First();
            var result1 = controller.Delete(medicalHistories.ID) as JsonResult;
            Assert.IsNotNull(result1);
        }
        
    }
}
