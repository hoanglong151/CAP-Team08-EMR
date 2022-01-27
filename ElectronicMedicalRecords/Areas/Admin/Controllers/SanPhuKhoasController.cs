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
    public class SanPhuKhoasController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/SanPhuKhoas
        public ActionResult Index()
        {
            return View(db.SanPhuKhoas.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var sanPhuKhoas = db.SanPhuKhoas.ToList();
            return Json(new { data = sanPhuKhoas }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(SanPhuKhoa sanPhuKhoa)
        {
            string text = "";
            var checkExist = db.SanPhuKhoas.FirstOrDefault(e => e.Name == sanPhuKhoa.Name);
            if (checkExist != null && sanPhuKhoa.Name != null)
            {
                text = "Sản Phụ Khoa đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(SanPhuKhoa sanPhuKhoa)
        {
            string text = "";
            var checkExist = db.SanPhuKhoas.FirstOrDefault(e => e.Name == sanPhuKhoa.Name);
            if (checkExist != null && checkExist.ID != sanPhuKhoa.ID && sanPhuKhoa.Name != null)
            {
                text = "Sản Phụ Khoa đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/SanPhuKhoas/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.SanPhuKhoa = db.SanPhuKhoas.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/SanPhuKhoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPhuKhoa sanPhuKhoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(sanPhuKhoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    sanPhuKhoa.ChiDinh = false;
                    db.SanPhuKhoas.Add(sanPhuKhoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(sanPhuKhoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/SanPhuKhoas/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            SanPhuKhoa sanPhuKhoa = db.SanPhuKhoas.Find(id);
            if (sanPhuKhoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = sanPhuKhoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/SanPhuKhoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPhuKhoa sanPhuKhoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(sanPhuKhoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.SanPhuKhoas.Find(sanPhuKhoa.ID);
                    db.Entry(existData).CurrentValues.SetValues(sanPhuKhoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(sanPhuKhoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/SanPhuKhoas/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            SanPhuKhoa sanPhuKhoa = db.SanPhuKhoas.Find(id);
            if (sanPhuKhoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = sanPhuKhoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/SanPhuKhoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            SanPhuKhoa sanPhuKhoa = db.SanPhuKhoas.Find(id);
            try
            {
                db.SanPhuKhoas.Remove(sanPhuKhoa);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Sản Phụ Khoa này đã được sử dụng. Bạn không thể xóa nó!" });
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
