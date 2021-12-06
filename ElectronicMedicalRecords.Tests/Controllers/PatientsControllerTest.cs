using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class PatientsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        PatientsController controller = new PatientsController();
        MultiplesModel multiplesModel = new MultiplesModel();
        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<Patient>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.Patients.Count(), model.Count);
        }

        [TestMethod]
        public void TestDetailIE()
        {
            var result = controller.DetailIE(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var patient = db.Patients.FirstOrDefault();
            var resultPatient = controller.DetailIE(patient.ID) as PartialViewResult;
            Assert.IsNotNull(patient);
            Assert.AreEqual("_DetailIE", resultPatient.ViewName);
        }

        [TestMethod]
        public void TestSearchPatient()
        {
            var patient = db.Patients.First();
            var informationExaminationList = db.InformationExaminations.ToList();
            var informationExamination = informationExaminationList.LastOrDefault();
            var resultPatient = controller.SearchPatient(informationExamination.DateExamine, informationExamination.DateEnd, patient.Name, patient.MaBN) as ViewResult;
            Assert.IsNotNull(resultPatient);

            var checkNull = controller.SearchPatient(null, null, "", null) as RedirectToRouteResult;
            Assert.IsNotNull(checkNull);
            Assert.AreEqual("Index", checkNull.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCreateG()
        {
            var resultPatient = controller.Create() as PartialViewResult;
            Assert.IsNotNull(resultPatient);
            Assert.AreEqual("_Create", resultPatient.ViewName);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var patient = new Patient
            {
                Address = rand.ToString(),
                BirthDate = DateTime.Now,
                Gender_ID = 1,
                HistoryDisease = rand.ToString(),
                HomeTown_ID = 1,
                InsuranceCode = rand.ToString(),
                MedicalHistory = rand.ToString(),
                Name = rand.ToString(),
                Nation1_ID = 1,
                Nation_ID =  1,
                Phone = 0987654321,
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(patient) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Create", result.RouteValues["action"]);
            }

            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Create(patient) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }

        [TestMethod]
        public void TestCreateOldPatientG()
        {
            var patient = db.Patients.First();
            var resultPatient = controller.CreateOldPatient(patient.ID) as PartialViewResult;
            Assert.IsNotNull(resultPatient);
            Assert.AreEqual("_CreateOldPatient", resultPatient.ViewName);
        }

        [TestMethod]
        public void TestCreateOldPatientP()
        {
            var patient = db.Patients.First();
            var newPatient = new Patient
            {
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Gender_ID = patient.Gender_ID,
                HistoryDisease = patient.HistoryDisease,
                HomeTown_ID = patient.HomeTown_ID,
                InsuranceCode = patient.InsuranceCode,
                MedicalHistory = patient.MedicalHistory,
                Name = patient.Name,
                Nation1_ID = patient.Nation1_ID,
                Nation_ID = patient.Nation_ID,
                Phone = patient.Phone,
                MaBN = patient.MaBN,
                ID = patient.ID
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(newPatient) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("CreateOldPatient", result.RouteValues["action"]);
            }

            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.CreateOldPatient(newPatient) as RedirectToRouteResult;
                Assert.IsNotNull(result1);
                Assert.AreEqual("CreateOldPatient", result1.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var patient = db.Patients.First();
            var resultPatient = controller.Edit(patient.ID) as PartialViewResult;
            Assert.IsNotNull(resultPatient);
            Assert.AreEqual("_Edit", resultPatient.ViewName);
        }

        [TestMethod]
        public void TestEditP()
        {
            var patient = db.Patients.First();
            var patientUpdate = new Patient
            {
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Gender_ID = patient.Gender_ID,
                HistoryDisease = patient.HistoryDisease,
                HomeTown_ID = patient.HomeTown_ID,
                InsuranceCode = patient.InsuranceCode,
                MedicalHistory = patient.MedicalHistory,
                Name = patient.Name,
                Nation1_ID = patient.Nation1_ID,
                Nation_ID = patient.Nation_ID,
                Phone = patient.Phone,
                MaBN = patient.MaBN,
                ID = patient.ID
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(patientUpdate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Edit", result.RouteValues["action"]);
            }

            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result1 = controller.Edit(patientUpdate) as ViewResult;
                Assert.IsNotNull(result1);
            }
        }
    }
}
