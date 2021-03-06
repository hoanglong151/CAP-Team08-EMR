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
    public class WardsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Wards
        public ActionResult Index()
        {
            ViewBag.Districts = db.Districts.ToList();
            ViewBag.HomeTown = db.HomeTowns.ToList();
            return View(db.Wards.ToList());
        }

        [HttpPost]
        public JsonResult filterWards(int id, int homeTown)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listWards = db.Wards.Where(p => p.District_ID == id && p.HomeTown_ID == homeTown).ToList();
            return Json(new { data = listWards });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var wards = db.Wards.Include(d => d.District).Include(h => h.HomeTown).ToList();
            var listWards = wards.Select(s => new
            {
                ID = s.ID,
                Ward1 = s.Ward1,
                District_ID = s.District.District1,
                HomeTown_ID = s.HomeTown.HomeTown1,
            }).ToList();
            return Json(new { data = listWards }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Ward ward)
        {
            string text = "";
            var checkExist = db.Wards.FirstOrDefault(e => e.Ward1 == ward.Ward1 && e.District_ID == ward.District_ID && e.HomeTown_ID == ward.HomeTown_ID);
            if (checkExist != null && ward.Ward1 != null)
            {
                text = "Phường/Xã đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Ward ward)
        {
            string text = "";
            var checkExist = db.Wards.FirstOrDefault(e => e.Ward1 == ward.Ward1 && e.District_ID == ward.District_ID && e.HomeTown_ID == ward.HomeTown_ID);
            if (checkExist != null && checkExist.ID != ward.ID && ward.Ward1 != null)
            {
                text = "Phường/Xã đã có trong danh sách";
            }
            return text;
        }

        // POST: Admin/Wards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ward ward)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(ward);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.Wards.Add(ward);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(ward);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Wards/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Ward ward = db.Wards.Find(id);
            ward.District = db.Districts.FirstOrDefault(p => p.ID == ward.District_ID);
            ward.HomeTown = db.HomeTowns.FirstOrDefault(p => p.ID == ward.HomeTown_ID);
            var getDistrict = new
            {
                ID = ward.ID,
                Ward1 = ward.Ward1,
                District = ward.District.District1,
                District_ID = ward.District_ID,
                HomeTown_ID = ward.HomeTown.HomeTown1,
                HomeTown = ward.HomeTown_ID
            };
            return Json(new { data = getDistrict }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Wards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ward ward)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(ward);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Wards.Find(ward.ID);
                    db.Entry(existData).CurrentValues.SetValues(ward);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(ward);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Wards/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Ward ward = db.Wards.Find(id);
            ward.District = db.Districts.FirstOrDefault(p => p.ID == ward.District_ID);
            ward.HomeTown = db.HomeTowns.FirstOrDefault(p => p.ID == ward.HomeTown_ID);
            var getWard = new
            {
                ID = ward.ID,
                Ward1 = ward.Ward1,
                District = ward.District.District1,
                District_ID = ward.District_ID,
                HomeTown_ID = ward.HomeTown.HomeTown1,
                HomeTown = ward.HomeTown_ID
            };
            return Json(new { data = getWard }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Wards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Ward ward = db.Wards.Find(id);
            try
            {
                db.Wards.Remove(ward);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Phường/Xã này đã được sử dụng. Bạn không thể xóa nó!" });
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
