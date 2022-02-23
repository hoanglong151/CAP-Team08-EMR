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
    public class TaissControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        TaisController controller = new TaisController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<Tai>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.Tais.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.Tais.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var tai = new Tai
            {
                ChiDinh = false,
                Name = rand.ToString(),
                Dangerous = false
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(tai) as JsonResult;
                Assert.IsNotNull(result);
            }

            tai.Name = "Viêm tai giữa";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(tai) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Tai đã có trong danh sách", noti.responseText);
            }

            tai.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(tai) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var tai = db.Tais.First();
            var result1 = controller.Edit(tai.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var tai = db.Tais.FirstOrDefault();
            tai.Name = "Viêm ống tai ngoài";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(tai) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Tai đã có trong danh sách", data.responseText);
            }
            tai.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(tai) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(tai) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var tai = db.Tais.First();
            var result1 = controller.Delete(tai.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic noti = result1.Data;
            Assert.IsNotNull(noti.data);
        }

        [TestMethod]
        public void TestCreateOldPaitent()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var result = controller.CreateOldPatient(multiplesModel) as PartialViewResult;
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void TestDeleteP()
        {
            var tai = db.Tais.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(2) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti = result.Data;
                Assert.AreEqual(true, noti.success);

                var result2 = controller.DeleteConfirmed(tai.ID) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti2 = result2.Data;
                Assert.AreEqual(false, noti2.success);
                Assert.AreEqual("Tai này đã được sử dụng. Bạn không thể xóa nó!", noti2.responseText);
            }
        }

        public MultiplesModel MockSessionAll()
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
            mockSession.SetupGet(s => s["MultipleModelsAll"]).Returns(multiplesModel); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;
            return multiplesModel;
        }

        [TestMethod]
        public void EditSelectTest()
        {
            var multiple = MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.EditSelect(multiple) as PartialViewResult;
                Assert.IsNotNull(result);
                var tai = db.Tais.First();
            }
        }
        [TestMethod]
        public void GetArrTaiTest()
        {
            var tai = db.InformationExaminations.FirstOrDefault(p => p.ID == 678);
            using (var scope = new TransactionScope())
            {
                var result1 = controller.GetArrTai(tai.ID) as JsonResult;
                Assert.IsNotNull(result1);
            }
        }
    }
}
