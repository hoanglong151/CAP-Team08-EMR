using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
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


        // GET: Admin/MultipleModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MultipleModels/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MultiplesModel multiplesModel)
         {
            PatientsController patientsController = new PatientsController();
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
            Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
            Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
            Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
                informationExaminationsController.Create(multiplesModel.InformationExamination, PatientID);
                detail_CTMauController.Create(multiplesModel.CTMau, multiplesModel.InformationExamination.ID);
                detail_SinhHoaMauController.Create(multiplesModel.SinhHoaMau, multiplesModel.InformationExamination.ID);
                detail_DongMauController.Create(multiplesModel.DongMau, multiplesModel.InformationExamination.ID);
                detail_NhomMauController.Create(multiplesModel.NhomMau, multiplesModel.InformationExamination.ID);
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
            var inforamtionExamination = db.InformationExaminations.FirstOrDefault(p => p.Patient_ID == id);
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
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
                patientsController.Edit(multiplesModel.Patient);
                informationExaminationsController.Edit(multiplesModel.InformationExamination);
                detail_CTMauController.Edit(multiplesModel.Detail_CTMaus);
                detail_SinhHoaMauController.Edit(multiplesModel.Detail_SinhHoaMaus);
                detail_DongMauController.Edit(multiplesModel.Detail_DongMaus);
                detail_NhomMauController.Edit(multiplesModel.Detail_NhomMaus);
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
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
