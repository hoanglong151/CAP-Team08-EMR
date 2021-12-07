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
            var prescription_Detail = db.Prescription_Detail.Include(p => p.Medication).Include(p => p.Prescription);
            return View(prescription_Detail.ToList());
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

        //// GET: Admin/Prescription_Detail/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Medication_ID = new SelectList(db.Medications, "ID", "Name");
        //    ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID");
        //    return View();
        //}

        public ActionResult CreateOldPatient()
        {
            ViewBag.NameMedication = db.Medications.ToList();
            return PartialView("_CreateOldPatient");
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
            ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID", prescription_Detail.Precription__ID);
            return View(prescription_Detail);
        }

        // GET: Admin/Prescription_Detail/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Medication_ID = new SelectList(db.Medications, "ID", "Name", prescription_Detail.Medication_ID);
            ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID", prescription_Detail.Precription__ID);
            return View(prescription_Detail);
        }

        // POST: Admin/Prescription_Detail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NumMedication,Note,Medication_ID,Precription__ID")] Prescription_Detail prescription_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Medication_ID = new SelectList(db.Medications, "ID", "Name", prescription_Detail.Medication_ID);
            ViewBag.Precription__ID = new SelectList(db.Prescriptions, "ID", "ID", prescription_Detail.Precription__ID);
            return View(prescription_Detail);
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
