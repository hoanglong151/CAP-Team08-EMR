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
    public class TamThansControllerTest
    {
            CP24Team08Entities db = new CP24Team08Entities();
            TamThansController controller = new TamThansController();
            private readonly object tamthan;

            [TestMethod]
            public void TestIndex()
            {
                var list = controller.Index() as ViewResult;
                Assert.IsNotNull(list);

                var model = list.Model as List<TamThan>;
                Assert.IsNotNull(model);

                Assert.AreEqual(db.TamThans.Count(), model.Count);
            }

            [TestMethod]
            public void GetData()
            {
                var getData = controller.GetData() as JsonResult;
                Assert.IsNotNull(getData);
                dynamic check = getData.Data;
                Assert.AreEqual(db.TamThans.Count(), check.data.Count);
            }

            [TestMethod]
            public void TestCreateP()
            {
                var rand = new Random();
                var tamthan = new TamThan
                {
                    ChiDinh = false,
                    Name = rand.ToString(),
                    Dangerous = false
                };

                using (var scope = new TransactionScope())
                {
                    var result = controller.Create(tamthan) as JsonResult;
                    Assert.IsNotNull(result);
                }

                tamthan.Name = "Tâm thần phân liệt";
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Create(tamthan) as JsonResult;
                    Assert.IsNotNull(result1);

                    dynamic noti = result1.Data;
                    Assert.AreEqual(false, noti.success);
                    Assert.AreEqual("Tâm Thần đã có trong danh sách", noti.responseText);
                }

                tamthan.Name = null;
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Create(tamthan) as ViewResult;
                    Assert.IsNotNull(result2);
                }
            }

            [TestMethod]
            public void TestEditG()
            {
                var result = controller.Edit(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var coxuongkhop = db.TamThans.First();
                var result1 = controller.Edit(coxuongkhop.ID) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.IsNotNull(data.data);
            }

            [TestMethod]
            public void TestEditP()
            {
                var rand = new Random();
                var tamthan = db.TamThans.FirstOrDefault();
                tamthan.Name = "Rối loạn ám sợ";
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Edit(tamthan) as JsonResult;
                    Assert.IsNotNull(result1);
                    dynamic data = result1.Data;
                    Assert.AreEqual(false, data.success);
                    Assert.AreEqual("Tâm Thần đã có trong danh sách", data.responseText);
                }
                tamthan.Name = rand.ToString();
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result = controller.Edit(tamthan) as JsonResult;
                    Assert.IsNotNull(result);
                    dynamic data = result.Data;
                    Assert.AreEqual(true, data.success);
                }
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Edit(tamthan) as ViewResult;
                    Assert.IsNotNull(result2);
                }
            }
            [TestMethod]
            public void TestDeleteG()
            {
                var result = controller.Delete(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var tamthan = db.TamThans.First();
                var result1 = controller.Delete(tamthan.ID) as JsonResult;
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
                var tamthan = db.TamThans.First();
                using (var scope = new TransactionScope())
                {
                    var result = controller.DeleteConfirmed(tamthan .ID) as JsonResult;
                    Assert.IsNotNull(result);

                    dynamic noti = result.Data;
                    Assert.AreEqual(true, noti.success);
                    //  Assert.AreEqual("Mũi này đã được sử dụng. Bạn không thể xóa nó!", noti.responseText);
                }
            }
        }
    }


