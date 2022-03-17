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
using System.Threading.Tasks;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class MultipleModelsControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        MultipleModelsController controller = new MultipleModelsController();

        [TestMethod]
        public void Index()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PrintExaminationInfo()
        {
            MockSession();
            var result = controller.PrintExaminationInfo() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceCTMau()
        {
            var detail_CTMau = db.Detail_CTMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_CTMau.InformationExamination_ID);
            information.PriceCTMaus = 250000;
            var result = controller.PriceCTMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestPriceAmniocente()
        {
            var detail_Aminocente = db.Detail_Amniocente.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_Aminocente.InformationExamination_ID);
            var result = controller.PriceAmniocente(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceSinhHoaMau()
        {
            var detail_SinhHoaMau = db.Detail_SinhHoaMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_SinhHoaMau.InformationExamination_ID);
            var result = controller.PriceSinhHoaMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceUrine()
        {
            var detail_Urine = db.Detail_Urine.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_Urine.InfomationExamination_ID);
            var result = controller.PriceUrine(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceViSinh()
        {
            var detail_ViSinh = db.Detail_ViSinh.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_ViSinh.InformationExamination_ID);
            var result = controller.PriceViSinh(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceDongMau()
        {
            var detail_DongMau = db.Detail_DongMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_DongMau.InformationExamination_ID);
            var result = controller.PriceDongMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceImmunes()
        {
            var detail_Immune = db.Detail_Immune.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_Immune.InformationExamination_ID);
            var result = controller.PriceImmune(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestPriceNhomMau()
        {
            var detail_NhomMau = db.Detail_NhomMau.FirstOrDefault();
            var information = db.InformationExaminations.FirstOrDefault(b => b.ID == detail_NhomMau.InformationExamination_ID);
            var result = controller.PriceNhomMau(information) as Task<int>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Payment()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Payment(info.ID, 25000) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("PrintBillExamination", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void PaymentPrescription()
        {
            var info = db.InformationExaminations.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.PaymentPrescription(info.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("PrintBillExamination", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void PaymentTestSubclinical()
        {
            var info = db.InformationExaminations.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.PaymentTestSubclinical(info.ID, (int)info.PriceCTMaus) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }

        public MultiplesModel MockSession()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.First();
            var patient = db.Patients.First(p => p.ID == info.Patient_ID);
            var clinical = db.Clinicals.First(p => p.InformationExamination_ID == info.ID);
            var prescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Amniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_DMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Immune = db.Detail_Immune.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_NMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_SHM = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Urine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == info.ID).ToList();
            var detail_VS = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == info.ID).ToList();

            multiplesModel.Detail_SinhHoaMaus = detail_SHM;
            multiplesModel.Detail_Urines = detail_Urine;
            multiplesModel.Detail_ViSinhs = detail_VS;
            multiplesModel.Detail_NhomMaus = detail_NMaus;
            multiplesModel.Detail_Immunes = detail_Immune;
            multiplesModel.Detail_DongMaus = detail_DMaus;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            multiplesModel.Detail_Amniocentes = detail_Amniocente;
            multiplesModel.Clinical = clinical;
            multiplesModel.Patient = patient;
            multiplesModel.Prescription_Details = prescription;
            multiplesModel.InformationExamination = info;

            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["MultipleModels"]).Returns(multiplesModel); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;
            return multiplesModel;
        }

        public MultiplesModel MockSessionAll()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.First();
            var patient = db.Patients.First(p => p.ID == info.Patient_ID);
            var clinical = db.Clinicals.First(p => p.InformationExamination_ID == info.ID);
            var prescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Amniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_DMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Immune = db.Detail_Immune.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_NMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_SHM = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            var detail_Urine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == info.ID).ToList();
            var detail_VS = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == info.ID).ToList();
            multiplesModel.Detail_SinhHoaMaus = detail_SHM;
            multiplesModel.Detail_Urines = detail_Urine;
            multiplesModel.Detail_ViSinhs = detail_VS;
            multiplesModel.Detail_NhomMaus = detail_NMaus;
            multiplesModel.Detail_Immunes = detail_Immune;
            multiplesModel.Detail_DongMaus = detail_DMaus;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            multiplesModel.Detail_Amniocentes = detail_Amniocente;
            multiplesModel.Clinical = clinical;
            multiplesModel.Patient = patient;
            multiplesModel.Prescription_Details = prescription;
            multiplesModel.InformationExamination = info;

            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["MultipleModelsAll"]).Returns(multiplesModel); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockControllerContext.Object;
            return multiplesModel;
        }

        [TestMethod]
        public void PrintExaminationInfoPost()
        {
            var multiple = MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintExaminationInfoPost(multiple) as JsonResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintAllTestInfo()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintAllTestInfo() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestCTMaus()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestCTMaus() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestSinhHoaMaus()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestSinhHoaMaus() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestDongMaus()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestDongMaus() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestNhomMaus()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestNhomMaus() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestNuocTieus()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestNuocTieus() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestMienDichs()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestMienDichs() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestDichChocDos()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestDichChocDos() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintTestViSinhs()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintTestViSinhs() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintPrescriptions()
        {
            MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintPrescriptions() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintAllExaminationInfoPost()
        {
            var multiple = MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintAllExaminationInfoPost(multiple) as JsonResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultCTMau()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultCTMau() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultSHMau()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultSHMau() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultDongMau()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultDongMau() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultNhomMau()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultNhomMau() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultNuocTieu()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultNuocTieu() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultMienDich()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultMienDich() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultDichChocDo()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultDichChocDo() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void PrintResultViSinh()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintResultViSinh() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void ConvertQRCode()
        {
            var multiple = MockSessionAll();
            using (var scope = new TransactionScope())
            {
                controller.ConvertQRCode(multiple);
            }
        }

        [TestMethod]
        public void PrintAllExaminationInfo()
        {
            MockSessionAll();
            using (var scope = new TransactionScope())
            {
                var result = controller.PrintAllExaminationInfo() as ViewResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void LoadDetailBloods()
        {
            int[] arr = new int[] { 1, 2, 4 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailBloods(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailBloods(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailUrines()
        {
            int[] arr = new int[] { 1, 2, 4 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailUrines(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailUrines(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailImmune()
        {
            int[] arr = new int[] { 1, 2, 4 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailImmune(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailImmune(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailAmniocente()
        {
            int[] arr = new int[] { 1, 2, 3 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailAmniocente(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailAmniocente(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailSHM()
        {
            int[] arr = new int[] { 1, 4, 3 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailSHM(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailSHM(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailDM()
        {
            int[] arr = new int[] { 1, 4, 3 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailDM(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailDM(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailNM()
        {
            int[] arr = new int[] { 1, 2 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailNM(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailNM(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void LoadDetailVS()
        {
            int[] arr = new int[] { 1, 2, 8 };
            using (var scope = new TransactionScope())
            {
                var result = controller.LoadDetailVS(arr) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            arr = null;
            using (var scope = new TransactionScope())
            {
                var resultError = controller.LoadDetailVS(arr) as JsonResult;
                Assert.IsNotNull(resultError);
            }
        }

        [TestMethod]
        public void checkExistPatient()
        {
            var patient = db.Patients.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.checkExistPatient(patient) as JsonResult;
                dynamic data = result.Data;
                Assert.AreEqual(false, data.success);
                Assert.AreEqual("Bệnh Nhân Đã Tồn Tại. Vui Lòng Kiểm Tra Lại", data.responseText);
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            var rand = new Random();
            var patientNew = new Patient
            {
                Address = rand.ToString(),
                BirthDate = DateTime.Now,
                Gender_ID = 1,
                HomeTown_ID = 1,
                InsuranceCode = rand.ToString(),
                Name = rand.ToString(),
                Nation1_ID = 1,
                Nation_ID = 1,
                Phone = 0987654321,
            };
            using (var scope = new TransactionScope())
            {
                var resultNew = controller.checkExistPatient(patientNew) as JsonResult;
                dynamic data = resultNew.Data;
                Assert.AreEqual(true, data.success);
                Assert.IsNotNull(resultNew);
            }
        }

        [TestMethod]
        public void CreateG()
        {
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateP()
        {
            var multiple = MockSession();
            using (var scope = new TransactionScope())
            {
                string[] chuyenkhoa = new string[2];
                var result = controller.Create(multiple, chuyenkhoa) as Task<RedirectToRouteResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void CreateOldPatientG()
        {
            var info = db.InformationExaminations.First();
            var result = controller.CreateOldPatient(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BillExamination()
        {
            var info = db.InformationExaminations.First();
            var result = controller.BillExamination(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateOldPatient()
        {
            var multiple = MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(multiple) as Task<RedirectToRouteResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var info = db.InformationExaminations.First();
            var result = controller.CreateTest(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateTestP()
        {
            var multiple = MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateTest(multiple) as Task<RedirectToRouteResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void EditG()
        {
            var info = db.InformationExaminations.First();
            var result = controller.Edit(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditByIDG()
        {
            var info = db.InformationExaminations.First();
            var result = controller.EditByID(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DetailsIEG()
        {
            var info = db.InformationExaminations.First();
            var result = controller.DetailsIE(info.ID) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditP()
        {
            var multiple = MockSession();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(multiple) as Task<RedirectToRouteResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void DeleteConfirmed()
        {
            var info = db.InformationExaminations.First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(info.ID) as JsonResult;
                Assert.IsNotNull(result);
            }
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var resultError = controller.DeleteConfirmed(0) as JsonResult;
                dynamic noti = resultError.Data;
                Assert.AreEqual(false, noti.success);
                Assert.IsNotNull(resultError);
            }
        }
    }
}
