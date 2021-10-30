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
    public class MedicationCategoriesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/MedicationCategories
        public ActionResult Index()
        {
            return View(db.Medications.ToList());
        }

        public ActionResult GetData()
        {
            var medication = db.Medications.ToList();
            return Json(new { data = medication }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/MedicationCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medicationCategory = db.Medications.Find(id);
            if (medicationCategory == null)
            {
                return HttpNotFound();
            }
            return View(medicationCategory);
        }

        // GET: Admin/MedicationCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MedicationCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medication medicationCategory)
        {
            if (ModelState.IsValid)
            {
                db.Medications.Add(medicationCategory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return View(medicationCategory);
        }

        // GET: Admin/MedicationCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medicationCategory = db.Medications.Find(id);
            if (medicationCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicationCategory }, JsonRequestBehavior.AllowGet);
            //return View(medicationCategory);
        }

        // POST: Admin/MedicationCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medication medicationCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicationCategory).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
                //return RedirectToAction("Index");
            }
            return View(medicationCategory);
        }

        // GET: Admin/MedicationCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medicationCategory = db.Medications.Find(id);
            if (medicationCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicationCategory }, JsonRequestBehavior.AllowGet);
            //return View(medicationCategory);
        }

        // POST: Admin/MedicationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medication medicationCategory = db.Medications.Find(id);
            db.Medications.Remove(medicationCategory);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json(new { success = true });
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
