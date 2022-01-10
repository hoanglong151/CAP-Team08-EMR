﻿using ElectronicMedicalRecords.Models;
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
using System.Threading.Tasks;
using System.Web;
using Moq;
using System.Security.Principal;
using System.Security.Claims;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class InformationExaminationsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        InformationExaminationsController controller = new InformationExaminationsController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<InformationExamination>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.InformationExaminations.Count(), model.Count);
        }

        [TestMethod]
        public void GetNotification()
        {
            var result = controller.GetNotification() as Task<ActionResult>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNotificationBS()
        {
            var result = controller.GetNotificationBS() as Task<ActionResult>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNotificationBS1()
        {
            var result = controller.GetNotificationBS1() as Task<ActionResult>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DetailsG()
        {
            var info = db.InformationExaminations.First();
            var result = controller.Details(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SearchPatientDetail()
        {
            var patient = db.Patients.First();
            var result = controller.SearchPatientDetail(DateTime.Now, DateTime.Now, patient.ID) as ViewResult;
            Assert.IsNotNull(result);

            var resultError = controller.SearchPatientDetail(null, null, patient.ID) as RedirectToRouteResult;
            Assert.IsNotNull(resultError);
            Assert.AreEqual("Details", resultError.RouteValues["action"]);
        }

        [TestMethod]
        public void DetailIE()
        {
            var info = db.InformationExaminations.First();
            var result = controller.DetailIE(info.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_DetailsIE", result.ViewName);

            var resultError = controller.DetailIE(0) as HttpNotFoundResult;
            Assert.IsNotNull(resultError);
        }

        public class MockHttpContextBase : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }

        [TestMethod]
        public void CreateG()
        {
            var patient = db.Patients.FirstOrDefault();
            var user = db.AspNetUsers.First();
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "Giám Đốc" });
            var fakeHttpContext = new MockHttpContextBase { User = genericPrincipal };
            var controllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };
            controller.ControllerContext = controllerContext;
            var result = controller.Create(patient.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Create", result.ViewName);
        }

        [TestMethod]
        public void CreateP()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            var patient = db.Patients.First();
            var patientStatus = db.PatientStatus.First();
            informationExamination.PatientStatus_ID = patientStatus.ID;
            multiplesModel.InformationExamination = informationExamination;
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(patient.ID, multiplesModel) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Create", result.RouteValues["action"]);
            }

            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var resultError = controller.Create(patient.ID, multiplesModel) as ViewResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void BillCheck()
        {
            var user = db.AspNetUsers.First();
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "Giám Đốc" });
            var fakeHttpContext = new MockHttpContextBase { User = genericPrincipal };
            var controllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };
            controller.ControllerContext = controllerContext;
            var info = db.InformationExaminations.First();
            var result = controller.BillCheck(info.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_BillCheck", result.ViewName);

            var resultError = controller.BillCheck(0) as HttpNotFoundResult;
            Assert.IsNotNull(resultError);
        }

        [TestMethod]
        public void CreateOldPatient()
        {
            var user = db.AspNetUsers.First();
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "Giám Đốc" });
            var fakeHttpContext = new MockHttpContextBase { User = genericPrincipal };
            var controllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };
            controller.ControllerContext = controllerContext;
            var info = db.InformationExaminations.First();
            var result = controller.CreateOldPatient(info.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_CreateOldPatient", result.ViewName);

            var resultError = controller.CreateOldPatient(0) as HttpNotFoundResult;
            Assert.IsNotNull(resultError);
        }

        [TestMethod]
        public void CreateOldPatientP()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            var patient = db.Patients.First();
            var patientStatus = db.PatientStatus.First();
            informationExamination.PatientStatus_ID = patientStatus.ID;
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Patient = patient;
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(multiplesModel) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("CreateOldPatient", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var resultError = controller.CreateOldPatient(multiplesModel) as ViewResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateTest(info) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var resultError = controller.CreateTest(info) as ViewResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void EditG()
        {
            var user = db.AspNetUsers.First();
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "Giám Đốc" });
            var fakeHttpContext = new MockHttpContextBase { User = genericPrincipal };
            var controllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };
            controller.ControllerContext = controllerContext;
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(info.ID) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_Edit", result.ViewName);
            }
            using (var scope = new TransactionScope())
            {
                var resultError = controller.Edit(0) as HttpNotFoundResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void EditP()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(info) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var resultError = controller.Edit(info) as ViewResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void DeleteConfirmed()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(info.ID) as JsonResult;
                Assert.IsNotNull(result);
            }
        }
    }
}
