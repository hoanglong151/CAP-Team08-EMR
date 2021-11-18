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


        // GET: Admin/MultipleModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MultipleModels/Create
        [HttpPost]
        public ActionResult Create(MultiplesModel multiplesModel)
        {
            PatientsController patientsController = new PatientsController();
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            Detail_CTMauController detail_CTMau = new Detail_CTMauController();
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
                informationExaminationsController.Create(multiplesModel.InformationExamination, PatientID);
                detail_CTMau.Create(multiplesModel.CTMau, multiplesModel.InformationExamination.ID);
                return RedirectToAction("Index", "Patients");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SearchPatient(DateTime? DateStart, DateTime? DateEnd, string Name, int? Code)
        {
            var formatDateStart = DateTime.Parse(DateStart.Value.ToString("MM/dd/yyyy"));
            var formatDateEnd = DateTime.ParseExact(DateEnd.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var find = db.InformationExaminations.FirstOrDefault(p => p.Patient_ID == Code);
            var check = find.DateExamine <= formatDateEnd;
            var findDate = db.InformationExaminations.Where(p => p.DateExamine <= formatDateEnd && p.DateEnd >= formatDateStart && p.Patient.Name == Name && p.Patient.ID == Code).ToList();
            return RedirectToAction("Index","Patients",findDate);
        }

        // GET: Admin/MultipleModels/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/MultipleModels/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
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
