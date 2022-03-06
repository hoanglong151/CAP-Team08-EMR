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
    public class DistrictsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Districts
        public ActionResult Index()
        {
            ViewBag.Hometown = db.HomeTowns.ToList();
            return View(db.Districts.ToList());
        }

        [HttpPost]
        public JsonResult filterDistricts(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listDistrics = db.Districts.Where(p => p.HomeTown_ID == id).ToList();
            return Json(new { data = listDistrics });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var homeTown = db.HomeTowns.FirstOrDefault();
            var districts = db.Districts.Include(h => h.HomeTown).ToList();
            var listDistricts = districts.Select(s => new
            {
                ID = s.ID,
                District1 = s.District1,
                HomeTown_ID = s.HomeTown.HomeTown1,
            }).ToList();
            return Json(new { data = listDistricts }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(District district)
        {
            string text = "";
            var checkExist = db.Districts.FirstOrDefault(e => e.District1 == district.District1 && e.HomeTown_ID == district.HomeTown_ID);
            if (checkExist != null && district.District1 != null)
            {
                text = "Quận/Huyện đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(District district)
        {
            string text = "";
            var checkExist = db.Districts.FirstOrDefault(e => e.District1 == district.District1 && e.HomeTown_ID == district.HomeTown_ID);
            if (checkExist != null && checkExist.ID != district.ID && district.District1 != null)
            {
                text = "Quận/Huyện đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/Districts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(District district)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(district);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.Districts.Add(district);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(district);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Districts/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            District district = db.Districts.Find(id);
            district.HomeTown = db.HomeTowns.FirstOrDefault(p => p.ID == district.HomeTown_ID);
            var getDistrict = new
            {
                ID = district.ID,
                District1 = district.District1,
                HomeTown = district.HomeTown.HomeTown1,
                HomeTown_ID = district.HomeTown_ID
            };
            return Json(new { data = getDistrict }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Districts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(District district)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(district);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Districts.Find(district.ID);
                    db.Entry(existData).CurrentValues.SetValues(district);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(district);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Districts/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = district }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            District district = db.Districts.Find(id);
            try
            {
                db.Districts.Remove(district);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Quận/Huyện này đã được sử dụng. Bạn không thể xóa nó!" });
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
