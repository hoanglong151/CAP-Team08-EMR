using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElectronicMedicalRecords.Models;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class CoXuongKhopsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        CoXuongKhopsController controller = new CoXuongKhopsController();
        private readonly object coxuongkhop;

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<CoXuongKhop>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.CoXuongKhops.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.CoXuongKhops.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var coxuongkhop = new CoXuongKhop
            {
                ChiDinh = false,
                Name = rand.ToString(),
                Dangerous = false
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(coxuongkhop) as JsonResult;
                Assert.IsNotNull(result);
            }

            coxuongkhop.Name = "Gai cột sống";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(coxuongkhop) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Cơ Xương Khớp đã có trong danh sách", noti.responseText);
            }

            coxuongkhop.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(coxuongkhop) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var coxuongkhop = db.CoXuongKhops.First();
            var result1 = controller.Edit(coxuongkhop.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var coxuongkhop = db.CoXuongKhops.FirstOrDefault();
            coxuongkhop.Name = "Thoái hóa khớp";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(coxuongkhop) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Cơ Xương Khớp đã có trong danh sách", data.responseText);
            }
            coxuongkhop.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(coxuongkhop) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(coxuongkhop) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var coxuongkhop = db.CoXuongKhops.First();
            var result1 = controller.Delete(coxuongkhop.ID) as JsonResult;
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
            var coXuongKhop = db.CoXuongKhops.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(coXuongKhop.ID) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti = result.Data;
                Assert.AreEqual(true, noti.success);
                //  Assert.AreEqual("Mũi này đã được sử dụng. Bạn không thể xóa nó!", noti.responseText);
            }
        }
    }
}


