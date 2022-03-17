using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Controllers
{
    public class HomeController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        public ActionResult Index()
        {
            var listPatient = db.Patients.Count();
            var role = db.AspNetRoles.FirstOrDefault(x => x.Name == "Giám Đốc");
            var director = role.AspNetUsers.FirstOrDefault();
            var listDoctor = db.Users.Count();
            ViewBag.ListPatient = listPatient;
            ViewBag.ListDoctor = listDoctor;
            ViewBag.Director = director.Users.FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult LoadPrescription(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == id).ToList();
            List<object> listPresciptions = new List<object>();
            foreach (var item in listPrescription)
            {
                var medication = db.Medications.FirstOrDefault(p => p.ID == item.Medication_ID);
                var prescription = new { item.Medication_ID, item.Note, item.NumMedication, item.InformationExamination_ID, item.ID, medication.Name };
                listPresciptions.Add(prescription);
            }
            return Json(new { success = true, data = listPresciptions }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchPatient(string Code, string BHYT)
        {
            PatientsController patientsController = new PatientsController();
            List<InformationExamination> info = new List<InformationExamination>();
            List<MultiplesModel> multiplesModels = new List<MultiplesModel>();
            var patients = new List<Patient>();
            if (Code != "")
            {
                var patient = db.Patients.FirstOrDefault(p => p.MaBN == Code.ToUpper());
                if(patient != null)
                {
                    info = db.InformationExaminations.Where(i => i.Patient_ID == patient.ID).ToList();
                    ViewBag.CodePatient = Code;
                }
            }
            if (BHYT != "")
            {
                var patient = db.Patients.FirstOrDefault(p => p.InsuranceCode == BHYT);
                if(patient != null)
                {
                    info = db.InformationExaminations.Where(i => i.Patient_ID == patient.ID).ToList();
                    ViewBag.BHYTPatient = BHYT;
                }
            }
            foreach(var item in info)
            {
                MultiplesModel multiplesModel = new MultiplesModel();
                var InformationExamination = db.InformationExaminations.Find(item.ID);
                var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
                var details_diagnostic = db.Detail_DiagnosticsCategory.FirstOrDefault(p => p.InformationExamination_ID == item.ID);
                var clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == item.ID);
                List<Detail_TuanHoan> detail_TuanHoans = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_TuanHoans = detail_TuanHoans;
                List<Detail_HoHap> detail_HoHaps = db.Detail_HoHap.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_HoHaps = detail_HoHaps;
                List<Detail_TieuHoa> detail_TieuHoas = db.Detail_TieuHoa.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_TieuHoas = detail_TieuHoas;
                List<Detail_ThanTietNieu> detail_ThanTietNieus = db.Detail_ThanTietNieu.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_ThanTietNieus = detail_ThanTietNieus;
                List<Detail_CoXuongKhop> detail_CoXuongKhops = db.Detail_CoXuongKhop.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_CoXuongKhops = detail_CoXuongKhops;
                List<Detail_ThanKinh> detail_ThanKinhs = db.Detail_ThanKinh.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_ThanKinhs = detail_ThanKinhs;
                List<Detail_TamThan> detail_TamThans = db.Detail_TamThan.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_TamThans = detail_TamThans;
                List<Detail_NgoaiKhoa> detail_NgoaiKhoas = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_NgoaiKhoas = detail_NgoaiKhoas;
                List<Detail_SanPhuKhoa> detail_SanPhuKhoas = db.Detail_SanPhuKhoa.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_SanPhuKhoas = detail_SanPhuKhoas;
                List<Detail_Mat> detail_Mats = db.Detail_Mat.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_Mats = detail_Mats;
                List<Detail_TaiMuiHong> detail_TaiMuiHongs = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_TaiMuiHongs = detail_TaiMuiHongs;
                List<Detail_RangHamMat> detail_RangHamMats = db.Detail_RangHamMat.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_RangHamMats = detail_RangHamMats;
                List<Detail_DaLieu> detail_DaLieus = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_DaLieus = detail_DaLieus;
                List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_CTMaus = detail_CTMaus;
                List<Detail_SinhHoaMau> detail_SinhHoaMaus = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_SinhHoaMaus = detail_SinhHoaMaus;
                List<Detail_DongMau> detail_DongMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_DongMaus = detail_DongMaus;
                List<Detail_NhomMau> detail_NhomMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_NhomMaus = detail_NhomMaus;
                List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_Urines = detail_Urines;
                List<Detail_Immune> detail_Immunes = db.Detail_Immune.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_Immunes = detail_Immunes;
                List<Detail_Amniocente> detail_Amniocentes = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_Amniocentes = detail_Amniocentes;
                List<Detail_ViSinh> detail_ViSinhs = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == item.ID).AsNoTracking().ToList();
                multiplesModel.Detail_ViSinhs = detail_ViSinhs;
                List<Detail_HistoryDisease> detail_HistoryDiseases1 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == InformationExamination.Patient_ID && p.LevelFamily == "Ông/Bà").AsNoTracking().ToList();
                List<Detail_HistoryDisease> detail_HistoryDiseases2 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == InformationExamination.Patient_ID && p.LevelFamily == "Cha/Mẹ").AsNoTracking().ToList();
                List<Detail_HistoryDisease> detail_HistoryDiseases3 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == InformationExamination.Patient_ID && p.LevelFamily == "Anh/Chị em").AsNoTracking().ToList();
                multiplesModel.Detail_HistoryDiseases1 = detail_HistoryDiseases1;
                multiplesModel.Detail_HistoryDiseases2 = detail_HistoryDiseases2;
                multiplesModel.Detail_HistoryDiseases3 = detail_HistoryDiseases3;
                List<Detail_MedicalHistory> detail_MedicalHistories = db.Detail_MedicalHistory.Where(p => p.Patient_ID == InformationExamination.Patient_ID).AsNoTracking().ToList();
                multiplesModel.Detail_MedicalHistories = detail_MedicalHistories;

                multiplesModel.Clinical = clinical;
                multiplesModel.InformationExamination = InformationExamination;
                multiplesModel.Detail_DiagnosticsCategory = details_diagnostic;
                multiplesModel.Patient = patient;
                ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
                ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
                ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
                ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
                ViewData["Patient.District_ID"] = new SelectList(db.Districts, "ID", "District1", multiplesModel.Patient.District_ID);
                ViewData["Patient.Ward_ID"] = new SelectList(db.Wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
                var UserName = db.Users.FirstOrDefault(p => p.ID == multiplesModel.InformationExamination.User_ID);
                ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
                if (UserName != null)
                {
                    ViewBag.UserName = UserName.Name;
                }
                multiplesModels.Add(multiplesModel);
            }
            TempData["multiplesModels"] = multiplesModels;
            return RedirectToAction("FindPatient", "Home");
        }

        public ActionResult FindPatient()
        {
            List<MultiplesModel> multiplesModels = (List<MultiplesModel>)TempData["multiplesModels"];
            if (multiplesModels != null)
            {
                return View(multiplesModels);
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Doctors()
        {
            var listDoctor = db.Users.Where(p => p.IsShow == true).ToList();
            return View(listDoctor);

        } public ActionResult News()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}