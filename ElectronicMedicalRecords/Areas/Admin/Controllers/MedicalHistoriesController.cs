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
    public class MedicalHistoriesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/MedicalHistories
        public ActionResult Index()
        {
            return View(db.MedicalHistories.ToList());
        }

        // GET: Admin/MedicalHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }

        // GET: Admin/MedicalHistories/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.MedicalHistories = db.MedicalHistories.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/MedicalHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicalHistory medicalHistory)
        {
            if (ModelState.IsValid)
            {
                db.MedicalHistories.Add(medicalHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicalHistory);
        }

        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var medicalHistories = db.MedicalHistories.ToList();
            multiplesModel.MedicalHistories = medicalHistories;
            var details_medicalHistories1 = db.Detail_MedicalHistory.Where(p => p.Patient_ID == id);
            foreach(var item in details_medicalHistories1)
            {
                var medicalHistory = multiplesModel.MedicalHistories.FirstOrDefault(p => p.ID == item.MedicalHistory_ID);
                medicalHistory.Selected = item.Selected;
            }
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/MedicalHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }

        // POST: Admin/MedicalHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Selected,Dangerous")] MedicalHistory medicalHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicalHistory);
        }

        // GET: Admin/MedicalHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }

        // POST: Admin/MedicalHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            db.MedicalHistories.Remove(medicalHistory);
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
