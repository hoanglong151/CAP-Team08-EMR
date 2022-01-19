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
    public class HomeTownsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/HomeTowns
        public ActionResult Index()
        {
            return View(db.HomeTowns.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var homeTowns = db.HomeTowns.ToList();
            return Json(new { data = homeTowns }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(HomeTown homeTown)
        {
            string text = "";
            var checkExist = db.HomeTowns.FirstOrDefault(e => e.HomeTown1 == homeTown.HomeTown1);
            if (checkExist != null && homeTown.HomeTown1 != null)
            {
                text = "Tỉnh/Thành đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(HomeTown homeTown)
        {
            string text = "";
            var checkExist = db.HomeTowns.FirstOrDefault(e => e.HomeTown1 == homeTown.HomeTown1);
            if (checkExist != null && checkExist.ID != homeTown.ID && homeTown.HomeTown1 != null)
            {
                text = "Tỉnh/Thành đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/HomeTowns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HomeTown homeTown)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(homeTown);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.HomeTowns.Add(homeTown);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(homeTown);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HomeTowns/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HomeTown homeTown = db.HomeTowns.Find(id);
            if (homeTown == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = homeTown }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HomeTowns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HomeTown homeTown)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(homeTown);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.HomeTowns.Find(homeTown.ID);
                    db.Entry(existData).CurrentValues.SetValues(homeTown);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(homeTown);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HomeTowns/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HomeTown homeTown = db.HomeTowns.Find(id);
            if (homeTown == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = homeTown }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HomeTowns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HomeTown homeTown = db.HomeTowns.Find(id);
            try
            {
                db.HomeTowns.Remove(homeTown);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tỉnh/Thành này đã được sử dụng. Bạn không thể xóa nó!" });
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
