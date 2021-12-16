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
using Moq;
using System.Web;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class Prescription_DetailControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        Prescription_DetailController controller = new Prescription_DetailController();

        public MultiplesModel MockPrescription()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var prescriptionDetail = new Prescription_Detail
            {
                Note = "OK",
                NumMedication = 3
            };
            var info = db.InformationExaminations.AsNoTracking().First();
            var patient = db.Patients.AsNoTracking().First(p => p.ID == info.Patient_ID);
            var clinical = db.Clinicals.AsNoTracking().First(p => p.InformationExamination_ID == info.ID);
            var prescription = db.Prescription_Detail.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Amniocente = db.Detail_Amniocente.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_CTMaus = db.Detail_CTMau.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_DMaus = db.Detail_DongMau.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Immune = db.Detail_Immune.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_NMaus = db.Detail_NhomMau.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_SHM = db.Detail_SinhHoaMau.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Urine = db.Detail_Urine.AsNoTracking().Where(p => p.InfomationExamination_ID == info.ID).ToList();
            var detail_VS = db.Detail_ViSinh.AsNoTracking().Where(p => p.InformationExamination_ID == info.ID).ToList();
            multiplesModel.Prescription_Detail = prescriptionDetail;
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
            return multiplesModel;
        }

        [TestMethod]
        public void Index()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DetailIE()
        {
            var info = db.InformationExaminations.First();
            var result = controller.DetailIE(info.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_DetailIE", result.ViewName);
        }

        [TestMethod]
        public void DetailIERead()
        {
            var info = db.InformationExaminations.First();
            var result = controller.DetailIERead(info.ID) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_DetailIERead", result.ViewName);
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
            var multiple = MockPrescription();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(multiple) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("CreateTest", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void EditG()
        {
            var multiple = MockPrescription();
            var result = controller.Edit(multiple) as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("_Edit", result.ViewName);
        }

        [TestMethod]
        public void LoadPrescription()
        {
            var info = db.InformationExaminations.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadPrescription(info.ID) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
        }

        [TestMethod]
        public void EditP()
        {
            var multiple = MockPrescription();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(multiple) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Edit", result.RouteValues["action"]);
            }

            controller.ModelState.Clear();
            multiple.Prescription_Details = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.Edit(multiple) as RedirectToRouteResult;
                Assert.IsNotNull(resultError);
                Assert.AreEqual("Edit", resultError.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void EditMedicationP()
        {
            var multiple = MockPrescription();
            using (var scope = new TransactionScope())
            {
                var result = controller.EditMedication(multiple) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            controller.ModelState.Clear();
            multiple.Prescription_Details = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.EditMedication(multiple) as JsonResult;
                Assert.IsNotNull(resultError);
                dynamic data = resultError.Data;
                Assert.AreEqual(true, data.success);
            }
        }
    }
}
