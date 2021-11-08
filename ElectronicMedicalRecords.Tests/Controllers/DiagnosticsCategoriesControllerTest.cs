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

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class DiagnosticsCategoriesControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        DiagnosticsCategoriesController controller = new DiagnosticsCategoriesController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<DiagnosticsCategory>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.DiagnosticsCategories.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var list = controller.GetData() as JsonResult;
            Assert.IsNotNull(list);

            IDictionary<string, object> model = new System.Web.Routing.RouteValueDictionary(list.Data);
            Assert.IsNotNull(model);
            var test = (List<DiagnosticsCategory>)model.Values.First();
            Assert.AreEqual(db.DiagnosticsCategories.Count(), test.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var diagnostic = new DiagnosticsCategory
            {
                Code = rand.ToString().Substring(0, 9),
                MDC = rand.ToString().Substring(0, 2),
                Name = rand.ToString(),
                NameEnglish = rand.ToString()
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(diagnostic) as JsonResult;
                Assert.IsNotNull(result);
            }

            diagnostic.Code = "M1199";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(diagnostic) as JsonResult;
                Assert.IsNotNull(result1);

                IDictionary<string, object> model = new System.Web.Routing.RouteValueDictionary(result1.Data);
                Assert.AreEqual(false, model.Values.First());
                Assert.AreEqual("Chẩn đoán đã có trong danh sách", model.Values.ElementAtOrDefault(1));
            }

            diagnostic.Code = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(diagnostic) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var diagnostic = db.DiagnosticsCategories.First();
            var result1 = controller.Edit(diagnostic.ID) as JsonResult;
            Assert.IsNotNull(result1);

            var model = new System.Web.Routing.RouteValueDictionary(result1.Data);
            Assert.IsNotNull(model.Values);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var diagnostic = db.DiagnosticsCategories.AsNoTracking().FirstOrDefault();
            diagnostic.Code = rand.ToString().Substring(0, 9);
            diagnostic.MDC = rand.ToString().Substring(0, 2);
            diagnostic.Name = rand.ToString();
            diagnostic.NameEnglish = rand.ToString();

            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(diagnostic) as JsonResult;
                Assert.IsNotNull(result);

                var model = new System.Web.Routing.RouteValueDictionary(result.Data);
                Assert.AreEqual(true, model.Values.First());
            }

            diagnostic.Code = "M120";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(diagnostic) as JsonResult;
                Assert.IsNotNull(result1);

                var model = new System.Web.Routing.RouteValueDictionary(result1.Data);
                Assert.AreEqual(false, model.Values.First());
                Assert.AreEqual("Chẩn đoán đã có trong danh sách", model.Values.ElementAtOrDefault(1));
            }

            diagnostic.Code = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(diagnostic) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var diagnostic = db.DiagnosticsCategories.First();
            var result1 = controller.Delete(diagnostic.ID) as JsonResult;
            Assert.IsNotNull(result1);

            var model = new System.Web.Routing.RouteValueDictionary(result1.Data);
            Assert.IsNotNull(model.Values);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var diagnostic = db.DiagnosticsCategories.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(diagnostic.ID) as JsonResult;
                Assert.IsNotNull(result);

                var model = new System.Web.Routing.RouteValueDictionary(result.Data);
                Assert.AreEqual(true, model.Values.ElementAtOrDefault(0));
            }
        }
    }
}
