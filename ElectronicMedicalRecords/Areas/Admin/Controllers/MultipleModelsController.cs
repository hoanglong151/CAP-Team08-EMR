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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class MultipleModelsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/MultipleModels
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintExaminationInfo()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModels"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            return View(multiplesModel);
        }

        public ActionResult Payment(int id, int price)
        {
            var examinationBill = db.InformationExaminations.Find(id);
            if(examinationBill.New == null && examinationBill.PriceExamination == null)
            {
                examinationBill.New = false;
                examinationBill.PriceExamination = price;
                db.Entry(examinationBill).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, responeText = "Hóa Đơn Này Đã Được Thanh Toán" });
            }
        }

        public ActionResult PaymentPrescription(int id)
        {
            int error = 0;
            var prescriptionsBill = db.Prescription_Detail.Where(p => p.InformationExamination_ID == id).ToList();
            foreach(var bill in prescriptionsBill)
            {
                if(bill.TotalPrice == null)
                {
                    bill.TotalPrice = bill.NumMedication * bill.Medication.Price;
                    db.Entry(bill).State = EntityState.Modified;
                    db.SaveChanges();
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
            return Json(new { success = true });
        }

        public ActionResult PaymentTestSubclinical(int id, int price)
        {
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
                db.Entry(examinationBill).State = EntityState.Modified;
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
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
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
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
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
            multiplesModel.InformationExamination.DiagnosticsCategory = db.DiagnosticsCategories.Find(multiplesModel.InformationExamination.DiagnosticCategory_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
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
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
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
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateOldPatient(multiplesModel);
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

                Detail_CTMau detail_CTMau = new Detail_CTMau();
                Detail_SinhHoaMau detail_SinhHoaMau = new Detail_SinhHoaMau();
                Detail_DongMau detail_DongMau = new Detail_DongMau();
                Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
                Detail_Urine detail_Urine = new Detail_Urine();
                Detail_Immune detail_Immune = new Detail_Immune();
                Detail_Amniocente detail_Amniocente = new Detail_Amniocente();
                Detail_ViSinh detail_ViSinh = new Detail_ViSinh();
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateTest(multiplesModel.InformationExamination);
                var CongThucMauCD = Task.Run(() => detail_CTMauController.CreateOldPatient(detail_CTMau, multiplesModel.CTMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var SinhHoaMauCD = Task.Run(() => detail_SinhHoaMauController.CreateOldPatient(detail_SinhHoaMau, multiplesModel.SinhHoaMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var DongMauCD = Task.Run(() => detail_DongMauController.CreateOldPatient(detail_DongMau, multiplesModel.DongMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var NhomMauCD = Task.Run(() => detail_NhomMauController.CreateOldPatient(detail_NhomMau, multiplesModel.NhomMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var UrineCD = Task.Run(() => detail_UrineController.CreateOldPatient(detail_Urine, multiplesModel.Urine, multiplesModel.InformationExamination.ID, multiplesModel));
                var ImmuneCD = Task.Run(() => detail_ImmuneController.CreateOldPatient(detail_Immune, multiplesModel.Immune, multiplesModel.InformationExamination.ID, multiplesModel));
                var AmniocenteCD = Task.Run(() => detail_AmniocenteController.CreateOldPatient(detail_Amniocente, multiplesModel.Amniocente, multiplesModel.InformationExamination.ID, multiplesModel));
                var ViSinhCD = Task.Run(() => detail_ViSinhController.CreateOldPatient(detail_ViSinh, multiplesModel.ViSinh, multiplesModel.InformationExamination.ID, multiplesModel));
                var ResultNew = await Task.WhenAll(CongThucMauCD, SinhHoaMauCD, DongMauCD, NhomMauCD, UrineCD, ImmuneCD, AmniocenteCD, ViSinhCD);
                clinicalsController.CreateOldPatient(multiplesModel);
                //cayMausController.CreateOldPatient(multiplesModel);
                prescription_DetailController.CreateOldPatient(multiplesModel);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch (Exception ex1)
            {
                var error = ex1;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
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
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult DetailsIERead(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            multiplesModel.InformationExamination = InformationExamination;
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
                patientsController.Edit(multiplesModel.Patient);
                informationExaminationsController.Edit(multiplesModel.InformationExamination);
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
                if (checkInfoExam.HeartBeat != null || checkInfoExam.Breathing != null
                    || checkInfoExam.BloodPressure != null || checkInfoExam.Weight != null
                    || checkInfoExam.Height != null || checkInfoExam.ResultCTMau != null
                    || checkInfoExam.ResultSHM != null || checkInfoExam.ResultDMau != null
                    || checkInfoExam.ResultNhomMau != null || checkInfoExam.ResultNuocTieu != null
                    || checkInfoExam.ResultMienDich != null || checkInfoExam.ResultDichChocDo != null
                    || checkInfoExam.ResultViSinh != null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    var patient = db.Patients.Find(id);
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
