﻿using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        // GET: Admin/MultipleModels/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Print()
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
        [Obsolete]
        public async Task<RedirectToRouteResult> Create(MultiplesModel multiplesModel)
         {
            PatientsController patientsController = new PatientsController();
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
            Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
            Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
            Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
            Detail_UrineController detail_UrineController = new Detail_UrineController();
            Detail_ImmuneController detail_ImmuneController = new Detail_ImmuneController();
            Detail_AmniocenteController detail_AmniocenteController = new Detail_AmniocenteController();
            ClinicalsController clinicalsController = new ClinicalsController();
            //CayMausController cayMausController = new CayMausController();
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
                informationExaminationsController.Create(multiplesModel.InformationExamination, PatientID);
                var CongThucMau = Task.Run(() => detail_CTMauController.Create(multiplesModel.CTMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var SinhHoaMau = Task.Run(() => detail_SinhHoaMauController.Create(multiplesModel.SinhHoaMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var DongMau = Task.Run(() => detail_DongMauController.Create(multiplesModel.DongMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var NhomMau =  Task.Run(() => detail_NhomMauController.Create(multiplesModel.NhomMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var Urine =  Task.Run(() => detail_UrineController.Create(multiplesModel.Urine, multiplesModel.InformationExamination.ID, multiplesModel));
                var Immune =  Task.Run(() => detail_ImmuneController.Create(multiplesModel.Immune, multiplesModel.InformationExamination.ID, multiplesModel));
                var Amniocente =  Task.Run(() => detail_AmniocenteController.Create(multiplesModel.Amniocente, multiplesModel.InformationExamination.ID, multiplesModel));
                //cayMausController.Create(multiplesModel, Server);
                var Result = await Task.WhenAll(CongThucMau, SinhHoaMau, DongMau, NhomMau, Urine, Immune, Amniocente);
                var Clinical = clinicalsController.Create(multiplesModel);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ex)
            {
                //return View();
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

        // POST: Admin/MultipleModels/CreateOldPatient/5
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
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
                ClinicalsController clinicalsController = new ClinicalsController();
                //CayMausController cayMausController = new CayMausController();
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateOldPatient(multiplesModel.InformationExamination);
                detail_CTMauController.CreateOldPatient(multiplesModel);
                detail_SinhHoaMauController.CreateOldPatient(multiplesModel);
                detail_DongMauController.CreateOldPatient(multiplesModel);
                detail_NhomMauController.CreateOldPatient(multiplesModel);
                detail_UrineController.CreateOldPatient(multiplesModel);
                detail_ImmuneController.CreateOldPatient(multiplesModel);
                detail_AmniocenteController.CreateOldPatient(multiplesModel);
                clinicalsController.CreateOldPatient(multiplesModel);
                //cayMausController.CreateOldPatient(multiplesModel);
                return RedirectToAction("Index", "Patients");
            }
            catch
            {
                return View();
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

        public ActionResult DetailsIE(int id)
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
        public ActionResult Edit(MultiplesModel multiplesModel)
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
                ClinicalsController clinicalsController = new ClinicalsController();
                //CayMausController cayMausController = new CayMausController();
                patientsController.Edit(multiplesModel.Patient);
                informationExaminationsController.Edit(multiplesModel.InformationExamination);
                detail_CTMauController.Edit(multiplesModel);
                detail_SinhHoaMauController.Edit(multiplesModel);
                detail_DongMauController.Edit(multiplesModel);
                detail_NhomMauController.Edit(multiplesModel);
                detail_UrineController.Edit(multiplesModel);
                detail_ImmuneController.Edit(multiplesModel);
                detail_AmniocenteController.Edit(multiplesModel);
                clinicalsController.Edit(multiplesModel);
                //cayMausController.Edit(multiplesModel.CayMau, Server);
                return RedirectToAction("Index", "Patients");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/MultipleModels/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
                    var clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == checkInfoExam.ID);
                    if(clinical.KidneyUrology != null
                        || clinical.Circles != null || clinical.Respiration != null || clinical.KidneyUrology != null
                        || clinical.Digestion != null || clinical.MuscleBoneJoint != null || clinical.Nerve != null
                        || clinical.Mental != null || clinical.Surgery != null || clinical.ObstetricsAndGynecology != null
                        || clinical.RightEyesNoGlasses != null || clinical.LeftEyesNoGlasses != null || clinical.RightEyesGlasses != null
                        || clinical.LeftEyesGlasses != null || clinical.EyesExamination != null || clinical.LeftEarSay != null
                        || clinical.LeftEarWhisper != null || clinical.RightEarSay != null || clinical.RightEarWhisper != null
                        || clinical.EarNoseThroatExamination != null || clinical.UpperJaw != null || clinical.LowerJaw != null
                        || clinical.TeethJawFaceExamination != null || clinical.Dermatology != null)
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        var patient = db.Patients.Find(id);
                        db.InformationExaminations.Remove(checkInfoExam);
                        db.Patients.Remove(patient);
                        db.Clinicals.Remove(clinical);
                        db.SaveChanges();
                        return Json(new { success = true });
                    }
                }
            }
            else
            {
                return Json(new { success = false });
            }            
        }
    }
}
