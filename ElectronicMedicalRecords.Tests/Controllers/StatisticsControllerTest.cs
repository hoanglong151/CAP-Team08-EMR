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

            var detail_diagnostic = db.Detail_DiagnosticsCategory.FirstOrDefault();
            var getDiagnostic = db.DiagnosticsCategories.AsNoTracking().FirstOrDefault(p => p.ID == detail_diagnostic.DiagnosticsCategory_ID);
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
        public void TestSearchMoney()
        {
            MockSession();
            var result = controller.SearchMoney(DateTime.Now, DateTime.Now) as Task<ActionResult>;
            Assert.IsNotNull(result);

            controller.ModelState.Clear();
            var result1 = controller.SearchMoney(DateTime.Now, null) as Task<ActionResult>;
            Assert.IsNotNull(result1);
            var result2 = controller.SearchMoney(null, DateTime.Now) as Task<ActionResult>;
            Assert.IsNotNull(result2);
            var result3 = controller.SearchMoney(null, null) as Task<ActionResult>;
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void TestStatisByMoney()
        {
            MockSession();
            var result = controller.StatisByMoney() as Task<ActionResult>;
            Assert.IsNotNull(result);
        }
    }
}
