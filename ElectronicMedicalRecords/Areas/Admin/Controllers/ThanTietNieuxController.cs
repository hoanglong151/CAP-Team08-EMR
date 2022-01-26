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
    public class ThanTietNieuxController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/ThanTietNieux
        public ActionResult Index()
        {
            return View(db.ThanTietNieux.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var thanTietNieus = db.ThanTietNieux.ToList();
            return Json(new { data = thanTietNieus }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(ThanTietNieu thanTietNieu)
        {
            string text = "";
            var checkExist = db.ThanTietNieux.FirstOrDefault(e => e.Name == thanTietNieu.Name);
            if (checkExist != null && thanTietNieu.Name != null)
            {
                text = "Thận Tiết Niệu đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(ThanTietNieu thanTietNieu)
        {
            string text = "";
            var checkExist = db.ThanTietNieux.FirstOrDefault(e => e.Name == thanTietNieu.Name);
            if (checkExist != null && checkExist.ID != thanTietNieu.ID && thanTietNieu.Name != null)
            {
                text = "Thận Tiết Niệu đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/CTMaus/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.ThanTietNieu = db.ThanTietNieux.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/ThanTietNieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThanTietNieu thanTietNieu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(thanTietNieu);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    thanTietNieu.ChiDinh = false;
                    db.ThanTietNieux.Add(thanTietNieu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(thanTietNieu);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/ThanTietNieux/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanTietNieu thanTietNieu = db.ThanTietNieux.Find(id);
            if (thanTietNieu == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = thanTietNieu }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ThanTietNieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThanTietNieu thanTietNieu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(thanTietNieu);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.ThanTietNieux.Find(thanTietNieu.ID);
                    db.Entry(existData).CurrentValues.SetValues(thanTietNieu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(thanTietNieu);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/ThanTietNieux/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanTietNieu thanTietNieu = db.ThanTietNieux.Find(id);
            if (thanTietNieu == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = thanTietNieu }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ThanTietNieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanTietNieu thanTietNieu = db.ThanTietNieux.Find(id);
            try
            {
                db.ThanTietNieux.Remove(thanTietNieu);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Thận Tiết Niệu này đã được sử dụng. Bạn không thể xóa nó!" });
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
