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
    public class HongsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Hongs
        public ActionResult Index()
        {
            return View(db.Hongs.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailHong = db.Detail_Hong.Where(p => p.InformationExamination_ID == id).ToList();
            var listHong = db.Hongs.ToList();
            foreach (var item in listDetailHong)
            {
                var changeSelect = listHong.FirstOrDefault(p => p.ID == item.Hong_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.Hong = listHong;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrHong(int id)
        {
            var listDetailHong = db.Detail_Hong.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailHong)
            {
                listOfStrings.Add("" + item.Hong_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hongs = db.Hongs.ToList();
            return Json(new { data = hongs }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Hong hong)
        {
            string text = "";
            var checkExist = db.Hongs.FirstOrDefault(e => e.Name == hong.Name);
            if (checkExist != null && hong.Name != null)
            {
                text = "Họng đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Hong hong)
        {
            string text = "";
            var checkExist = db.Hongs.FirstOrDefault(e => e.Name == hong.Name);
            if (checkExist != null && checkExist.ID != hong.ID && hong.Name != null)
            {
                text = "Họng đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/NgoaiKhoas/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.Hong = db.Hongs.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Hongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hong hong)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(hong);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    hong.ChiDinh = false;
                    db.Hongs.Add(hong);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(hong);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Hongs/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Hong hong = db.Hongs.Find(id);
            if (hong == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = hong }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Hongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Hong hong)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(hong);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Hongs.Find(hong.ID);
                    db.Entry(existData).CurrentValues.SetValues(hong);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(hong);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Hongs/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Hong hong = db.Hongs.Find(id);
            if (hong == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = hong }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Hongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Hong hong = db.Hongs.Find(id);
            try
            {
                db.Hongs.Remove(hong);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Họng này đã được sử dụng. Bạn không thể xóa nó!" });
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
