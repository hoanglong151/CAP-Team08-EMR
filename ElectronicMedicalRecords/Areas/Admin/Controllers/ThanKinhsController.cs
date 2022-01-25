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
    public class ThanKinhsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/ThanKinhs
        public ActionResult Index()
        {
            return View(db.ThanKinhs.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var thanKinhs = db.ThanKinhs.ToList();
            return Json(new { data = thanKinhs }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(ThanKinh thanKinh)
        {
            string text = "";
            var checkExist = db.CoXuongKhops.FirstOrDefault(e => e.Name == thanKinh.Name);
            if (checkExist != null && thanKinh.Name != null)
            {
                text = "Thần Kinh đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(ThanKinh thanKinh)
        {
            string text = "";
            var checkExist = db.ThanKinhs.FirstOrDefault(e => e.Name == thanKinh.Name);
            if (checkExist != null && checkExist.ID != thanKinh.ID && thanKinh.Name != null)
            {
                text = "Thần Kinh đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/ThanKinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThanKinh thanKinh)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(thanKinh);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    thanKinh.ChiDinh = false;
                    db.ThanKinhs.Add(thanKinh);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(thanKinh);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/ThanKinhs/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanKinh thanKinh = db.ThanKinhs.Find(id);
            if (thanKinh == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = thanKinh }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ThanKinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThanKinh thanKinh)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(thanKinh);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.ThanKinhs.Find(thanKinh.ID);
                    db.Entry(existData).CurrentValues.SetValues(thanKinh);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(thanKinh);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/ThanKinhs/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanKinh thanKinh = db.ThanKinhs.Find(id);
            if (thanKinh == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = thanKinh }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ThanKinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ThanKinh thanKinh = db.ThanKinhs.Find(id);
            try
            {
                db.ThanKinhs.Remove(thanKinh);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Thần Kinh này đã được sử dụng. Bạn không thể xóa nó!" });
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
