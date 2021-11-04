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
    [Authorize]
    public class MedicationsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Medications
        public ActionResult Index()
        {
            ViewBag.NameHC = db.MedicationCategories.ToList();
            return View(db.Medications.Include(m => m.MedicationCategory).ToList());
        }

        public ActionResult GetData()
        {
            var medications = db.Medications.Include(m => m.MedicationCategory).ToList();
            var listMedication = medications.Select(s => new
            {
                ID = s.ID,
                Name = s.Name,
                Medication_ID = s.MedicationCategory.Name,
                Unit = s.Unit,
                Price = s.Price
            });
            return Json(new { data = listMedication }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Medications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = db.Medications.Find(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return View(medication);
        }

        // GET: Admin/Medications/Create
        public ActionResult Create()
        {
            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name");
            return View();
        }

        // POST: Admin/Medications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medication medication)
        {
            if (ModelState.IsValid)
            {
                db.Medications.Add(medication);
                db.SaveChanges();
                return Json(new { success = true });
                //return RedirectToAction("Index");
            }

            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name", medication.MedicationCategory_ID);
            return View(medication);
        }

        // GET: Admin/Medications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = db.Medications.Find(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name", medication.MedicationCategory_ID);
            var listMedication = new
            {
                ID = medication.ID,
                Name = medication.Name,
                Medication_Name = medication.MedicationCategory.Name,
                Medication_ID = medication.MedicationCategory_ID,
                Unit = medication.Unit,
                Price = medication.Price
            };
            return Json(new { data = listMedication }, JsonRequestBehavior.AllowGet);
            //return View(medication);
        }

        // POST: Admin/Medications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medication medication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name", medication.MedicationCategory_ID);
            return View(medication);
        }

        // GET: Admin/Medications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = db.Medications.Find(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            var newMedication = new
            {
                ID = medication.ID,
                Name = medication.Name,
                Medication_Name = medication.MedicationCategory.Name,
                Medication_ID = medication.MedicationCategory_ID,
                Unit = medication.Unit,
                Price = medication.Price
            };
            return Json(new { data = newMedication }, JsonRequestBehavior.AllowGet);
            //return View(medication);
        }

        // POST: Admin/Medications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medication medication = db.Medications.Find(id);
            db.Medications.Remove(medication);
            db.SaveChanges();
            return Json(new { success = true });
            //return RedirectToAction("Index");
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
