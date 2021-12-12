using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class PatientsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        MultiplesModel multiplesModel = new MultiplesModel();
        // GET: Admin/Patients
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            var patients = db.Patients.Include(p => p.Gender).Include(p => p.HomeTown).Include(p => p.Nation).Include(p => p.Nation1).ToList();
            return View(patients);
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var patients = db.Patients.Include(p => p.Gender).Include(p => p.HomeTown).Include(p => p.Nation).Include(p => p.Nation1).ToList();
            var listMedication = patients.Select(s => new
            {
                ID = s.ID,
                MaBN = s.MaBN,
                Name = s.Name,
                BirthDate = s.BirthDate.Value.ToString("yyyy"),
                Address = s.Address,
                Gender = s.Gender.Gender1
            }).ToList();
            return Json(new { data = listMedication }, JsonRequestBehavior.AllowGet);
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

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
            return PartialView("_DetailIE", multiplesModel);
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
            //var findDate = new List<InformationExamination>();
            var informationExaminations = db.InformationExaminations.ToList();
            var patients = new List<Patient>();
            if (Name != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.Name.Contains(Name)).ToList();
            }
            if (DateStart.HasValue)
            {
                TimeSpan timeStart = new TimeSpan(1, 0, 01);
                DateStart = DateStart + timeStart;
                informationExaminations = informationExaminations.Where(p => p.DateExamine >= DateStart.Value).ToList();
            }
            if (DateEnd.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                DateEnd = DateEnd + timeEnd;
                informationExaminations = informationExaminations.Where(p => p.DateEnd <= DateEnd.Value).ToList();
            }
            if( Code != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.MaBN.Contains(Code)).ToList();
            }
            if (informationExaminations.Count > 0)
            {
                for (int i = 0; i < informationExaminations.Count; i++)
                {
                    var checkexist = patients.FirstOrDefault(p => p.MaBN == informationExaminations[i].Patient.MaBN);
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
            return View("Index", patients);
        }

        // GET: Admin/Patients/Create
        public ActionResult Create()
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1");
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1");
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name");
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name");
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
            return View(patient);
        }

        // GET: Admin/Patients/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CreateOldPatient", "MultipleModels");
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Patients/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
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
            return View(patient);
        }

        // GET: Admin/Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
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
