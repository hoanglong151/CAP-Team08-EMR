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

        public class ThanKinhsControllerTest
        {
            CP24Team08Entities db = new CP24Team08Entities();
            ThanKinhsController controller = new ThanKinhsController();
        private object coxuongkhop;
        private readonly object thankinh;

            [TestMethod]
            public void TestIndex()
            {
                var list = controller.Index() as ViewResult;
                Assert.IsNotNull(list);

                var model = list.Model as List<ThanKinh>;
                Assert.IsNotNull(model);

                Assert.AreEqual(db.ThanKinhs.Count(), model.Count);
            }

            [TestMethod]
            public void GetData()
            {
                var getData = controller.GetData() as JsonResult;
                Assert.IsNotNull(getData);
                dynamic check = getData.Data;
                Assert.AreEqual(db.ThanKinhs.Count(), check.data.Count);
            }

            [TestMethod]
            public void TestCreateP()
            {
                var rand = new Random();
                var ThanKinh = new ThanKinh
                {
                    ChiDinh = false,
                    Name = rand.ToString(),
                    Dangerous = false
                };

                using (var scope = new TransactionScope())
                {
                    var result = controller.Create(ThanKinh) as JsonResult;
                    Assert.IsNotNull(result);
                }
                ThanKinh.Name = "Đột quỵ (tai biến mạch máu não)";
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Create(ThanKinh) as JsonResult;
                    Assert.IsNotNull(result1);

                    dynamic noti = result1.Data;
                    Assert.AreEqual(false, noti.success);
                    Assert.AreEqual("Thần Kinh đã có trong danh sách", noti.responseText);
                }

                ThanKinh.Name = null;
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Create(ThanKinh) as ViewResult;
                    Assert.IsNotNull(result2);
                }
        }
    
        [TestMethod]
            public void TestEditG()
            {
                var result = controller.Edit(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var thankinh = db.ThanKinhs.First();
                var result1 = controller.Edit(thankinh.ID) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.IsNotNull(data.data);
            }

            [TestMethod]
            public void TestEditP()
            {
                var rand = new Random();
                var thankinh = db.ThanKinhs.FirstOrDefault();
                thankinh.Name = "Bệnh Parkinson";
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Edit(thankinh) as JsonResult;
                    Assert.IsNotNull(result1);
                    dynamic data = result1.Data;
                    Assert.AreEqual(false, data.success);
                    Assert.AreEqual("Thần Kinh đã có trong danh sách", data.responseText);
                }
                thankinh.Name = rand.ToString();
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result = controller.Edit(thankinh) as JsonResult;
                    Assert.IsNotNull(result);
                    dynamic data = result.Data;
                    Assert.AreEqual(true, data.success);
                }
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Edit(thankinh) as ViewResult;
                    Assert.IsNotNull(result2);
                }
            }
            [TestMethod]
            public void TestDeleteG()
            {
                var result = controller.Delete(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var thankinh = db.ThanKinhs.First();
                var result1 = controller.Delete(thankinh.ID) as JsonResult;
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
                var thankinh = db.ThanKinhs.First();
                using (var scope = new TransactionScope())
                {
                    var result = controller.DeleteConfirmed(thankinh.ID) as JsonResult;
                    Assert.IsNotNull(result);

                    dynamic noti = result.Data;
                    Assert.AreEqual(true, noti.success);
                    //  Assert.AreEqual("Mũi này đã được sử dụng. Bạn không thể xóa nó!", noti.responseText);
                }
            }
        }
    }
