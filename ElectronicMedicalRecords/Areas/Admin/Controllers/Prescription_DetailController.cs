using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    public class Prescription_DetailController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Prescription_Detail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListPrescriptionBill()
        {
            List<Prescription_Detail> prescription_Details = new List<Prescription_Detail>();
            List<InformationExamination> informationExaminations = new List<InformationExamination>();
            var UserID = User.Identity.GetUserId();
            var prescriptionList = db.Prescription_Detail.ToList();
            foreach (var prescription in prescriptionList)
            {
                var check = prescription_Details.FirstOrDefault(p => p.InformationExamination_ID == prescription.InformationExamination_ID);
                if (check == null)
                {
                    var info = db.InformationExaminations.FirstOrDefault(p => p.ID == prescription.InformationExamination_ID);
                    var userBS = db.Users.FirstOrDefault(p => p.ID == info.User_ID);
                    if(User.IsInRole("Giám Đốc") || User.IsInRole("QTV"))
                    {
                        prescription_Details.Add(prescription);
                        informationExaminations.Add(info);
                    }
                    else
                    {
                        if (userBS.UserID == UserID)
                        {
                            prescription_Details.Add(prescription);
                            informationExaminations.Add(info);
                        }
                    }
                }
            }
            return View(informationExaminations);
        }

        public ActionResult ListExamination()
        {
            List<InformationExamination> informationExaminations = new List<InformationExamination>();
            var UserID = User.Identity.GetUserId();
            var informationList = db.InformationExaminations.ToList();
            foreach (var information in informationList)
            {
                if(information.User_ID != null)
                {
                    var userBS = db.Users.FirstOrDefault(p => p.ID == information.User_ID);
                    var userByID = db.AspNetUsers.Find(userBS.UserID);
                    if (User.IsInRole("Giám Đốc") || User.IsInRole("QTV"))
                    {
                        information.User.AspNetUser = userByID;
                        informationExaminations.Add(information);
                    }
                    else
                    {
                        if (userBS.UserID == UserID)
                        {
                            information.User.AspNetUser = userByID;
                            informationExaminations.Add(information);
                        }
                    }
                }                
            }
            return View(informationExaminations);
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.Find(id);
            multiplesModel.InformationExamination = info;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult DetailIERead(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var info = db.InformationExaminations.Find(id);
            multiplesModel.InformationExamination = info;
            return PartialView("_DetailIERead", multiplesModel);
        }

        // GET: Admin/Prescription_Detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription_Detail prescription_Detail = db.Prescription_Detail.Find(id);
            if (prescription_Detail == null)
            {
                return HttpNotFound();
            }
            return View(prescription_Detail);
        }

        public ActionResult CreateOldPatient()
        {
            ViewBag.NameDiagnostics = db.DiagnosticsCategories.ToList();
            ViewBag.NameMedication = db.Medications.ToList();
            return PartialView("_CreateOldPatient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            if(multiplesModel.Prescription_Details != null)
            {
                foreach (var prescription_Detail in multiplesModel.Prescription_Details)
                {
                    if (ModelState.IsValid)
                    {
                        prescription_Detail.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                        db.Prescription_Detail.Add(prescription_Detail);
                        db.SaveChanges();
                    }
                }
            }
            ViewBag.Medication_ID = new SelectList(db.Medications, "ID", "Name", multiplesModel.Prescription_Detail.Medication_ID);
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        // POST: Admin/Prescription_Detail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NumMedication,Note,Medication_ID,Precription__ID")] Prescription_Detail prescription_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Prescription_Detail.Add(prescription_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Medication_ID = new SelectList(db.Medications, "ID", "Name", prescription_Detail.Medication_ID);
            return View(prescription_Detail);
        }

        // GET: Admin/Prescription_Detail/Edit/5
        public ActionResult Edit(int? id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            ViewBag.NameDiagnostic = db.DiagnosticsCategories.ToList();
            ViewBag.NameMedication = db.Medications.ToList();
            if(id != null)
            {
                var info = db.InformationExaminations.Find(id);
                if(info.DiagnosticCategory_ID != null)
                {
                    info.DiagnosticsCategory = db.DiagnosticsCategories.First(p => p.ID == info.DiagnosticCategory_ID);
                }
                multiplesModel.InformationExamination = info;
            }
            return PartialView("_Edit", multiplesModel);
        }

        // GET: Admin/Prescription_Detail/Edit/5
        public ActionResult EditNoButton(int? id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            ViewBag.NameDiagnostic = db.DiagnosticsCategories.ToList();
            ViewBag.NameMedication = db.Medications.ToList();
            if (id != null)
            {
                var info = db.InformationExaminations.Find(id);
                if (info.DiagnosticCategory_ID != null)
                {
                    info.DiagnosticsCategory = db.DiagnosticsCategories.First(p => p.ID == info.DiagnosticCategory_ID);
                }
                multiplesModel.InformationExamination = info;
            }
            return PartialView("_EditNoButton", multiplesModel);
        }

        [HttpPost]
        public ActionResult LoadPrescription(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == id).ToList();
            List<object> listPresciptions = new List<object>();
            foreach(var item in listPrescription)
            {
                var medication = db.Medications.FirstOrDefault(p => p.ID == item.Medication_ID);
                var prescription = new { item.Medication_ID, item.Note, item.NumMedication, item.InformationExamination_ID, item.ID, medication.Name };
                listPresciptions.Add(prescription);
            }
            return Json(new { success = true, data = listPresciptions }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Prescription_Detail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).ToList();
            if(multiplesModel.Prescription_Details != null)
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
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditMedication(MultiplesModel multiplesModel)
        {
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
            return Json(new { success = true });
        }

        // GET: Admin/Prescription_Detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription_Detail prescription_Detail = db.Prescription_Detail.Find(id);
            if (prescription_Detail == null)
            {
                return HttpNotFound();
            }
            return View(prescription_Detail);
        }

        // POST: Admin/Prescription_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescription_Detail prescription_Detail = db.Prescription_Detail.Find(id);
            db.Prescription_Detail.Remove(prescription_Detail);
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
