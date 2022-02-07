using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class MultipleModelsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/MultipleModels
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailForm(MultiplesModel multiplesModel)
        {
            int count = 0;
            int success = 0;
            db.Entry(multiplesModel.Patient).State = EntityState.Modified;
            db.SaveChanges();
            if (multiplesModel.Detail_CTMaus != null)
            {
                count += 1;
                if(multiplesModel.Detail_CTMaus.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_SinhHoaMaus != null)
            {
                count += 1;
                if(multiplesModel.Detail_SinhHoaMaus.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_DongMaus != null)
            {
                count += 1;
                if(multiplesModel.Detail_DongMaus.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_NhomMaus != null)
            {
                count += 1;
                if (multiplesModel.Detail_NhomMaus.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_Urines != null)
            {
                count += 1;
                if (multiplesModel.Detail_Urines.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_Immunes != null)
            {
                count += 1;
                if (multiplesModel.Detail_Immunes.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_Amniocentes != null)
            {
                count += 1;
                if (multiplesModel.Detail_Amniocentes.All(p => p.Result != null))
                {
                    success += 1;
                }
            }
            if (multiplesModel.Detail_ViSinhs != null)
            {
                count += 1;
                List<Detail_ViSinh> detail_ViSinhsAD = multiplesModel.Detail_ViSinhs.Where(p => p.Result != null).ToList();
                List<Detail_ViSinh> detail_ViSinhsRS = multiplesModel.Detail_ViSinhs.Where(p => p.ResultNC != null && p.ResultDD != null && p.MatDo != null).ToList();
                if((detail_ViSinhsAD.Count + detail_ViSinhsRS.Count) == multiplesModel.Detail_ViSinhs.Count)
                {
                    success += 1;
                }
            }
            if(count == success)
            {
                multiplesModel.InformationExamination.ResultCTMau = null;
                multiplesModel.InformationExamination.ResultSHM = null;
                multiplesModel.InformationExamination.ResultDMau = null;
                multiplesModel.InformationExamination.ResultNhomMau = null;
                multiplesModel.InformationExamination.ResultNuocTieu = null;
                multiplesModel.InformationExamination.ResultMienDich = null;
                multiplesModel.InformationExamination.ResultDichChocDo = null;
                multiplesModel.InformationExamination.ResultViSinh = null;
            }
            var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).ToList();
            if (multiplesModel.Prescription_Details != null)
            {
                foreach (var item1 in listPrescription)
                {
                    db.Prescription_Detail.Remove(item1);
                    db.SaveChanges();
                }
                foreach (var prescription_Detail in multiplesModel.Prescription_Details)
                {
                    prescription_Detail.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Prescription_Detail.Add(prescription_Detail);
                    db.SaveChanges();
                }
            }
            else
            {
                foreach (var item1 in listPrescription)
                {
                    db.Prescription_Detail.Remove(item1);
                    db.SaveChanges();
                }
            }
            var checkExistDiagnostic = db.Detail_DiagnosticsCategory.AsNoTracking().FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
            if(checkExistDiagnostic == null)
            {
                Detail_DiagnosticsCategory detail_DiagnosticsCategory = new Detail_DiagnosticsCategory();
                detail_DiagnosticsCategory.DiagnosticsCategory_ID = multiplesModel.Detail_DiagnosticsCategory.DiagnosticsCategory_ID;
                detail_DiagnosticsCategory.Advice = multiplesModel.Detail_DiagnosticsCategory.Advice;
                detail_DiagnosticsCategory.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                db.Detail_DiagnosticsCategory.Add(detail_DiagnosticsCategory);
                db.SaveChanges();
            }
            else
            {
                db.Entry(multiplesModel.Detail_DiagnosticsCategory).State = EntityState.Modified;
                db.SaveChanges();
            }

            //Clinical
            Detail_TuanHoanController detail_TuanHoanController = new Detail_TuanHoanController();
            Detail_HoHapController detail_HoHapController = new Detail_HoHapController();
            Detail_TieuHoaController detail_TieuHoaController = new Detail_TieuHoaController();
            Detail_ThanTietNieuController detail_ThanTietNieuController = new Detail_ThanTietNieuController();
            Detail_CoXuongKhopController detail_CoXuongKhopController = new Detail_CoXuongKhopController();
            Detail_ThanKinhController detail_ThanKinhController = new Detail_ThanKinhController();
            Detail_TamThanController detail_TamThanController = new Detail_TamThanController();
            Detail_NgoaiKhoaController detail_NgoaiKhoaController = new Detail_NgoaiKhoaController();
            Detail_SanPhuKhoaController detail_SanPhuKhoaController = new Detail_SanPhuKhoaController();
            Detail_MatController detail_MatController = new Detail_MatController();
            Detail_TaiController detail_TaiController = new Detail_TaiController();
            Detail_MuiController detail_MuiController = new Detail_MuiController();
            Detail_HongController detail_HongController = new Detail_HongController();
            Detail_RangHamMatController detail_RangHamMatController = new Detail_RangHamMatController();
            Detail_DaLieuController detail_DaLieuController = new Detail_DaLieuController();

            // Set Up Clinical
            multiplesModel.TuanHoan = multiplesModel.TuanHoan.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HoHap = multiplesModel.HoHap.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.TieuHoa = multiplesModel.TieuHoa.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.ThanTietNieu = multiplesModel.ThanTietNieu.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.CoXuongKhop = multiplesModel.CoXuongKhop.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.TamThan = multiplesModel.TamThan.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.ThanKinh = multiplesModel.ThanKinh.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.NgoaiKhoa = multiplesModel.NgoaiKhoa.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.SanPhuKhoa = multiplesModel.SanPhuKhoa.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Mat = multiplesModel.Mat.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Tai = multiplesModel.Tai.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Mui = multiplesModel.Mui.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Hong = multiplesModel.Hong.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.RangHamMat = multiplesModel.RangHamMat.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.DaLieu = multiplesModel.DaLieu.Where(p => p.ChiDinh == true).ToList();

            //Clinical
            detail_TuanHoanController.CreateOldPatient(multiplesModel);
            detail_HoHapController.CreateOldPatient(multiplesModel);
            detail_TieuHoaController.CreateOldPatient(multiplesModel);
            detail_ThanTietNieuController.CreateOldPatient(multiplesModel);
            detail_CoXuongKhopController.CreateOldPatient(multiplesModel);
            detail_ThanKinhController.CreateOldPatient(multiplesModel);
            detail_TamThanController.CreateOldPatient(multiplesModel);
            detail_NgoaiKhoaController.CreateOldPatient(multiplesModel);
            detail_SanPhuKhoaController.CreateOldPatient(multiplesModel);
            detail_MatController.CreateOldPatient(multiplesModel);
            detail_TaiController.CreateOldPatient(multiplesModel);
            detail_MuiController.CreateOldPatient(multiplesModel);
            detail_HongController.CreateOldPatient(multiplesModel);
            detail_RangHamMatController.CreateOldPatient(multiplesModel);
            detail_DaLieuController.CreateOldPatient(multiplesModel);

            db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Patients");
        }

        public ActionResult PrintExaminationInfo()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            return View(multiplesModel);
        }

        [HttpPost]
        public ActionResult Payment(int id, int price)
        {
            var examinationBill = db.InformationExaminations.Find(id);
            if(examinationBill.New == null && examinationBill.PriceExamination == null)
            {
                examinationBill.New = false;
                examinationBill.PriceExamination = price;
                db.Entry(examinationBill).State = EntityState.Modified;
                db.SaveChanges();
                var bill = new Bill();
                bill.InformationExamination_ID = examinationBill.ID;
                bill.Patient_ID = examinationBill.Patient_ID;
                bill.Date = DateTime.Now;
                bill.TypePayment = "Khám";
                bill.UserPayment_ID = User.Identity.GetUserId();
                db.Bills.Add(bill);
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, responeText = "Hóa Đơn Này Đã Được Thanh Toán" });
            }
        }
        [HttpPost]
        public ActionResult PaymentPrescription(int id)
        {
            int error = 0;
            int totalPrice = 0;
            var prescriptionsBill = db.Prescription_Detail.Where(p => p.InformationExamination_ID == id).ToList();
            foreach(var bill in prescriptionsBill)
            {
                if(bill.TotalPrice == null)
                {
                    bill.TotalPrice = bill.NumMedication * bill.Medication.Price;
                    db.Entry(bill).State = EntityState.Modified;
                    db.SaveChanges();
                    totalPrice += (int)bill.TotalPrice;
                }
                else
                {
                    error += 1;
                }
            }
            if(error == prescriptionsBill.Count)
            {
                return Json(new { success = false, responeText = "Hóa Đơn Này Đã Được Thanh Toán" });
            }
            else
            {
                var info = db.InformationExaminations.Find(id);
                var checkid = db.Bills.FirstOrDefault(p => p.InformationExamination_ID == info.ID && p.TypePayment == "Thuốc");
                if(checkid == null)
                {
                    var patient = db.Patients.FirstOrDefault(p => p.ID == info.Patient_ID);
                    var billPre = new Bill();
                    billPre.InformationExamination_ID = info.ID;
                    billPre.Patient_ID = patient.ID;
                    billPre.Date = DateTime.Now;
                    billPre.TypePayment = "Thuốc";
                    billPre.UserPayment_ID = User.Identity.GetUserId();
                    db.Bills.Add(billPre);
                    db.SaveChanges();
                }
                else
                {
                    checkid.Date = DateTime.Now;
                    checkid.UserPayment_ID = User.Identity.GetUserId();
                    db.Entry(checkid).State = EntityState.Modified;
                    db.SaveChanges();
                }
                info.PricePrescription = totalPrice;
                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        public async Task<int> PriceCTMau(InformationExamination informationExamination)
        {
            int priceCTMaus = 0;
            var priceCTMau = db.Detail_CTMau.FirstOrDefault(p => p.InformationExamination_ID == informationExamination.ID);
            if (informationExamination.PriceCTMaus != null && priceCTMau != null)
            {
                priceCTMaus += (int)informationExamination.PriceCTMaus;
            }
            return priceCTMaus;
        }

        public async Task<int> PriceAmniocente(InformationExamination informationExamination)
        {
            int priceAmniocentes = 0;
            var priceAmniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceAmniocente != null)
            {
                foreach (var priceAmnio in priceAmniocente)
                {
                    priceAmnio.Amniocente = db.Amniocentes.Find(priceAmnio.Amniocente_ID);
                    priceAmniocentes += priceAmnio.Amniocente.Price;
                }
            }
            return priceAmniocentes;
        }

        public async Task<int> PriceDongMau(InformationExamination informationExamination)
        {
            int priceDongMaus = 0;
            var priceDongMau = db.Detail_DongMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceDongMau != null)
            {
                foreach (var priceDM in priceDongMau)
                {
                    priceDM.DongMau = db.DongMaus.Find(priceDM.DongMau_ID);
                    priceDongMaus += priceDM.DongMau.Price;
                }
            }
            return priceDongMaus;
        }

        public async Task<int> PriceImmune(InformationExamination informationExamination)
        {
            int priceImmunes = 0;
            var priceImmune = db.Detail_Immune.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceImmune != null)
            {
                foreach (var priceIm in priceImmune)
                {
                    priceIm.Immune = db.Immunes.Find(priceIm.Immue_ID);
                    priceImmunes += priceIm.Immune.Price;
                }
            }
            return priceImmunes;
        }

        public async Task<int> PriceNhomMau(InformationExamination informationExamination)
        {
            int priceNhomMaus = 0;
            var priceNhomMau = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceNhomMau != null)
            {
                foreach (var priceNM in priceNhomMau)
                {
                    priceNM.NhomMau = db.NhomMaus.Find(priceNM.NhomMau_ID);
                    priceNhomMaus += priceNM.NhomMau.Price;
                }
            }
            return priceNhomMaus;
        }

        public async Task<int> PriceSinhHoaMau(InformationExamination informationExamination)
        {
            int priceSHMaus = 0;
            var priceSHMau = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceSHMau != null)
            {
                foreach (var priceSHM in priceSHMau)
                {
                    priceSHM.SinhHoaMau = db.SinhHoaMaus.Find(priceSHM.SinhHoaMau_ID);
                    priceSHMaus += priceSHM.SinhHoaMau.Price;
                }
            }
            return priceSHMaus;
        }

        public async Task<int> PriceUrine(InformationExamination informationExamination)
        {
            int priceUrines = 0;
            var priceUrine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == informationExamination.ID).ToList();
            if (priceUrine != null)
            {
                foreach (var priceUr in priceUrine)
                {
                    priceUr.Urine = db.Urines.Find(priceUr.Urine_ID);
                    priceUrines += priceUr.Urine.Price;
                }
            }
            return priceUrines;
        }

        public async Task<int> PriceViSinh(InformationExamination informationExamination)
        {
            int priceViSinhs = 0;
            var priceVSinh = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceVSinh != null)
            {
                foreach (var priceVS in priceVSinh)
                {
                    priceVS.ViSinh = db.ViSinhs.Find(priceVS.ViSinh_ID);
                    priceViSinhs += priceVS.ViSinh.Price;
                }
            }
            return priceViSinhs;
        }

        [HttpPost]
        public async Task<ActionResult> PaymentTestSubclinical(int id, int? price)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var examinationBill = db.InformationExaminations.Find(id);
            if(examinationBill.ResultCTMau != null || examinationBill.ResultSHM != null || examinationBill.ResultDMau != null || examinationBill.ResultNhomMau != null
                || examinationBill.ResultNuocTieu != null || examinationBill.ResultMienDich != null || examinationBill.ResultDichChocDo != null || examinationBill.ResultViSinh != null)
            {
                return Json(new { success = false, responeText = "Hóa Đơn Này Đã Được Thanh Toán" });
            }
            else
            {
                examinationBill.PriceCTMaus = price;
                var checkCongThucMauCD = db.Detail_CTMau.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkCongThucMauCD != null)
                {
                    examinationBill.ResultCTMau = false;
                }
                var checkSinhHoaMauCD = db.Detail_SinhHoaMau.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkSinhHoaMauCD != null)
                {
                    examinationBill.ResultSHM = false;
                }
                var checkDongMauCD = db.Detail_DongMau.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkDongMauCD != null)
                {
                    examinationBill.ResultDMau = false;
                }
                var checkNhomMauCD = db.Detail_NhomMau.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkNhomMauCD != null)
                {
                    examinationBill.ResultNhomMau = false;
                }
                var checkUrineCD = db.Detail_Urine.FirstOrDefault(p => p.InfomationExamination_ID == examinationBill.ID);
                if (checkUrineCD != null)
                {
                    examinationBill.ResultNuocTieu = false;
                }
                var checkImmuneCD = db.Detail_Immune.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkImmuneCD != null)
                {
                    examinationBill.ResultMienDich = false;
                }
                var checkAmniocenteCD = db.Detail_Amniocente.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkAmniocenteCD != null)
                {
                    examinationBill.ResultDichChocDo = false;
                }
                var checkViSinhCD = db.Detail_ViSinh.FirstOrDefault(p => p.InformationExamination_ID == examinationBill.ID);
                if (checkViSinhCD != null)
                {
                    examinationBill.ResultViSinh = false;
                }
                var priceTotalTest = 0;
                var priceCTMau = PriceCTMau(examinationBill);
                var priceAmniocente = PriceAmniocente(examinationBill);
                var priceDongMau = PriceDongMau(examinationBill);
                var priceImmune = PriceImmune(examinationBill);
                var priceNhomMau = PriceNhomMau(examinationBill);
                var priceSinhHoaMau = PriceSinhHoaMau(examinationBill);
                var priceUrine = PriceUrine(examinationBill);
                var priceViSinh = PriceViSinh(examinationBill);
                var result = await Task.WhenAll(priceCTMau, priceAmniocente
                    , priceDongMau, priceImmune, priceNhomMau, priceSinhHoaMau
                    , priceUrine, priceViSinh);
                foreach (var priceTest in result)
                {
                    priceTotalTest += priceTest;
                }
                examinationBill.PriceTest = priceTotalTest;
                db.Entry(examinationBill).State = EntityState.Modified;
                db.SaveChanges();
                var bill = new Bill();
                bill.Date = DateTime.Now;
                bill.InformationExamination_ID = examinationBill.ID;
                bill.Patient_ID = examinationBill.Patient_ID;
                bill.TypePayment = "Xét Nghiệm";
                bill.UserPayment_ID = User.Identity.GetUserId();
                db.Bills.Add(bill);
                db.SaveChanges();
                return Json(new { success = true });
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PrintExaminationInfoPost(MultiplesModel multiplesModel)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Session["MultipleModels"] = multiplesModel;
            return Json(new { success = true, data = multiplesModel });
        }

        public ActionResult PrintAllTestInfo()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            multiplesModel.CTMau = multiplesModel.CTMau.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.SinhHoaMau = multiplesModel.SinhHoaMau.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.DongMau = multiplesModel.DongMau.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.NhomMau = multiplesModel.NhomMau.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Urine = multiplesModel.Urine.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Immune = multiplesModel.Immune.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Amniocente = multiplesModel.Amniocente.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.ViSinh = multiplesModel.ViSinh.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestCTMaus()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.CTMau = multiplesModel.CTMau.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestSinhHoaMaus()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.SinhHoaMau = multiplesModel.SinhHoaMau.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestDongMaus()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.DongMau = multiplesModel.DongMau.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestNhomMaus()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.NhomMau = multiplesModel.NhomMau.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestNuocTieus()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID); multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.Urine = multiplesModel.Urine.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestMienDichs()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID); multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.Immune = multiplesModel.Immune.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestDichChocDos()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.Amniocente = multiplesModel.Amniocente.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintTestViSinhs()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            multiplesModel.ViSinh = multiplesModel.ViSinh.Where(p => p.ChiDinh == true).ToList();
            return View(multiplesModel);
        }

        public ActionResult PrintPrescriptions()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            multiplesModel.Detail_DiagnosticsCategory = db.Detail_DiagnosticsCategory.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
            if(multiplesModel.Detail_DiagnosticsCategory != null)
            {
                multiplesModel.Detail_DiagnosticsCategory.DiagnosticsCategory = db.DiagnosticsCategories.FirstOrDefault(p => p.ID == multiplesModel.Detail_DiagnosticsCategory.DiagnosticsCategory_ID);
            }
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            if(multiplesModel.HistoryDiseases1 != null)
            {
                multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
            }
            if(multiplesModel.HistoryDiseases2 != null)
            {
                multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
            }
            if(multiplesModel.HistoryDiseases3 != null)
            {
                multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
            }
            if(multiplesModel.MedicalHistories != null)
            {
                multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
            }
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            //ViewBag.Diagnostic = diagnostic.Name;
            if(multiplesModel.Prescription_Details != null)
            {
                foreach (var item in multiplesModel.Prescription_Details)
                {
                    item.Medication = db.Medications.Find(item.Medication_ID);
                }

            }
            return View(multiplesModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PrintAllExaminationInfoPost(MultiplesModel multiplesModel)
        {
            db.Configuration.LazyLoadingEnabled = false;
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            Session["MultipleModelsAll"] = multiplesModel;
            return Json(new { success = true, data = multiplesModel });
        }

        public ActionResult PrintResultCTMau()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_CTMaus != null)
            {
                foreach (var ctmau in multiplesModel.Detail_CTMaus)
                {
                    ctmau.CTMau = db.CTMaus.Find(ctmau.CTMau_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultSHMau()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_SinhHoaMaus != null)
            {
                foreach (var shoamau in multiplesModel.Detail_SinhHoaMaus)
                {
                    shoamau.SinhHoaMau = db.SinhHoaMaus.Find(shoamau.SinhHoaMau_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultDongMau()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_DongMaus != null)
            {
                foreach (var dmau in multiplesModel.Detail_DongMaus)
                {
                    dmau.DongMau = db.DongMaus.Find(dmau.DongMau_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultNhomMau()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_NhomMaus != null)
            {
                foreach (var nmau in multiplesModel.Detail_NhomMaus)
                {
                    nmau.NhomMau = db.NhomMaus.Find(nmau.NhomMau_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultNuocTieu()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_Urines != null)
            {
                foreach (var urine in multiplesModel.Detail_Urines)
                {
                    urine.Urine = db.Urines.Find(urine.Urine_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultMienDich()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_Immunes != null)
            {
                foreach (var immune in multiplesModel.Detail_Immunes)
                {
                    immune.Immune = db.Immunes.Find(immune.Immue_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultDichChocDo()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            if (multiplesModel.Detail_Amniocentes != null)
            {
                foreach (var amniocente in multiplesModel.Detail_Amniocentes)
                {
                    amniocente.Amniocente = db.Amniocentes.Find(amniocente.Amniocente_ID);
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintResultViSinh()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            List<object> VSDuongAm = new List<object>();
            List<object> VSResult = new List<object>();
            if (multiplesModel.Detail_ViSinhs != null)
            {
                foreach (var visinh in multiplesModel.Detail_ViSinhs)
                {
                    visinh.ViSinh = db.ViSinhs.Find(visinh.ViSinh_ID);
                    switch (visinh.ViSinh_ID)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 39:
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                            VSDuongAm.Add(visinh);
                            break;
                        default:
                            VSResult.Add(visinh);
                            break;
                    }
                }
            }
            ViewBag.VSAmDuong = VSDuongAm;
            ViewBag.VSResult = VSResult;
            return View(multiplesModel);
        }

        public void ConvertQRCode(MultiplesModel multiplesModel)
        {
            QRCodeGenerator ObjQr = new QRCodeGenerator();
            QRCodeData qrCodeData = ObjQr.CreateQrCode(multiplesModel.Patient.MaBN, QRCodeGenerator.ECCLevel.Q);
            Bitmap bitMap = new QRCode(qrCodeData).GetGraphic(20);
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                ViewBag.Url = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
        }

        public ActionResult PrintAllExaminationInfo()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsAll"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            ConvertQRCode(multiplesModel);
            List<object> VSDuongAm = new List<object>();
            List<object> VSResult = new List<object>();
            if (multiplesModel.Detail_Amniocentes != null)
            {
                foreach(var amnio in multiplesModel.Detail_Amniocentes)
                {
                    amnio.Amniocente = db.Amniocentes.Find(amnio.Amniocente_ID);
                }
            }
            if (multiplesModel.Detail_CTMaus != null)
            {
                foreach (var ctmau in multiplesModel.Detail_CTMaus)
                {
                    ctmau.CTMau = db.CTMaus.Find(ctmau.CTMau_ID);
                }
            }
            if (multiplesModel.Detail_DongMaus != null)
            {
                foreach (var dmau in multiplesModel.Detail_DongMaus)
                {
                    dmau.DongMau = db.DongMaus.Find(dmau.DongMau_ID);
                }
            }
            if (multiplesModel.Detail_Immunes != null)
            {
                foreach (var immune in multiplesModel.Detail_Immunes)
                {
                    immune.Immune = db.Immunes.Find(immune.Immue_ID);
                }
            }
            if (multiplesModel.Detail_NhomMaus != null)
            {
                foreach (var nmau in multiplesModel.Detail_NhomMaus)
                {
                    nmau.NhomMau = db.NhomMaus.Find(nmau.NhomMau_ID);
                }
            }
            if (multiplesModel.Detail_SinhHoaMaus != null)
            {
                foreach (var shoamau in multiplesModel.Detail_SinhHoaMaus)
                {
                    shoamau.SinhHoaMau = db.SinhHoaMaus.Find(shoamau.SinhHoaMau_ID);
                }
            }
            if (multiplesModel.Detail_Urines != null)
            {
                foreach (var urien in multiplesModel.Detail_Urines)
                {
                    urien.Urine = db.Urines.Find(urien.Urine_ID);
                }
            }
            if (multiplesModel.Detail_ViSinhs != null)
            {
                foreach (var visinh in multiplesModel.Detail_ViSinhs)
                {
                    visinh.ViSinh = db.ViSinhs.Find(visinh.ViSinh_ID);
                    switch (visinh.ViSinh_ID)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 39:
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                            VSDuongAm.Add(visinh);
                            break;
                        default:
                            VSResult.Add(visinh);
                            break;
                    }
                }
            }
            ViewBag.VSAmDuong = VSDuongAm;
            ViewBag.VSResult = VSResult;
            return View(multiplesModel);
        }

        // GET: Admin/MultipleModels/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult LoadDetailBloods(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<CTMau> listNewBloods = new List<CTMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var blood = db.CTMaus.FirstOrDefault(p => p.ID == id);
                    blood.ChiDinh = true;
                    listNewBloods.Add(blood);
                }
                return Json(new { data = listNewBloods }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailUrines(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Urine> listNewUrines = new List<Urine>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var urine = db.Urines.FirstOrDefault(p => p.ID == id);
                    urine.ChiDinh = true;
                    listNewUrines.Add(urine);
                }
                return Json(new { data = listNewUrines }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailImmune(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Immune> listNewImmunes = new List<Immune>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var immune = db.Immunes.FirstOrDefault(p => p.ID == id);
                    immune.ChiDinh = true;
                    listNewImmunes.Add(immune);
                }
                return Json(new { data = listNewImmunes }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailAmniocente(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Amniocente> aMniocentes = new List<Amniocente>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var amniocente = db.Amniocentes.FirstOrDefault(p => p.ID == id);
                    amniocente.ChiDinh = true;
                    aMniocentes.Add(amniocente);
                }
                return Json(new { data = aMniocentes }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailSHM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<SinhHoaMau> sinhHoaMaus = new List<SinhHoaMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var SHM = db.SinhHoaMaus.FirstOrDefault(p => p.ID == id);
                    SHM.ChiDinh = true;
                    sinhHoaMaus.Add(SHM);
                }
                return Json(new { data = sinhHoaMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailDM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<DongMau> dongMaus = new List<DongMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var DM = db.DongMaus.FirstOrDefault(p => p.ID == id);
                    DM.ChiDinh = true;
                    dongMaus.Add(DM);
                }
                return Json(new { data = dongMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailNM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<NhomMau> nhomMaus = new List<NhomMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var NM = db.NhomMaus.FirstOrDefault(p => p.ID == id);
                    NM.ChiDinh = true;
                    nhomMaus.Add(NM);
                }
                return Json(new { data = nhomMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailVS(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ViSinh> viSinhs = new List<ViSinh>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var VS = db.ViSinhs.FirstOrDefault(p => p.ID == id);
                    VS.ChiDinh = true;
                    viSinhs.Add(VS);
                }
                return Json(new { data = viSinhs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailTuanHoan(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<TuanHoan> tuanHoans = new List<TuanHoan>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var tuanHoan = db.TuanHoans.FirstOrDefault(p => p.ID == id);
                    tuanHoan.ChiDinh = true;
                    tuanHoans.Add(tuanHoan);
                }
                return Json(new { data = tuanHoans }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailHoHap(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<HoHap> hoHaps = new List<HoHap>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var hoHap = db.HoHaps.FirstOrDefault(p => p.ID == id);
                    hoHap.ChiDinh = true;
                    hoHaps.Add(hoHap);
                }
                return Json(new { data = hoHaps }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailTieuHoa(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<TieuHoa> tieuHoas = new List<TieuHoa>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var tieuHoa = db.TieuHoas.FirstOrDefault(p => p.ID == id);
                    tieuHoa.ChiDinh = true;
                    tieuHoas.Add(tieuHoa);
                }
                return Json(new { data = tieuHoas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailThanTietNieu(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ThanTietNieu> thanTietNieus = new List<ThanTietNieu>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var thanTietNieu = db.ThanTietNieux.FirstOrDefault(p => p.ID == id);
                    thanTietNieu.ChiDinh = true;
                    thanTietNieus.Add(thanTietNieu);
                }
                return Json(new { data = thanTietNieus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailCoXuongKhops(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<CoXuongKhop> coXuongKhops = new List<CoXuongKhop>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var coXuongKhop = db.CoXuongKhops.FirstOrDefault(p => p.ID == id);
                    coXuongKhop.ChiDinh = true;
                    coXuongKhops.Add(coXuongKhop);
                }
                return Json(new { data = coXuongKhops }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailThanKinhs(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ThanKinh> thanKinhs = new List<ThanKinh>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var thanKinh = db.ThanKinhs.FirstOrDefault(p => p.ID == id);
                    thanKinh.ChiDinh = true;
                    thanKinhs.Add(thanKinh);
                }
                return Json(new { data = thanKinhs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailTamThans(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<TamThan> tamThans = new List<TamThan>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var tamThan = db.TamThans.FirstOrDefault(p => p.ID == id);
                    tamThan.ChiDinh = true;
                    tamThans.Add(tamThan);
                }
                return Json(new { data = tamThans }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailNgoaiKhoas(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<NgoaiKhoa> ngoaiKhoas = new List<NgoaiKhoa>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var ngoaiKhoa = db.NgoaiKhoas.FirstOrDefault(p => p.ID == id);
                    ngoaiKhoa.ChiDinh = true;
                    ngoaiKhoas.Add(ngoaiKhoa);
                }
                return Json(new { data = ngoaiKhoas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailSanPhuKhoas(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<SanPhuKhoa> sanPhuKhoas = new List<SanPhuKhoa>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var sanPhuKhoa = db.SanPhuKhoas.FirstOrDefault(p => p.ID == id);
                    sanPhuKhoa.ChiDinh = true;
                    sanPhuKhoas.Add(sanPhuKhoa);
                }
                return Json(new { data = sanPhuKhoas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailMats(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Mat> mats = new List<Mat>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var mat = db.Mats.FirstOrDefault(p => p.ID == id);
                    mat.ChiDinh = true;
                    mats.Add(mat);
                }
                return Json(new { data = mats }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailTais(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Tai> tais = new List<Tai>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var tai = db.Tais.FirstOrDefault(p => p.ID == id);
                    tai.ChiDinh = true;
                    tais.Add(tai);
                }
                return Json(new { data = tais }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailMuis(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Mui> muis = new List<Mui>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var mui = db.Muis.FirstOrDefault(p => p.ID == id);
                    mui.ChiDinh = true;
                    muis.Add(mui);
                }
                return Json(new { data = muis }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailHongs(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Hong> hongs = new List<Hong>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var hong = db.Hongs.FirstOrDefault(p => p.ID == id);
                    hong.ChiDinh = true;
                    hongs.Add(hong);
                }
                return Json(new { data = hongs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailRangHamMats(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<RangHamMat> rangHamMats = new List<RangHamMat>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var rangHamMat = db.RangHamMats.FirstOrDefault(p => p.ID == id);
                    rangHamMat.ChiDinh = true;
                    rangHamMats.Add(rangHamMat);
                }
                return Json(new { data = rangHamMats }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDetailDaLieus(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<DaLieu> daLieus = new List<DaLieu>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var daLieu = db.DaLieux.FirstOrDefault(p => p.ID == id);
                    daLieu.ChiDinh = true;
                    daLieus.Add(daLieu);
                }
                return Json(new { data = daLieus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkExistPatient(Patient patient)
        {
            var checkExist = db.Patients.FirstOrDefault(p => p.Name == patient.Name && p.Address == patient.Address && p.BirthDate == patient.BirthDate);
            if (checkExist != null)
            {
                return Json(new { success = false, responseText = "Bệnh Nhân Đã Tồn Tại. Vui Lòng Kiểm Tra Lại" });
            }
            else
            {
                return Json(new { success = true });
            }
        }

        [HttpGet]
        // GET: Admin/MultipleModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MultipleModels/Create
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> Create(MultiplesModel multiplesModel)
         {
            PatientsController patientsController = new PatientsController();
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            Detail_HistoryDiseaseController detail_HistoryDiseaseController = new Detail_HistoryDiseaseController();
            Detail_MedicalHistoryController detail_MedicalHistoryController = new Detail_MedicalHistoryController();
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
                detail_HistoryDiseaseController.Create(PatientID, multiplesModel);
                detail_MedicalHistoryController.Create(PatientID, multiplesModel);
                informationExaminationsController.Create(PatientID, multiplesModel);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ex)
            {
                var error = ex;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExaminationList = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var inforamtionExamination = inforamtionExaminationList.LastOrDefault();
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // GET: Admin/MultipleModels/BillExamination/5
        public ActionResult BillExamination(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExaminationList = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var inforamtionExamination = inforamtionExaminationList.LastOrDefault();
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/CreateOldPatient/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> CreateOldPatient(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                Detail_HistoryDiseaseController detail_HistoryDiseaseController = new Detail_HistoryDiseaseController();
                Detail_MedicalHistoryController detail_MedicalHistoryController = new Detail_MedicalHistoryController();
                multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
                Detail_MedicalHistory detail_MedicalHistory = new Detail_MedicalHistory();
                Detail_HistoryDisease detail_HistoryDisease = new Detail_HistoryDisease();
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateOldPatient(multiplesModel);
                detail_HistoryDiseaseController.CreateOldPatient(multiplesModel);
                detail_MedicalHistoryController.CreateOldPatient(multiplesModel);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ex1)
            {
                var error = ex1;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/CreateOldPatient/5
        public ActionResult CreateTest(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == inforamtionExamination.Patient_ID);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/CreateOldPatient/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> CreateTest(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                Detail_HistoryDiseaseController detail_HistoryDiseaseController = new Detail_HistoryDiseaseController();
                Detail_MedicalHistoryController detail_MedicalHistoryController = new Detail_MedicalHistoryController();
                Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
                Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
                Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
                Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
                Detail_UrineController detail_UrineController = new Detail_UrineController();
                Detail_ImmuneController detail_ImmuneController = new Detail_ImmuneController();
                Detail_AmniocenteController detail_AmniocenteController = new Detail_AmniocenteController();
                Detail_ViSinhController detail_ViSinhController = new Detail_ViSinhController();
                ClinicalsController clinicalsController = new ClinicalsController();
                Prescription_DetailController prescription_DetailController = new Prescription_DetailController();
                //CayMausController cayMausController = new CayMausController();

                //Clinical
                Detail_TuanHoanController detail_TuanHoanController = new Detail_TuanHoanController();
                Detail_HoHapController detail_HoHapController = new Detail_HoHapController();
                Detail_TieuHoaController detail_TieuHoaController = new Detail_TieuHoaController();
                Detail_ThanTietNieuController detail_ThanTietNieuController = new Detail_ThanTietNieuController();
                Detail_CoXuongKhopController detail_CoXuongKhopController = new Detail_CoXuongKhopController();
                Detail_ThanKinhController detail_ThanKinhController = new Detail_ThanKinhController();
                Detail_TamThanController detail_TamThanController = new Detail_TamThanController();
                Detail_NgoaiKhoaController detail_NgoaiKhoaController = new Detail_NgoaiKhoaController();
                Detail_SanPhuKhoaController detail_SanPhuKhoaController = new Detail_SanPhuKhoaController();
                Detail_MatController detail_MatController = new Detail_MatController();
                Detail_TaiController detail_TaiController = new Detail_TaiController();
                Detail_MuiController detail_MuiController = new Detail_MuiController();
                Detail_HongController detail_HongController = new Detail_HongController();
                Detail_RangHamMatController detail_RangHamMatController = new Detail_RangHamMatController();
                Detail_DaLieuController detail_DaLieuController = new Detail_DaLieuController();

                Detail_CTMau detail_CTMau = new Detail_CTMau();
                Detail_SinhHoaMau detail_SinhHoaMau = new Detail_SinhHoaMau();
                Detail_DongMau detail_DongMau = new Detail_DongMau();
                Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
                Detail_Urine detail_Urine = new Detail_Urine();
                Detail_Immune detail_Immune = new Detail_Immune();
                Detail_Amniocente detail_Amniocente = new Detail_Amniocente();
                Detail_ViSinh detail_ViSinh = new Detail_ViSinh();
                Detail_MedicalHistory detail_MedicalHistory = new Detail_MedicalHistory();
                Detail_HistoryDisease detail_HistoryDisease = new Detail_HistoryDisease();

                multiplesModel.CTMau = multiplesModel.CTMau.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.SinhHoaMau = multiplesModel.SinhHoaMau.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.DongMau = multiplesModel.DongMau.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.NhomMau = multiplesModel.NhomMau.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Urine = multiplesModel.Urine.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Immune = multiplesModel.Immune.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Amniocente = multiplesModel.Amniocente.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.ViSinh = multiplesModel.ViSinh.Where(p => p.ChiDinh == true).ToList();

                // Set Up Clinical
                multiplesModel.TuanHoan = multiplesModel.TuanHoan.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.HoHap = multiplesModel.HoHap.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.TieuHoa = multiplesModel.TieuHoa.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.ThanTietNieu = multiplesModel.ThanTietNieu.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.CoXuongKhop = multiplesModel.CoXuongKhop.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.TamThan = multiplesModel.TamThan.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.ThanKinh = multiplesModel.ThanKinh.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.NgoaiKhoa = multiplesModel.NgoaiKhoa.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.SanPhuKhoa = multiplesModel.SanPhuKhoa.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Mat = multiplesModel.Mat.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Tai = multiplesModel.Tai.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Mui = multiplesModel.Mui.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.Hong = multiplesModel.Hong.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.RangHamMat = multiplesModel.RangHamMat.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.DaLieu = multiplesModel.DaLieu.Where(p => p.ChiDinh == true).ToList();

                // Set Up Bệnh Tiền Sử, Bệnh Sử
                multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
                multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();

                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateTest(multiplesModel.InformationExamination, multiplesModel.Detail_DiagnosticsCategory);
                detail_HistoryDiseaseController.CreateOldPatient(multiplesModel);
                detail_MedicalHistoryController.CreateOldPatient(multiplesModel);
                var CongThucMauCD = detail_CTMauController.CreateOldPatient(detail_CTMau, multiplesModel.CTMau, multiplesModel.InformationExamination.ID, multiplesModel);
                var SinhHoaMauCD = detail_SinhHoaMauController.CreateOldPatient(detail_SinhHoaMau, multiplesModel.SinhHoaMau, multiplesModel.InformationExamination.ID, multiplesModel);
                var DongMauCD = detail_DongMauController.CreateOldPatient(detail_DongMau, multiplesModel.DongMau, multiplesModel.InformationExamination.ID, multiplesModel);
                var NhomMauCD = detail_NhomMauController.CreateOldPatient(detail_NhomMau, multiplesModel.NhomMau, multiplesModel.InformationExamination.ID, multiplesModel);
                var UrineCD = detail_UrineController.CreateOldPatient(detail_Urine, multiplesModel.Urine, multiplesModel.InformationExamination.ID, multiplesModel);
                var ImmuneCD = detail_ImmuneController.CreateOldPatient(detail_Immune, multiplesModel.Immune, multiplesModel.InformationExamination.ID, multiplesModel);
                var AmniocenteCD = detail_AmniocenteController.CreateOldPatient(detail_Amniocente, multiplesModel.Amniocente, multiplesModel.InformationExamination.ID, multiplesModel);
                var ViSinhCD = detail_ViSinhController.CreateOldPatient(detail_ViSinh, multiplesModel.ViSinh, multiplesModel.InformationExamination.ID, multiplesModel);
                clinicalsController.CreateOldPatient(multiplesModel);

                //Clinical
                detail_TuanHoanController.CreateOldPatient(multiplesModel);
                detail_HoHapController.CreateOldPatient(multiplesModel);
                detail_TieuHoaController.CreateOldPatient(multiplesModel);
                detail_ThanTietNieuController.CreateOldPatient(multiplesModel);
                detail_CoXuongKhopController.CreateOldPatient(multiplesModel);
                detail_ThanKinhController.CreateOldPatient(multiplesModel);
                detail_TamThanController.CreateOldPatient(multiplesModel);
                detail_NgoaiKhoaController.CreateOldPatient(multiplesModel);
                detail_SanPhuKhoaController.CreateOldPatient(multiplesModel);
                detail_MatController.CreateOldPatient(multiplesModel);
                detail_TaiController.CreateOldPatient(multiplesModel);
                detail_MuiController.CreateOldPatient(multiplesModel);
                detail_HongController.CreateOldPatient(multiplesModel);
                detail_RangHamMatController.CreateOldPatient(multiplesModel);
                detail_DaLieuController.CreateOldPatient(multiplesModel);

                //cayMausController.CreateOldPatient(multiplesModel);
                prescription_DetailController.CreateOldPatient(multiplesModel);
                return RedirectToAction("Index", "Patients");
            }
            catch (Exception ex1)
            {
                var error = ex1;
                return RedirectToAction("Index", "Patients");
            }
        }

        // GET: Admin/MultipleModels/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExaminationList = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var inforamtionExamination = inforamtionExaminationList.LastOrDefault();
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult EditByID(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult DetailsIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            var details_diagnostic = db.Detail_DiagnosticsCategory.FirstOrDefault(p => p.InformationExamination_ID == id);
            var clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            multiplesModel.Clinical = clinical;
            multiplesModel.Detail_DiagnosticsCategory = details_diagnostic;
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult DetailsIERead(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            var details_diagnostic = db.Detail_DiagnosticsCategory.FirstOrDefault(p => p.InformationExamination_ID == id);
            var clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            multiplesModel.Clinical = clinical;
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Detail_DiagnosticsCategory = details_diagnostic;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/Edit/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> Edit(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                Detail_HistoryDiseaseController detail_HistoryDiseaseController = new Detail_HistoryDiseaseController();
                Detail_MedicalHistoryController detail_MedicalHistoryController = new Detail_MedicalHistoryController();
                Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
                Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
                Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
                Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
                Detail_UrineController detail_UrineController = new Detail_UrineController();
                Detail_ImmuneController detail_ImmuneController = new Detail_ImmuneController();
                Detail_AmniocenteController detail_AmniocenteController = new Detail_AmniocenteController();
                Detail_ViSinhController detail_ViSinhController = new Detail_ViSinhController();
                ClinicalsController clinicalsController = new ClinicalsController();
                Prescription_DetailController prescription_DetailController = new Prescription_DetailController();
                //CayMausController cayMausController = new CayMausController();
                if(multiplesModel.HistoryDiseases1 != null)
                {
                    multiplesModel.HistoryDiseases1 = multiplesModel.HistoryDiseases1.Where(p => p.ChiDinh == true).ToList();
                }
                if(multiplesModel.HistoryDiseases2 != null)
                {
                    multiplesModel.HistoryDiseases2 = multiplesModel.HistoryDiseases2.Where(p => p.ChiDinh == true).ToList();
                }
                if(multiplesModel.HistoryDiseases3 != null)
                {
                    multiplesModel.HistoryDiseases3 = multiplesModel.HistoryDiseases3.Where(p => p.ChiDinh == true).ToList();
                }
                if(multiplesModel.MedicalHistories != null)
                {
                    multiplesModel.MedicalHistories = multiplesModel.MedicalHistories.Where(p => p.ChiDinh == true).ToList();
                }

                patientsController.Edit(multiplesModel.Patient);
                informationExaminationsController.Edit(multiplesModel.InformationExamination, multiplesModel.Detail_DiagnosticsCategory);
                if (multiplesModel.MedicalHistories != null)
                {
                    detail_MedicalHistoryController.CreateOldPatient(multiplesModel);
                }
                if(multiplesModel.HistoryDiseases1 != null || multiplesModel.HistoryDiseases2 != null || multiplesModel.HistoryDiseases3 != null)
                {
                    detail_HistoryDiseaseController.CreateOldPatient(multiplesModel);
                }
                var CongThucMauEdit = Task.Run(() => detail_CTMauController.Edit(multiplesModel));
                var SinhHoaMauEdit = Task.Run(() => detail_SinhHoaMauController.Edit(multiplesModel));
                var DongMauEdit = Task.Run(() => detail_DongMauController.Edit(multiplesModel));
                var NhomMauEdit = Task.Run(() => detail_NhomMauController.Edit(multiplesModel));
                var UrineEdit = Task.Run(() => detail_UrineController.Edit(multiplesModel));
                var ImmuneEdit = Task.Run(() => detail_ImmuneController.Edit(multiplesModel));
                var AmniocenteEdit = Task.Run(() => detail_AmniocenteController.Edit(multiplesModel));
                var ViSinhEdit = Task.Run(() => detail_ViSinhController.Edit(multiplesModel));
                var ResultEdit = await Task.WhenAll(CongThucMauEdit, SinhHoaMauEdit, DongMauEdit, NhomMauEdit, UrineEdit, ImmuneEdit, AmniocenteEdit, ViSinhEdit);
                clinicalsController.Edit(multiplesModel);
                prescription_DetailController.Edit(multiplesModel);
                if(multiplesModel.Detail_CTMaus != null)
                {
                    var checkResultCTMau = multiplesModel.Detail_CTMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultCTMau == true)
                    {
                        multiplesModel.InformationExamination.ResultCTMau = true;
                    }
                }
                if(multiplesModel.Detail_SinhHoaMaus != null)
                {
                    var checkResultSHM = multiplesModel.Detail_SinhHoaMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultSHM == true)
                    {
                        multiplesModel.InformationExamination.ResultSHM = true;
                    }
                }
                if(multiplesModel.Detail_DongMaus != null)
                {
                    var checkResultDMau = multiplesModel.Detail_DongMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultDMau == true)
                    {
                        multiplesModel.InformationExamination.ResultDMau = true;
                    }
                }
                if(multiplesModel.Detail_NhomMaus != null)
                {
                    var checkResultNMau = multiplesModel.Detail_NhomMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultNMau == true)
                    {
                        multiplesModel.InformationExamination.ResultNhomMau = true;
                    }
                }
                if(multiplesModel.Detail_Urines != null)
                {
                    var checkResultNuocTieu = multiplesModel.Detail_Urines.All(p => p.Result != null && p.InfomationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultNuocTieu == true)
                    {
                        multiplesModel.InformationExamination.ResultNuocTieu = true;
                    }
                }
                if(multiplesModel.Detail_Immunes != null)
                {
                    var checkResultImmune = multiplesModel.Detail_Immunes.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultImmune == true)
                    {
                        multiplesModel.InformationExamination.ResultMienDich = true;
                    }
                }
                if(multiplesModel.Detail_Amniocentes != null)
                {
                    var checkResultAmniocente = multiplesModel.Detail_Amniocentes.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultAmniocente == true)
                    {
                        multiplesModel.InformationExamination.ResultDichChocDo = true;
                    }
                }
                if (multiplesModel.Detail_ViSinhs != null)
                {
                    var checkResultViSinhs = multiplesModel.Detail_ViSinhs.All(p => p.Result != null || p.ResultNC != null || p.ResultDD != null || p.MatDo != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultViSinhs == true)
                    {
                        multiplesModel.InformationExamination.ResultViSinh = true;
                    }
                }
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                db.SaveChanges();
                //cayMausController.Edit(multiplesModel.CayMau, Server);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ED)
            {
                var errorEdit = ED;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // POST: Admin/MultipleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var InfoExamination = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            if(InfoExamination.Count == 1)
            {
                var checkInfoExam = db.InformationExaminations.FirstOrDefault(p => p.Patient_ID == id);
                var checkBill = db.Bills.FirstOrDefault(p => p.Patient_ID == id);
                if (checkInfoExam.HeartBeat != null || checkInfoExam.Breathing != null
                    || checkInfoExam.BloodPressure != null || checkInfoExam.Weight != null
                    || checkInfoExam.Height != null || checkInfoExam.ResultCTMau != null
                    || checkInfoExam.ResultSHM != null || checkInfoExam.ResultDMau != null
                    || checkInfoExam.ResultNhomMau != null || checkInfoExam.ResultNuocTieu != null
                    || checkInfoExam.ResultMienDich != null || checkInfoExam.ResultDichChocDo != null
                    || checkInfoExam.ResultViSinh != null || checkBill != null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    var patient = db.Patients.Find(id);
                    var historyDiseases = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id).ToList();
                    if(historyDiseases.Count != 0)
                    {
                        foreach(var item in historyDiseases)
                        {
                            db.Detail_HistoryDisease.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    var medicalHistory = db.Detail_MedicalHistory.Where(p => p.Patient_ID == id).ToList();
                    if(medicalHistory.Count != 0)
                    {
                        foreach(var item1 in medicalHistory)
                        {
                            db.Detail_MedicalHistory.Remove(item1);
                            db.SaveChanges();
                        }
                    }
                    db.InformationExaminations.Remove(checkInfoExam);
                    db.Patients.Remove(patient);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new { success = false });
            }            
        }
    }
}
