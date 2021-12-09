using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;

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

        public ActionResult DetailIE()
        {
            return PartialView("_DetailIE");
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
            ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID", multiplesModel.Prescription_Detail.InformationExamination_ID);
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
            ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID", prescription_Detail.InformationExamination_ID);
            return View(prescription_Detail);
        }

        // GET: Admin/Prescription_Detail/Edit/5
        public ActionResult Edit()
        {
            ViewBag.NameMedication = db.Medications.ToList();
            return PartialView("_Edit");
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
