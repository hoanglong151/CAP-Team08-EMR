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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class PatientStatusController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/PatientStatus
        public ActionResult Index()
        {
            var patientStatus = db.PatientStatus.ToList();
            return View(patientStatus);
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var patientStatus = db.PatientStatus.ToList();
            return Json(new { data = patientStatus }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/PatientStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientStatu patientStatu)
        {
            var text = ValidateForm(patientStatu);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    db.PatientStatus.Add(patientStatu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(patientStatu);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/PatientStatus/Edit/5
        public ActionResult Edit(int id)
        {
            PatientStatu patientStatu = db.PatientStatus.Find(id);
            if (patientStatu == null)
            {
                return HttpNotFound();
            }
            var patientStatus = new { patientStatu.Name, patientStatu.ID };
            return Json(new { data = patientStatus }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/PatientStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientStatu patientStatu)
        {
            var text = ValidateFormUpdate(patientStatu);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.PatientStatus.Find(patientStatu.ID);
                    db.Entry(existData).CurrentValues.SetValues(patientStatu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(patientStatu);
            }  
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/PatientStatus/Delete/5
        public ActionResult Delete(int id)
        {
            PatientStatu patientStatu = db.PatientStatus.Find(id);
            if (patientStatu == null)
            {
                return HttpNotFound();
            }
            var patientStatusDel = new { patientStatu.Name, patientStatu.ID };
            return Json(new { data = patientStatusDel }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/PatientStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientStatu patientStatu = db.PatientStatus.Find(id);
            try
            {
                db.PatientStatus.Remove(patientStatu);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = "Tình Trạng này đang được sử dụng" });
            }

        }

        public string ValidateForm(PatientStatu patientStatu)
        {
            string text = "";
            var checkExist = db.PatientStatus.FirstOrDefault(e => e.Name == patientStatu.Name);
            if (checkExist != null && patientStatu.Name != null)
            {
                text = "Tình trạng bệnh nhân đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(PatientStatu patientStatu)
        {
            string text = "";
            var checkExist = db.PatientStatus.FirstOrDefault(e => e.Name == patientStatu.Name);
            if (checkExist != null && checkExist.ID != patientStatu.ID && patientStatu.Name != null)
            {
                text = "Tình trạng bệnh nhân đã có trong danh sách";
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
