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
    public class TieuHoasController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/TieuHoas
        public ActionResult Index()
        {
            return View(db.TieuHoas.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tieuHoas = db.TieuHoas.ToList();
            return Json(new { data = tieuHoas }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(TieuHoa tieuHoa)
        {
            string text = "";
            var checkExist = db.TieuHoas.FirstOrDefault(e => e.Name == tieuHoa.Name);
            if (checkExist != null && tieuHoa.Name != null)
            {
                text = "Tiêu Hóa đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(TieuHoa tieuHoa)
        {
            string text = "";
            var checkExist = db.TieuHoas.FirstOrDefault(e => e.Name == tieuHoa.Name);
            if (checkExist != null && checkExist.ID != tieuHoa.ID && tieuHoa.Name != null)
            {
                text = "Tiêu Hóa đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/TieuHoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TieuHoa tieuHoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(tieuHoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    tieuHoa.ChiDinh = false;
                    db.TieuHoas.Add(tieuHoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tieuHoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TieuHoas/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TieuHoa tieuHoa = db.TieuHoas.Find(id);
            if (tieuHoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tieuHoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TieuHoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TieuHoa tieuHoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(tieuHoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.TieuHoas.Find(tieuHoa.ID);
                    db.Entry(existData).CurrentValues.SetValues(tieuHoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tieuHoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TieuHoas/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TieuHoa tieuHoa = db.TieuHoas.Find(id);
            if (tieuHoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tieuHoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TieuHoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TieuHoa tieuHoa = db.TieuHoas.Find(id);
            try
            {
                db.TieuHoas.Remove(tieuHoa);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tiêu Hóa này đã được sử dụng. Bạn không thể xóa nó!" });
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
