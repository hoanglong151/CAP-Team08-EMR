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
            var medicationCategories = db.MedicationCategories.ToList();
            return View(medicationCategories);
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var medicationCategories = db.MedicationCategories.ToList();
            return Json(new { data = medicationCategories }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/MedicationCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicationCategory medicationCategory)
        {
            var text = ValidateForm(medicationCategory);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.MedicationCategories.Add(medicationCategory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medicationCategory);
            }
            return Json(new { success = false, responseText = text});
        }

        // GET: Admin/MedicationCategories/Edit/5
        public ActionResult Edit(int id)
        {
            MedicationCategory medicationCategory = db.MedicationCategories.Find(id);
            if (medicationCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicationCategory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/MedicationCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MedicationCategory medicationCategory)
        {
            var text = ValidateFormUpdate(medicationCategory);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.MedicationCategories.Find(medicationCategory.ID);
                    db.Entry(existData).CurrentValues.SetValues(medicationCategory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medicationCategory);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/MedicationCategories/Delete/5
        public ActionResult Delete(int id)
        {
            MedicationCategory medicationCategory = db.MedicationCategories.Find(id);
            if (medicationCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicationCategory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/MedicationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicationCategory medicationCategory = db.MedicationCategories.Find(id);
            try
            {
                db.MedicationCategories.Remove(medicationCategory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = "Danh mục thuốc này đang được sử dụng" });
            }
        }

        public string ValidateForm(MedicationCategory medicationCategory)
        {
            string text = "";
            var checkExist = db.MedicationCategories.FirstOrDefault(e => e.Name == medicationCategory.Name);
            if (checkExist != null && medicationCategory.Name != null)
            {
                text = "Danh mục thuốc đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(MedicationCategory medicationCategory)
        {
            string text = "";
            var checkExist = db.MedicationCategories.FirstOrDefault(e => e.Name == medicationCategory.Name);
            if (checkExist != null && checkExist.ID != medicationCategory.ID && medicationCategory.Name != null)
            {
                text = "Danh mục thuốc đã có trong danh sách";
            }
            return text;
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
