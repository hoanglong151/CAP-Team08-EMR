using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            MultiplesModel multiplesModel = new MultiplesModel();
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
    }
}
