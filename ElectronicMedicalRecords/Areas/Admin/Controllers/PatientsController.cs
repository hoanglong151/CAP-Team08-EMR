using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class PatientsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        MultiplesModel multiplesModel = new MultiplesModel();
        // GET: Admin/Patients
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Patient> patientlist = (List<Patient>)TempData["Patient"];
            List<string> Noti = new List<string>();
            if (patientlist == null)
            {
                var patients = db.Patients.Include(p => p.Gender).Include(p => p.HomeTown).Include(p => p.Nation).Include(p => p.Nation1).Include(p => p.Ward).Include(p => p.District).Include(p => p.InformationExaminations).OrderByDescending(p => p.ID).AsNoTracking().ToList();
                var listInfo = db.InformationExaminations.Where(p => p.PatientStatus_ID == 44).AsNoTracking().ToList();
                foreach(var info in listInfo)
                {
                    info.Patient = db.Patients.FirstOrDefault(p => p.ID == info.Patient_ID);
                    if (!Noti.Contains(info.Patient.MaBN))
                    {
                        Noti.Add(info.Patient.MaBN);
                    }
                }
                var listPatient = patients.Select(s => new
                {
                    ID = s.ID,
                    MaBN = s.MaBN,
                    Name = s.Name,
                    BirthDate = s.BirthDate.Value.ToString("yyyy"),
                    Address = s.Address,
                    Gender = s.Gender.Gender1
                }).ToList();
                foreach(var noti in Noti)
                {
                    var getPatient = listPatient.FirstOrDefault(p => p.MaBN == noti);
                    listPatient.Remove(getPatient);
                    listPatient.Insert(0, getPatient);
                }
                Session["Noti"] = Noti;
                return Json(new { data = listPatient }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<InformationExamination> listInfo = new List<InformationExamination>();
                foreach (var item in patientlist)
                {
                    item.Gender = db.Genders.First(p => p.ID == item.Gender_ID);
                    var getDangerPatient = db.InformationExaminations.FirstOrDefault(p => p.PatientStatus_ID == 44 && p.Patient_ID == item.ID);
                    if(getDangerPatient != null)
                    {
                        listInfo.Add(getDangerPatient);
                    }
                }
                if(listInfo != null) {
                    foreach (var info in listInfo)
                    {
                        info.Patient = db.Patients.FirstOrDefault(p => p.ID == info.Patient_ID);
                        if (!Noti.Contains(info.Patient.MaBN))
                        {
                            Noti.Add(info.Patient.MaBN);
                        }
                    }
                }
                else
                {
                    listInfo = db.InformationExaminations.Where(p => p.PatientStatus_ID == 44).AsNoTracking().ToList();
                    foreach (var info in listInfo)
                    {
                        info.Patient = db.Patients.FirstOrDefault(p => p.ID == info.Patient_ID);
                        if (!Noti.Contains(info.Patient.MaBN))
                        {
                            Noti.Add(info.Patient.MaBN);
                        }
                    }
                }
                var listPatient1 = patientlist.Select(s => new
                {
                    ID = s.ID,
                    MaBN = s.MaBN,
                    Name = s.Name,
                    BirthDate = s.BirthDate.Value.ToString("yyyy"),
                    Address = s.Address,
                    Gender = s.Gender.Gender1
                }).ToList();
                foreach (var noti in Noti)
                {
                    var getPatient = listPatient1.FirstOrDefault(p => p.MaBN == noti);
                    listPatient1.Remove(getPatient);
                    listPatient1.Insert(0, getPatient);
                }
                Session["Noti"] = Noti;
                return Json(new { data = listPatient1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PrintBillExaminationPost(MultiplesModel multiplesModel)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Session["MultipleModelsPatient"] = multiplesModel;
            return Json(new { success = true, data = multiplesModel });
        }

        public ActionResult PrintBillPrescription()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsPatient"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            if(multiplesModel.Detail_DiagnosticsCategory != null)
            {
                multiplesModel.Detail_DiagnosticsCategory.DiagnosticsCategory = db.DiagnosticsCategories.FirstOrDefault(p => p.ID == multiplesModel.Detail_DiagnosticsCategory.DiagnosticsCategory_ID);
            }
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            if(multiplesModel.Prescription_Details != null)
            {
                var checkPrescriptions = multiplesModel.Prescription_Details.ToList();
                if (checkPrescriptions.Count != 0)
                {
                    foreach (var itemPrescription in checkPrescriptions)
                    {
                        itemPrescription.Medication = db.Medications.Find(itemPrescription.Medication_ID);
                        itemPrescription.TotalPrice = (itemPrescription.NumMedication * itemPrescription.Medication.Price);
                    }
                }
            }
            return View(multiplesModel);
        }

        public ActionResult PrintBillExamination()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsPatient"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.PatientStatus = statusPatient.Name;
            return View(multiplesModel);
        }

        public ActionResult PrintBillTestSubclinical()
        {
            db.Configuration.LazyLoadingEnabled = false;
            MultiplesModel multiplesModel = (MultiplesModel)Session["MultipleModelsPatient"];
            var gender = db.Genders.Find(multiplesModel.Patient.Gender_ID);
            var doctor = db.Users.Find(multiplesModel.InformationExamination.User_ID);
            var statusPatient = db.PatientStatus.Find(multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.Patient.Ward = db.Wards.FirstOrDefault(p => p.ID == multiplesModel.Patient.Ward_ID);
            multiplesModel.Patient.District = db.Districts.FirstOrDefault(p => p.ID == multiplesModel.Patient.District_ID);
            ViewBag.Gender = gender.Gender1;
            ViewBag.Doctor = doctor.Name;
            ViewBag.PatientStatus = statusPatient.Name;
            if(multiplesModel.Detail_CTMaus != null)
            {
                var checkCTMaus = multiplesModel.Detail_CTMaus.ToList();
                if (checkCTMaus.Count != 0)
                {
                    foreach (var itemCTMau in checkCTMaus)
                    {
                        itemCTMau.CTMau = db.CTMaus.Find(itemCTMau.CTMau_ID);
                        itemCTMau.InformationExamination = multiplesModel.InformationExamination;
                    }
                    ViewBag.CTMaus = checkCTMaus;
                }
            }
            if(multiplesModel.Detail_SinhHoaMaus != null)
            {
                var checkSHMaus = multiplesModel.Detail_SinhHoaMaus.ToList();
                if (checkSHMaus.Count != 0)
                {
                    foreach (var itemSHM in checkSHMaus)
                    {
                        itemSHM.SinhHoaMau = db.SinhHoaMaus.Find(itemSHM.SinhHoaMau_ID);
                    }
                    ViewBag.SHMaus = checkSHMaus;
                }
            }
            if(multiplesModel.Detail_DongMaus != null)
            {
                var checkDMaus = multiplesModel.Detail_DongMaus.ToList();
                if (checkDMaus.Count != 0)
                {
                    foreach (var itemDM in checkDMaus)
                    {
                        itemDM.DongMau = db.DongMaus.Find(itemDM.DongMau_ID);
                    }
                    ViewBag.DMaus = checkDMaus;
                }
            }
            if(multiplesModel.Detail_NhomMaus != null)
            {
                var checkNhomMaus = multiplesModel.Detail_NhomMaus.ToList();
                if (checkNhomMaus.Count != 0)
                {
                    foreach (var itemNM in checkNhomMaus)
                    {
                        itemNM.NhomMau = db.NhomMaus.Find(itemNM.NhomMau_ID);
                    }
                    ViewBag.NhomMaus = checkNhomMaus;
                }
            }
            if(multiplesModel.Detail_Urines != null)
            {
                var checkNuocTieu = multiplesModel.Detail_Urines.ToList();
                if (checkNuocTieu.Count != 0)
                {
                    foreach (var itemNT in checkNuocTieu)
                    {
                        itemNT.Urine = db.Urines.Find(itemNT.Urine_ID);
                    }
                    ViewBag.NuocTieu = checkNuocTieu;
                }
            }
            if(multiplesModel.Detail_Immunes != null)
            {
                var checkMienDich = multiplesModel.Detail_Immunes.ToList();
                if (checkMienDich.Count != 0)
                {
                    foreach (var itemMD in checkMienDich)
                    {
                        itemMD.Immune = db.Immunes.Find(itemMD.Immue_ID);
                    }
                    ViewBag.MienDich = checkMienDich;
                }
            }
            if(multiplesModel.Detail_Amniocentes != null)
            {
                var checkDichChocDo = multiplesModel.Detail_Amniocentes.ToList();
                if (checkDichChocDo.Count != 0)
                {
                    foreach (var itemDCD in checkDichChocDo)
                    {
                        itemDCD.Amniocente = db.Amniocentes.Find(itemDCD.Amniocente_ID);
                    }
                    ViewBag.DichChocDo = checkDichChocDo;
                }
            }
            if(multiplesModel.Detail_ViSinhs != null)
            {
                var checkViSinh = multiplesModel.Detail_ViSinhs.ToList();
                if (checkViSinh.Count != 0)
                {
                    foreach (var itemVSs in checkViSinh)
                    {
                        itemVSs.ViSinh = db.ViSinhs.Find(itemVSs.ViSinh_ID);
                    }
                    ViewBag.ViSinh = checkViSinh;
                }
            }
            return View(multiplesModel);
        }

        public ActionResult DetailIE(MultiplesModel multiplesModel)
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(db.Districts, "ID", "District1", multiplesModel.Patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(db.Wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult DetailIERead(MultiplesModel multiplesModel)
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(db.Districts, "ID", "District1", multiplesModel.Patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(db.Wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
            return PartialView("_DetailIERead", multiplesModel);
        }

        private string ConvertToUnSign(string input)
        {
            input = input.Trim().ToLower();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        [HttpPost]
        public ActionResult SearchPatient(DateTime? DateStart, DateTime? DateEnd, string Name, string Code)
        {
            PatientsController patientsController = new PatientsController();
            if (DateStart == null && DateEnd == null && Name == "" && Code == null)
            {
                TempData["Error"] = "Vui lòng nhập ít nhất 1 trường";
                return RedirectToAction("Index", "Patients");
            }
            var informationExaminations = db.InformationExaminations.ToList();
            var informationExaminationsList = db.InformationExaminations.ToList();
            var patients = new List<Patient>();
            if (Name != "")
            {     
                informationExaminations = informationExaminationsList.Where(delegate (InformationExamination c)
                {
                    if (ConvertToUnSign(c.Patient.Name).IndexOf(Name, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        return true;
                    else
                        return false;
                }).ToList();
                if (informationExaminations.Count == 0)
                {
                    informationExaminations = informationExaminationsList.Where(p => p.Patient.Name.ToLower().Contains(Name.ToLower())).ToList();
                }
                ViewBag.NamePatient = Name;
            }
            if (DateStart.HasValue)
            {
                informationExaminations = informationExaminations.Where(p => p.DateExamine >= DateStart.Value).ToList();
                ViewBag.DateStart = DateStart;
            }
            if (DateEnd.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                DateEnd = DateEnd + timeEnd;
                informationExaminations = informationExaminations.Where(p => p.DateEnd <= DateEnd.Value).ToList();
                ViewBag.DateEnd = DateEnd;
            }
            if( Code != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.MaBN.ToLower().Contains(Code.ToLower())).ToList();
                ViewBag.CodePatient = Code;
            }
            if (informationExaminations.Count > 0)
            {
                for (int i = 0; i < informationExaminations.Count; i++)
                {
                    var CodePatient = informationExaminations[i].Patient.MaBN;
                    var checkexist = patients.FirstOrDefault(p => p.MaBN == CodePatient);
                    if(checkexist == null)
                    {
                        var patient_ID = informationExaminations[i].Patient_ID;
                        var patient = db.Patients.FirstOrDefault(p => p.ID == patient_ID);
                        patients.Add(patient);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            TempData["Patient"] = patients;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchPatientNoInfo(DateTime? BirthDate, string Address, string Name, int genders)
        {
            PatientsController patientsController = new PatientsController();
            var informationExaminations = db.InformationExaminations.ToList();
            var informationExaminationsList = db.InformationExaminations.ToList();
            var patients = new List<Patient>();
            informationExaminations = informationExaminationsList.Where(p => p.Patient.Gender_ID == genders).ToList();
            ViewBag.gender = genders;
            if (Name != "")
            {
                informationExaminations = informationExaminationsList.Where(delegate (InformationExamination c)
                {
                    if (ConvertToUnSign(c.Patient.Name).IndexOf(Name, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        return true;
                    else
                        return false;
                }).ToList();
                if (informationExaminations.Count == 0)
                {
                    informationExaminations = informationExaminationsList.Where(p => p.Patient.Name.ToLower().Contains(Name.ToLower())).ToList();
                }
                ViewBag.NamePatient = Name;
            }
            if (BirthDate.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                BirthDate = BirthDate + timeEnd;
                informationExaminations = informationExaminations.Where(p => p.Patient.BirthDate <= BirthDate.Value).ToList();
                ViewBag.BirthDate = BirthDate;
            }
            if (Address != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.Address.ToLower().Contains(Address.ToLower())).ToList();
                ViewBag.Address = Address;
            }
            if (informationExaminations.Count > 0)
            {
                for (int i = 0; i < informationExaminations.Count; i++)
                {
                    var CodePatient = informationExaminations[i].Patient.MaBN;
                    var checkexist = patients.FirstOrDefault(p => p.MaBN == CodePatient);
                    if (checkexist == null)
                    {
                        var patient_ID = informationExaminations[i].Patient_ID;
                        var patient = db.Patients.FirstOrDefault(p => p.ID == patient_ID);
                        patients.Add(patient);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            TempData["Patient"] = patients;
            return View("Index");
        }

        // GET: Admin/Patients/Create
        public ActionResult Create()
        {
            var hometownFirst = db.HomeTowns.FirstOrDefault();
            var listDistricts = db.Districts.Where(p => p.HomeTown_ID == hometownFirst.ID);
            var districtFirst = listDistricts.FirstOrDefault();
            var listWards = db.Wards.Where(p => p.District_ID == districtFirst.ID);
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1");
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1");
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name");
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name");
            ViewData["Patient.District_ID"] = new SelectList(listDistricts, "ID", "District1");
            ViewData["Patient.Ward_ID"] = new SelectList(listWards, "ID", "Ward1");
            return PartialView("_Create");
        }

        // POST: Admin/Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var listPatient = db.Patients.ToList().LastOrDefault();
                if (listPatient != null)
                {
                    var numPatient = Convert.ToInt32(listPatient.MaBN.Substring(6));
                    if (numPatient < 10)
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + "00000" + (++numPatient);
                    }
                    else if (numPatient < 100)
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + "0000" + (++numPatient);
                    }
                    else if (numPatient < 1000)
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + "000" + (++numPatient);
                    }
                    else if (numPatient < 10000)
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + "00" + (++numPatient);
                    }
                    else if (numPatient < 100000)
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + "0" + (++numPatient);
                    }
                    else
                    {
                        patient.MaBN = "BN" + DateTime.Now.Year + (++numPatient);
                    }
                }
                else
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "00000" + 1;
                }
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }            
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            ViewBag.District_ID = new SelectList(db.Districts, "ID", "District1", patient.District_ID);
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Ward1", patient.Ward_ID);
            return View(patient);
        }
        
        public ActionResult checkExist(int id)
        {
            var Info = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var InfoLast = Info.LastOrDefault();
            if(InfoLast.PriceExamination == null)
            {
                return Json(new { success = true, responeText = "Một hồ sơ mới đã được tạo trước đó!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Patients/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            var listInfo = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var lastInfo = listInfo.LastOrDefault();
            var listDistricts = db.Districts.Where(p => p.HomeTown_ID == patient.HomeTown_ID);
            var listWards = db.Wards.Where(p => p.District_ID == patient.District_ID);
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(listDistricts, "ID", "District1", patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(listWards, "ID", "Ward1", patient.Ward_ID);
            multiplesModel.Patient = patient;
            multiplesModel.InformationExamination = lastInfo;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/Patients/BillExamination/5
        public ActionResult BillExamination(MultiplesModel multiplesModel)
        {
            var districts = db.Districts.Where(p => p.HomeTown_ID == multiplesModel.Patient.HomeTown_ID).ToList();
            var wards = db.Wards.Where(p => p.District_ID == multiplesModel.Patient.District_ID).ToList();
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(districts, "ID", "District1", multiplesModel.Patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
            return PartialView("_BillExamination", multiplesModel);
        }

        // GET: Admin/Patients/CreateOldPatient/5
        public ActionResult CreateTest(MultiplesModel multiplesModel)
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(db.Districts, "ID", "District1", multiplesModel.Patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(db.Wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
            return PartialView("_CreateTest", multiplesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(Patient patient, MultiplesModel multiplesModel)
        {
            if (ModelState.IsValid)
            {
                if(multiplesModel.Patient.MoneyBackup != null && multiplesModel.InformationExamination.PriceExamination == null)
                {
                    if (multiplesModel.InformationExamination.ExaminationType == true)
                    {
                        if(multiplesModel.Patient.MoneyBackup >= 250000)
                        {
                            patient.MoneyBackup = patient.MoneyBackup - 250000;
                            multiplesModel.InformationExamination.New = false;
                            multiplesModel.InformationExamination.PriceExamination = 250000;
                            multiplesModel.InformationExamination.DateExamine = DateTime.Now;
                            multiplesModel.InformationExamination.Patient_ID = patient.ID;
                            db.InformationExaminations.Add(multiplesModel.InformationExamination);
                            var userID = System.Web.HttpContext.Current.User.Identity.GetUserId();
                            var userPayment = db.Users.FirstOrDefault(p => p.UserID == userID);
                            var bill = new Bill();
                            bill.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                            bill.Patient_ID = multiplesModel.InformationExamination.Patient_ID;
                            bill.Date = DateTime.Now;
                            bill.TypePayment = "Khám";
                            bill.UserPayment_ID = userPayment.ID;
                            db.Bills.Add(bill);
                            db.SaveChanges();
                        }
                    }
                }
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CreateOldPatient", "MultipleModels");
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            ViewBag.District_ID = new SelectList(db.Districts, "ID", "District1", patient.District_ID);
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Ward1", patient.Ward_ID);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Patients/Edit/5
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", multiplesModel.Patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", multiplesModel.Patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", multiplesModel.Patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", multiplesModel.Patient.Nation1_ID);
            ViewData["Patient.District_ID"] = new SelectList(db.Districts, "ID", "District1", multiplesModel.Patient.District_ID);
            ViewData["Patient.Ward_ID"] = new SelectList(db.Wards, "ID", "Ward1", multiplesModel.Patient.Ward_ID);
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "MultipleModels");
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            ViewBag.District_ID = new SelectList(db.Districts, "ID", "District1", patient.District_ID);
            ViewBag.Ward_ID = new SelectList(db.Wards, "ID", "Ward1", patient.Ward_ID);
            return View(patient);
        }

        // POST: Admin/Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
