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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class ViSinhsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/ViSinhs
        public ActionResult Index()
        {
            return View(db.ViSinhs.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var viSinhs = db.ViSinhs.ToList();
            return Json(new { data = viSinhs }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/ViSinhs/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.ViSinh = db.ViSinhs.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/ViSinhs/Edit/5
        public ActionResult Edit(int id)
        {
            ViSinh viSinh = db.ViSinhs.Find(id);
            if (viSinh == null)
            {
                return HttpNotFound();
            }
            var ViSinh = new { viSinh.ID, viSinh.NameTest, viSinh.Price, viSinh.ChiDinh };
            return Json(new { data = ViSinh }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ViSinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(ViSinh viSinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viSinh).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, responseText = "Không thể cập nhật giá" });
        }

        // POST: Admin/ViSinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViSinh viSinh = db.ViSinhs.Find(id);
            db.ViSinhs.Remove(viSinh);
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
