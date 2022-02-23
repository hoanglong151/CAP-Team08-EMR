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
     public class RangHamMatsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        RangHamMatsController controller = new RangHamMatsController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<RangHamMat>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.RangHamMats.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.RangHamMats.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var ranghamMat = new RangHamMat
            {
                ChiDinh = false,
                Name = rand.ToString(),
                Dangerous = false
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(ranghamMat) as JsonResult;
                Assert.IsNotNull(result);
            }

            ranghamMat.Name = "Sâu răng";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(ranghamMat) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Răng Hàm Mặt đã có trong danh sách", noti.responseText);
            }

            ranghamMat.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(ranghamMat) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var ranghamMat = db.RangHamMats.First();
            var result1 = controller.Edit(ranghamMat.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var ranghamMat = db.RangHamMats.FirstOrDefault();
            ranghamMat.Name = "Răng nhạy cảm";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(ranghamMat) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Răng Hàm Mặt đã có trong danh sách", data.responseText);
            }
            ranghamMat.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(ranghamMat) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(ranghamMat) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var ranghamMat = db.RangHamMats.First();
            var result1 = controller.Delete(ranghamMat.ID) as JsonResult;
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
