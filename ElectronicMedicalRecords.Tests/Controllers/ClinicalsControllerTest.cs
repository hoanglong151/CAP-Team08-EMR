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
    public class ClinicalsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        ClinicalsController controller = new ClinicalsController();

        [TestMethod]
        public void TestDetailIE()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            var list = controller.DetailIE(info.ID) as PartialViewResult;
            Assert.IsNotNull(list);
            Assert.AreEqual("_DetailIE", list.ViewName);


            var infoError = controller.DetailIE(0) as HttpNotFoundResult;
            Assert.IsNotNull(infoError);
        }

        [TestMethod]
        public void CreateOldPatientG()
        {
            var result = controller.CreateOldPatient() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_CreateOldPatient", result.ViewName);
        }

        [TestMethod]
        public void CreateOldPatientP()
        {
            var rand = new Random();
            var clinical = new Clinical
            {
                RightEyesNoGlasses = rand.NextDouble(),
                LeftEyesNoGlasses = rand.NextDouble(),
                RightEyesGlasses = rand.NextDouble(),
                LeftEyesGlasses = rand.NextDouble(),
                LeftEarSay = rand.NextDouble(),
                LeftEarWhisper = rand.NextDouble(),
                RightEarSay = rand.NextDouble(),
                RightEarWhisper = rand.NextDouble(),
                UpperJaw = rand.ToString(),
                LowerJaw = rand.ToString(),
                User_ID = db.Users.First().ID,
            };
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.AsNoTracking().First();
            multiplesModel.InformationExamination = info;
            multiplesModel.Clinical = clinical;
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(multiplesModel) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Create", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.CreateOldPatient(multiplesModel) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }

        [TestMethod]
        public void CreateG()
        {
            var result = controller.Create() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Create", result.ViewName);
        }

        [TestMethod]
        public void CreateP()
        {
            var rand = new Random();
            var clinical = new Clinical
            {
                RightEyesNoGlasses = rand.NextDouble(),
                LeftEyesNoGlasses = rand.NextDouble(),
                RightEyesGlasses = rand.NextDouble(),
                LeftEyesGlasses = rand.NextDouble(),
                LeftEarSay = rand.NextDouble(),
                LeftEarWhisper = rand.NextDouble(),
                RightEarSay = rand.NextDouble(),
                RightEarWhisper = rand.NextDouble(),
                UpperJaw = rand.ToString(),
                LowerJaw = rand.ToString(),
                User_ID = db.Users.First().ID,
            };
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.AsNoTracking().First();
            multiplesModel.InformationExamination = info;
            multiplesModel.Clinical = clinical;
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(multiplesModel) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Create", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Create(multiplesModel) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var info = db.InformationExaminations.AsNoTracking().First();
            var result1 = controller.Edit(info.ID) as PartialViewResult;
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void TestEditP()
        {
            var clinical = db.Clinicals.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Clinical = clinical;
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(multiplesModel) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Edit", result.RouteValues["action"]);
            }

            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Edit(multiplesModel) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }
    }
}
