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
    public class RangHamMatsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/RangHamMats
        public ActionResult Index()
        {
            return View(db.RangHamMats.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var rangHamMats = db.RangHamMats.ToList();
            return Json(new { data = rangHamMats }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(RangHamMat rangHamMat)
        {
            string text = "";
            var checkExist = db.RangHamMats.FirstOrDefault(e => e.Name == rangHamMat.Name);
            if (checkExist != null && rangHamMat.Name != null)
            {
                text = "Răng Hàm Mặt đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(RangHamMat rangHamMat)
        {
            string text = "";
            var checkExist = db.RangHamMats.FirstOrDefault(e => e.Name == rangHamMat.Name);
            if (checkExist != null && checkExist.ID != rangHamMat.ID && rangHamMat.Name != null)
            {
                text = "Răng Hàm Mặt đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/RangHamMats/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Clinical clinical = new Clinical();
            multiplesModel.Clinical = clinical;
            multiplesModel.RangHamMat = db.RangHamMats.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/RangHamMats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RangHamMat rangHamMat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(rangHamMat);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    rangHamMat.ChiDinh = false;
                    db.RangHamMats.Add(rangHamMat);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(rangHamMat);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/RangHamMats/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            RangHamMat rangHamMat = db.RangHamMats.Find(id);
            if (rangHamMat == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = rangHamMat }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/RangHamMats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RangHamMat rangHamMat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(rangHamMat);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.RangHamMats.Find(rangHamMat.ID);
                    db.Entry(existData).CurrentValues.SetValues(rangHamMat);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(rangHamMat);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/RangHamMats/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            RangHamMat rangHamMat = db.RangHamMats.Find(id);
            if (rangHamMat == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = rangHamMat }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/RangHamMats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            RangHamMat rangHamMat = db.RangHamMats.Find(id);
            try
            {
                db.RangHamMats.Remove(rangHamMat);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Răng Hàm Mặt này đã được sử dụng. Bạn không thể xóa nó!" });
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
