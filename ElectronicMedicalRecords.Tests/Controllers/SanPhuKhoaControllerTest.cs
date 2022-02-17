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

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class SanPhuKhoaControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        SanPhuKhoasController controller = new SanPhuKhoasController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<SanPhuKhoa>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.SanPhuKhoas.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.SanPhuKhoas.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var spKhoa = new SanPhuKhoa
            {
                ChiDinh = false,
                Name = rand.ToString(),
                Dangerous = false
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(spKhoa) as JsonResult;
                Assert.IsNotNull(result);
            }

            spKhoa.Name = "Viêm vòi trứng";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(spKhoa) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Sản Phụ Khoa đã có trong danh sách", noti.responseText);
            }

            spKhoa.Name = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(spKhoa) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var spKhoa = db.SanPhuKhoas.First();
            var result1 = controller.Edit(spKhoa.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var spKhoa = db.SanPhuKhoas.FirstOrDefault();
            spKhoa.Name = "Viêm vòi trứng";
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(spKhoa) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Sản Phụ Khoa đã có trong danh sách", data.responseText);
            }
            spKhoa.Name = rand.ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(spKhoa) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(spKhoa) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }
        [TestMethod]
        public void TestDeleteG()
        {
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var spKhoa = db.SanPhuKhoas.First();
            var result1 = controller.Delete(spKhoa.ID) as JsonResult;
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
        //[TestMethod]
        //public void TestDeleteP()
        //{
        //    var spKhoa = db.SanPhuKhoas.AsNoTracking().First();
        //    using (var scope = new TransactionScope())
        //    {
        //        var result = controller.DeleteConfirmed(spKhoa.ID) as JsonResult;
        //        Assert.IsNotNull(result);

        //        dynamic noti = result.Data;
        //        Assert.AreEqual(false, noti.success);
        //    }
        //}
    }
}
