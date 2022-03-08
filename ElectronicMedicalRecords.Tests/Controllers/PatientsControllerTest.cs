using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class PatientsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        PatientsController controller = new PatientsController();
        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestDetailIE()
        {
            var patient = db.Patients.FirstOrDefault();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Patient = patient;
            var resultPatient = controller.DetailIE(multiplesModel) as PartialViewResult;
            Assert.IsNotNull(multiplesModel);
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
                HomeTown_ID = 1,
                InsuranceCode = rand.ToString(),
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
                HomeTown_ID = patient.HomeTown_ID,
                InsuranceCode = patient.InsuranceCode,
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
            var patient = db.Patients.FirstOrDefault();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Patient = patient;
            var resultPatient = controller.Edit(multiplesModel) as PartialViewResult;
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
                HomeTown_ID = patient.HomeTown_ID,
                InsuranceCode = patient.InsuranceCode,
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

        [TestMethod]
        public void GetData()
        {
            List<string> Noti = new List<string>();
            var listInfo = db.InformationExaminations.Where(p => p.PatientStatus_ID == 44).ToList();
            foreach (var info in listInfo)
            {
                info.Patient = db.Patients.FirstOrDefault(p => p.ID == info.Patient_ID);
                if (!Noti.Contains(info.Patient.MaBN))
                {
                    Noti.Add(info.Patient.MaBN);
                }
            }
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Noti"]).Returns(Noti); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;

            var result = controller.GetData() as JsonResult;
            Assert.IsNotNull(result);
            dynamic check = result.Data;
            Assert.AreEqual(db.Patients.Count(), check.data.Count);
        }

        public MultiplesModel MockSessionBillAll()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.First();
            var patient = db.Patients.First(p => p.ID == info.Patient_ID);
            var clinical = db.Clinicals.First(p => p.InformationExamination_ID == info.ID);
            var prescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Amniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_DMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Immune = db.Detail_Immune.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_NMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_SHM = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Urine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == info.ID).ToList();
            var detail_VS = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == info.ID).ToList();
            multiplesModel.Detail_SinhHoaMaus = detail_SHM;
            multiplesModel.Detail_Urines = detail_Urine;
            multiplesModel.Detail_ViSinhs = detail_VS;
            multiplesModel.Detail_NhomMaus = detail_NMaus;
            multiplesModel.Detail_Immunes = detail_Immune;
            multiplesModel.Detail_DongMaus = detail_DMaus;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            multiplesModel.Detail_Amniocentes = detail_Amniocente;
            multiplesModel.Clinical = clinical;
            multiplesModel.Patient = patient;
            multiplesModel.Prescription_Details = prescription;
            multiplesModel.InformationExamination = info;

            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["MultipleModelsPatient"]).Returns(multiplesModel); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;
            return multiplesModel;
        }

        [TestMethod]
        public void PrintBillExaminationPost()
        {
            var multiple = MockSessionBillAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintBillExaminationPost(multiple) as JsonResult;
                Assert.IsNotNull(result);
                dynamic check = result.Data;
                Assert.AreEqual(true, check.success);
            }
        }

        [TestMethod]
        public void PrintBillPrescription()
        {
            MockSessionBillAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintBillPrescription() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintBillExamination()
        {
            MockSessionBillAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintBillExamination() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintBillTestSubclinical()
        {
            MockSessionBillAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintBillTestSubclinical() as ViewResult;
                Assert.IsNotNull(result);
            }
        }
    }
}
