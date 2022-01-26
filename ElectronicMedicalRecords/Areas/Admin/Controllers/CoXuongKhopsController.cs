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
    public class CoXuongKhopsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/CoXuongKhops
        public ActionResult Index()
        {
            return View(db.CoXuongKhops.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var coXuongKhops = db.CoXuongKhops.ToList();
            return Json(new { data = coXuongKhops }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(CoXuongKhop coXuongKhop)
        {
            string text = "";
            var checkExist = db.CoXuongKhops.FirstOrDefault(e => e.Name == coXuongKhop.Name);
            if (checkExist != null && coXuongKhop.Name != null)
            {
                text = "Cơ Xương Khớp đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(CoXuongKhop coXuongKhop)
        {
            string text = "";
            var checkExist = db.CoXuongKhops.FirstOrDefault(e => e.Name == coXuongKhop.Name);
            if (checkExist != null && checkExist.ID != coXuongKhop.ID && coXuongKhop.Name != null)
            {
                text = "Cơ Xương Khớp đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/CoXuongKhop/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.CoXuongKhop = db.CoXuongKhops.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/CoXuongKhops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoXuongKhop coXuongKhop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(coXuongKhop);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    coXuongKhop.ChiDinh = false;
                    db.CoXuongKhops.Add(coXuongKhop);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(coXuongKhop);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/CoXuongKhops/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            CoXuongKhop coXuongKhop = db.CoXuongKhops.Find(id);
            if (coXuongKhop == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = coXuongKhop }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/CoXuongKhops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoXuongKhop coXuongKhop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(coXuongKhop);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.CoXuongKhops.Find(coXuongKhop.ID);
                    db.Entry(existData).CurrentValues.SetValues(coXuongKhop);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(coXuongKhop);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/CoXuongKhops/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            CoXuongKhop coXuongKhop = db.CoXuongKhops.Find(id);
            if (coXuongKhop == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = coXuongKhop }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/CoXuongKhops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            CoXuongKhop coXuongKhop = db.CoXuongKhops.Find(id);
            try
            {
                db.CoXuongKhops.Remove(coXuongKhop);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Cơ Xương Khớp này đã được sử dụng. Bạn không thể xóa nó!" });
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
