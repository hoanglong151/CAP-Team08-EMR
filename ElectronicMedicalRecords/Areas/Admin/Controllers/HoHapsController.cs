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
    public class HoHapsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/HoHaps
        public ActionResult Index()
        {
            return View(db.HoHaps.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hoHaps = db.HoHaps.ToList();
            return Json(new { data = hoHaps }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(HoHap hoHap)
        {
            string text = "";
            var checkExist = db.HoHaps.FirstOrDefault(e => e.Name == hoHap.Name);
            if (checkExist != null && hoHap.Name != null)
            {
                text = "Hô Hấp đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(HoHap hoHap)
        {
            string text = "";
            var checkExist = db.HoHaps.FirstOrDefault(e => e.Name == hoHap.Name);
            if (checkExist != null && checkExist.ID != hoHap.ID && hoHap.Name != null)
            {
                text = "Hô Hấp đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/CTMaus/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.HoHap = db.HoHaps.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/HoHaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HoHap hoHap)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(hoHap);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    hoHap.ChiDinh = false;
                    db.HoHaps.Add(hoHap);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(hoHap);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HoHaps/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HoHap hoHap = db.HoHaps.Find(id);
            if (hoHap == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = hoHap }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HoHaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoHap hoHap)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(hoHap);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.HoHaps.Find(hoHap.ID);
                    db.Entry(existData).CurrentValues.SetValues(hoHap);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(hoHap);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HoHaps/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HoHap hoHap = db.HoHaps.Find(id);
            if (hoHap == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = hoHap }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HoHaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HoHap hoHap = db.HoHaps.Find(id);
            try
            {
                db.HoHaps.Remove(hoHap);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Hô Hấp này đã được sử dụng. Bạn không thể xóa nó!" });
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
