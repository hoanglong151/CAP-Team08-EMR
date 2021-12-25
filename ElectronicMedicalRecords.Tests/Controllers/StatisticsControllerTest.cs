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
    }
}
