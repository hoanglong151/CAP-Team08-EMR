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
using System.Threading.Tasks;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class StatisticsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        StatisticsController controller = new StatisticsController();

        public List<StatisticModel> MockSession()
        {
            List<StatisticModel> statisticModels = new List<StatisticModel>();

            var Infomation = db.InformationExaminations.FirstOrDefault(p => p.DiagnosticCategory_ID != null);
            var getDiagnostic = db.DiagnosticsCategories.AsNoTracking().FirstOrDefault(p => p.ID == Infomation.DiagnosticCategory_ID);
            for (int i = 0; i < 2; i++)
            {
                var statis = new StatisticModel();
                statis.diagnosticsCategory = new DiagnosticsCategory();
                statis.countPatient = i + 1;
                statis.diagnosticsCategory = getDiagnostic;
                statisticModels.Add(statis);
            }

            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Diagnostic"]).Returns(statisticModels); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;
            return statisticModels;
        }

        [TestMethod]
        public void TestStatisByDiagnostic()
        {
            MockSession();
            var result = controller.StatisByDiagnostic() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestSearchBSAndTT()
        {
            MockSession();
            var result = controller.SearchBSAndTT(DateTime.Now, DateTime.Now) as ViewResult;
            Assert.IsNotNull(result);

            controller.ModelState.Clear();
            var result1 = controller.SearchBSAndTT(DateTime.Now, null) as ViewResult;
            Assert.IsNotNull(result1);
        }
        [TestMethod]
        public void TestStatisByDoctorAndCondition()
        {
            MockSession();
            var result = controller.StatisByDoctorAndCondition() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestPriceCTMau()
        {
            var detail_CTMau = db.Detail_CTMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_CTMau.InformationExamination_ID);
            information.PriceCTMaus = 250000;
            var result = controller.PriceCTMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestPriceAmniocente()
        {
            var detail_Aminocente = db.Detail_Amniocente.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_Aminocente.InformationExamination_ID);
            var result = controller.PriceAmniocente(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestSearchDiagnostic()
        {
            MockSession();
            var result = controller.SearchDiagnostic(DateTime.Now, DateTime.Now) as ViewResult;
            Assert.IsNotNull(result);

            controller.ModelState.Clear();
            var result1 = controller.SearchDiagnostic(DateTime.Now, null) as ViewResult;
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void TestPriceSinhHoaMau()
        {
            var detail_SinhHoaMau = db.Detail_SinhHoaMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_SinhHoaMau.InformationExamination_ID);
            var result = controller.PriceSinhHoaMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceUrine()
        {
            var detail_Urine = db.Detail_Urine.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_Urine.InfomationExamination_ID);
            var result = controller.PriceUrine(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceViSinh()
        {
            var detail_ViSinh = db.Detail_ViSinh.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_ViSinh.InformationExamination_ID);
            var result = controller.PriceViSinh(information) as Task<int>;
            Assert.IsNotNull(result);
        }

    }
}
