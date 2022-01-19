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
    public class Nation1Controller : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Nation1
        public ActionResult Index()
        {
            return View(db.Nation1.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nation1 = db.Nation1.ToList();
            return Json(new { data = nation1 }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Nation1 nation1)
        {
            string text = "";
            var checkExist = db.Nation1.FirstOrDefault(e => e.Name == nation1.Name);
            if (checkExist != null && nation1.Name != null)
            {
                text = "Dân tộc đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Nation1 nation1)
        {
            string text = "";
            var checkExist = db.Nation1.FirstOrDefault(e => e.Name == nation1.Name);
            if (checkExist != null && checkExist.ID != nation1.ID && nation1.Name != null)
            {
                text = "Dân tộc đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/Nation1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nation1 nation1)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(nation1);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.Nation1.Add(nation1);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(nation1);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Nation1/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation1 nation1 = db.Nation1.Find(id);
            if (nation1 == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = nation1 }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Nation1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Nation1 nation1)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(nation1);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Nation1.Find(nation1.ID);
                    db.Entry(existData).CurrentValues.SetValues(nation1);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(nation1);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Nation1/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation1 nation1 = db.Nation1.Find(id);
            if (nation1 == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = nation1 }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Nation1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Nation1 nation1 = db.Nation1.Find(id);
            try
            {
                db.Nation1.Remove(nation1);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Dân tộc này đã được sử dụng. Bạn không thể xóa nó!" });
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
