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
    public class NationsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Nations
        public ActionResult Index()
        {
            return View(db.Nations.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nations = db.Nations.ToList();
            return Json(new { data = nations }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Nation nation)
        {
            string text = "";
            var checkExist = db.Nations.FirstOrDefault(e => e.Name == nation.Name);
            if (checkExist != null && nation.Name != null)
            {
                text = "Quốc gia đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Nation nation)
        {
            string text = "";
            var checkExist = db.Nations.FirstOrDefault(e => e.Name == nation.Name);
            if (checkExist != null && checkExist.ID != nation.ID && nation.Name != null)
            {
                text = "Quốc gia đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/Nations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nation nation)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(nation);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.Nations.Add(nation);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(nation);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Nations/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation nation = db.Nations.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = nation }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Nations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Nation nation)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(nation);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Nations.Find(nation.ID);
                    db.Entry(existData).CurrentValues.SetValues(nation);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(nation);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Nations/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation nation = db.Nations.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = nation }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Nations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation nation = db.Nations.Find(id);
            try
            {
                db.Nations.Remove(nation);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Quốc gia này đã được sử dụng. Bạn không thể xóa nó!" });
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
