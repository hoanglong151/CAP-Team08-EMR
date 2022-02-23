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
    public class NgoaiKhoasControllerTest
    {
            CP24Team08Entities db = new CP24Team08Entities();
            NgoaiKhoasController controller = new NgoaiKhoasController();
            private readonly object ngoaikhoa;

            [TestMethod]
            public void TestIndex()
            {
                var list = controller.Index() as ViewResult;
                Assert.IsNotNull(list);

                var model = list.Model as List<NgoaiKhoa>;
                Assert.IsNotNull(model);

                Assert.AreEqual(db.NgoaiKhoas.Count(), model.Count);
            }

            [TestMethod]
            public void GetData()
            {
                var getData = controller.GetData() as JsonResult;
                Assert.IsNotNull(getData);
                dynamic check = getData.Data;
                Assert.AreEqual(db.NgoaiKhoas.Count(), check.data.Count);
            }

            [TestMethod]
            public void TestCreateP()
            {
                var rand = new Random();
                var ngoaikhoa = new NgoaiKhoa
                {
                    ChiDinh = false,
                    Name = rand.ToString(),
                    Dangerous = false
                };

                using (var scope = new TransactionScope())
                {
                    var result = controller.Create(ngoaikhoa) as JsonResult;
                    Assert.IsNotNull(result);
                }

                ngoaikhoa.Name = "Gãy cổ xương đùi";
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Create(ngoaikhoa) as JsonResult;
                    Assert.IsNotNull(result1);

                    dynamic noti = result1.Data;
                    Assert.AreEqual(false, noti.success);
                    Assert.AreEqual("Ngoại Khoa đã có trong danh sách", noti.responseText);
                }

                ngoaikhoa.Name = null;
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Create(ngoaikhoa) as ViewResult;
                    Assert.IsNotNull(result2);
                }
            }

            [TestMethod]
            public void TestEditG()
            {
                var result = controller.Edit(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var ngoaikhoa = db.NgoaiKhoas.First();
                var result1 = controller.Edit(ngoaikhoa.ID) as JsonResult;
                Assert.IsNotNull(result1);
                dynamic data = result1.Data;
                Assert.IsNotNull(data.data);
            }

            [TestMethod]
            public void TestEditP()
            {
                var rand = new Random();
                var ngoaikhoa = db.NgoaiKhoas.FirstOrDefault();
                ngoaikhoa.Name = "Sỏi mật";
                using (var scope = new TransactionScope())
                {
                    var result1 = controller.Edit(ngoaikhoa) as JsonResult;
                    Assert.IsNotNull(result1);
                    dynamic data = result1.Data;
                    Assert.AreEqual(false, data.success);
                    Assert.AreEqual("Ngoại Khoa đã có trong danh sách", data.responseText);
                }
                ngoaikhoa.Name = rand.ToString();
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    var result = controller.Edit(ngoaikhoa) as JsonResult;
                    Assert.IsNotNull(result);
                    dynamic data = result.Data;
                    Assert.AreEqual(true, data.success);
                }
                controller.ModelState.Clear();
                using (var scope = new TransactionScope())
                {
                    controller.ModelState.AddModelError("", "Error Message");
                    var result2 = controller.Edit(ngoaikhoa) as ViewResult;
                    Assert.IsNotNull(result2);
                }
            }
            [TestMethod]
            public void TestDeleteG()
            {
                var result = controller.Delete(0) as HttpNotFoundResult;
                Assert.IsNotNull(result);

                var ngoaikhoa = db.NgoaiKhoas.First();
                var result1 = controller.Delete(ngoaikhoa.ID) as JsonResult;
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
                var ngoaikhoa = db.NgoaiKhoas.First();
                using (var scope = new TransactionScope())
                {
                    var result = controller.DeleteConfirmed(ngoaikhoa.ID) as JsonResult;
                    Assert.IsNotNull(result);

                    dynamic noti = result.Data;
                    Assert.AreEqual(true, noti.success);
                    //  Assert.AreEqual("Mũi này đã được sử dụng. Bạn không thể xóa nó!", noti.responseText);
                }
            }
        }
    }
