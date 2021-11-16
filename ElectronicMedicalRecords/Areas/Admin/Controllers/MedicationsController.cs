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
            }).ToList();
            return Json(new { data = listMedication }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FindMedication(int? id)
        {
            var medication = db.Medications.FirstOrDefault(p => p.ID == id);
            return Json(new { data = medication.Unit }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Medications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medication medication)
        {
            var text = Validation(medication);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    db.Medications.Add(medication);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medication);
            }
            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name", medication.MedicationCategory_ID);
            return Json(new { success = false, responseText = text });
        }

        public string Validation(Medication medcation)
        {
            string text = "";
            var checkExist = db.Medications.FirstOrDefault(e => e.Name == medcation.Name);
            if(checkExist != null && medcation.Name != null && medcation.MedicationCategory_ID != 0 && medcation.Price != null && medcation.Unit != null)
            {
                text = "Tên thuốc đã có trong danh sách";
            }
            return text;
        }

        public string ValidationUpdate(Medication medcation)
        {
            string text = "";
            var checkExist = db.Medications.FirstOrDefault(e => e.Name == medcation.Name);
            if (checkExist != null && checkExist.ID != medcation.ID && medcation.Name != null && medcation.MedicationCategory_ID != 0 && medcation.Price != null && medcation.Unit != null)
            {
                text = "Tên thuốc đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/Medications/Edit/5
        public ActionResult Edit(int id)
        {
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
        }

        // POST: Admin/Medications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medication medication)
        {
            var text = ValidationUpdate(medication);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Medications.Find(medication.ID);
                    db.Entry(existData).CurrentValues.SetValues(medication);
                    //db.Entry(medication).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medication);
            }
            ViewBag.MedicationCategory_ID = new SelectList(db.MedicationCategories, "ID", "Name", medication.MedicationCategory_ID);
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Medications/Delete/5
        public ActionResult Delete(int id)
        {
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
        }

        // POST: Admin/Medications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medication medication = db.Medications.Find(id);
            try
            {
                db.Medications.Remove(medication);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = "Thuốc này đang được sử dụng trong toa thuốc" });
            }
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
