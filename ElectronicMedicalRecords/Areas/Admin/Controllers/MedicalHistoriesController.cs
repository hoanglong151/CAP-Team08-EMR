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

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var medicalHistories = db.MedicalHistories.ToList();
            return Json(new { data = medicalHistories }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(MedicalHistory medicalHistory)
        {
            string text = "";
            var checkExist = db.MedicalHistories.FirstOrDefault(e => e.Name == medicalHistory.Name);
            if (checkExist != null && medicalHistory.Name != null)
            {
                text = "Bệnh Sử đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(MedicalHistory medicalHistory)
        {
            string text = "";
            var checkExist = db.MedicalHistories.FirstOrDefault(e => e.Name == medicalHistory.Name);
            if (checkExist != null && checkExist.ID != medicalHistory.ID && medicalHistory.Name != null)
            {
                text = "Bệnh Sử đã có trong danh sách";
            }
            return text;
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
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(medicalHistory);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    medicalHistory.ChiDinh = false;
                    db.MedicalHistories.Add(medicalHistory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medicalHistory);
            }
            return Json(new { success = false, responseText = text });
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
                medicalHistory.ChiDinh = item.Selected;
            }
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/MedicalHistories/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicalHistory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/MedicalHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MedicalHistory medicalHistory)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(medicalHistory);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.MedicalHistories.Find(medicalHistory.ID);
                    db.Entry(existData).CurrentValues.SetValues(medicalHistory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(medicalHistory);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/MedicalHistories/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = medicalHistory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/MedicalHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            try
            {
                db.MedicalHistories.Remove(medicalHistory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Bệnh Sử này đã được sử dụng. Bạn không thể xóa nó!" });
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
