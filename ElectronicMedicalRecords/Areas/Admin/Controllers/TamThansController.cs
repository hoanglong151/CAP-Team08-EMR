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
    public class TamThansController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/TamThans
        public ActionResult Index()
        {
            return View(db.TamThans.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tamThans = db.TamThans.ToList();
            return Json(new { data = tamThans }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(TamThan tamThan)
        {
            string text = "";
            var checkExist = db.TieuHoas.FirstOrDefault(e => e.Name == tamThan.Name);
            if (checkExist != null && tamThan.Name != null)
            {
                text = "Tâm Thần đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(TamThan tamThan)
        {
            string text = "";
            var checkExist = db.TamThans.FirstOrDefault(e => e.Name == tamThan.Name);
            if (checkExist != null && checkExist.ID != tamThan.ID && tamThan.Name != null)
            {
                text = "Tâm Thần đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/TamThans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TamThan tamThan)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(tamThan);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    tamThan.ChiDinh = false;
                    db.TamThans.Add(tamThan);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tamThan);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TamThans/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TamThan tamThan = db.TamThans.Find(id);
            if (tamThan == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tamThan }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TamThans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TamThan tamThan)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(tamThan);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.TamThans.Find(tamThan.ID);
                    db.Entry(existData).CurrentValues.SetValues(tamThan);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tamThan);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TamThans/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TamThan tamThan = db.TamThans.Find(id);
            if (tamThan == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tamThan }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TamThans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TamThan tamThan = db.TamThans.Find(id);
            try
            {
                db.TamThans.Remove(tamThan);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tâm Thần này đã được sử dụng. Bạn không thể xóa nó!" });
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
