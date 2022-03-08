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
    public class WardsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        WardsController controller = new WardsController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<Ward>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.Wards.Count(), model.Count);
        }

        [TestMethod]
        public void GetData()
        {
            var getData = controller.GetData() as JsonResult;
            Assert.IsNotNull(getData);
            dynamic check = getData.Data;
            Assert.AreEqual(db.Wards.Count(), check.data.Count);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var HomeTown_ID = db.HomeTowns.FirstOrDefault();
            var District_ID = db.Districts.FirstOrDefault();
            var ward = new Ward
            {
                Ward1 = rand.ToString(),
                HomeTown_ID = HomeTown_ID.ID,
                District_ID = District_ID.ID
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(ward) as JsonResult;
                Assert.IsNotNull(result);
            }

            ward.Ward1 = "Phường 1";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Create(ward) as JsonResult;
                Assert.IsNotNull(result1);

                dynamic noti = result1.Data;
                Assert.AreEqual(false, noti.success);
                Assert.AreEqual("Phường/Xã đã có trong danh sách", noti.responseText);
            }

            ward.Ward1 = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Create(ward) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var ward = db.Wards.First();
            var result1 = controller.Edit(ward.ID) as JsonResult;
            Assert.IsNotNull(result1);
            dynamic data = result1.Data;
            Assert.IsNotNull(data.data);
        }

        [TestMethod]
        public void TestEditP()
        {
            var rand = new Random();
            var ward = db.Wards.AsNoTracking().FirstOrDefault();
            ward.Ward1 = rand.ToString();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(ward) as JsonResult;
                Assert.IsNotNull(result);
                dynamic data = result.Data;
                Assert.AreEqual(true, data.success);
            }

            ward.District_ID = 7;
            ward.Ward1 = "Phường 1";
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(ward) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Phường/Xã đã có trong danh sách", data.responseText);
            }

            ward.Ward1 = null;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                controller.ModelState.AddModelError("", "Error Message");
                var result2 = controller.Edit(ward) as ViewResult;
                Assert.IsNotNull(result2);
            }
        }

        [TestMethod]
        public void TestDeleteG()
        {
            var ward = db.Wards.First();
            var result1 = controller.Delete(ward.ID) as JsonResult;
            Assert.IsNotNull(result1);

            dynamic noti = result1.Data;
            Assert.IsNotNull(noti.data);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var ward = db.Wards.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(ward.ID) as JsonResult;
                Assert.IsNotNull(result);

                dynamic noti = result.Data;
                Assert.AreEqual(false, noti.success);
            }
        }
    }
}
