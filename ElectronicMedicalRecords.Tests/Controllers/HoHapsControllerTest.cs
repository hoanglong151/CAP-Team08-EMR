using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class HoHapsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        HoHapsController controller = new HoHapsController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<HoHap>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.HoHaps.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.HoHaps.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var hoHap = new HoHap
            {
                ChiDinh = false,
                Name = rand.ToString(),
                Dangerous = false
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(hoHap) as JsonResult;
                Assert.IsNotNull(result);
            }

            hoHap.Name = "Cảm cúm";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(hoHap) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Hô Hấp đã có trong danh sách", noti.responseText);
            }

            hoHap.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(hoHap) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var hoHap = db.HoHaps.First();
            var result1 = controller.Edit(hoHap.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var hoHap = db.HoHaps.FirstOrDefault();
            hoHap.Name = "Hen suyễn";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(hoHap) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Hô Hấp đã có trong danh sách", data.responseText);
            }
            hoHap.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(hoHap) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(hoHap) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var hoHap = db.HoHaps.First();
            var result1 = controller.Delete(hoHap.ID) as JsonResult;
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
    }
}
