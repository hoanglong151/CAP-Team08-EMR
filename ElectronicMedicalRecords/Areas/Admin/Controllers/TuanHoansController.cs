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
    public class TuanHoansController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/TuanHoans
        public ActionResult Index()
        {
            return View(db.TuanHoans.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailTuanHoan = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == id).ToList();
            var listTuanHoan = db.TuanHoans.ToList();
            foreach(var item in listDetailTuanHoan)
            {
                var changeSelect = listTuanHoan.FirstOrDefault(p => p.ID == item.TuanHoan_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.TuanHoan = listTuanHoan;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrTuanHoan(int id)
        {
            var listDetailTuanHoan = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailTuanHoan)
            {
                listOfStrings.Add("" + item.TuanHoan_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tuanHoans = db.TuanHoans.ToList();
            return Json(new { data = tuanHoans }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(TuanHoan tuanHoan)
        {
            string text = "";
            var checkExist = db.TuanHoans.FirstOrDefault(e => e.Name == tuanHoan.Name);
            if (checkExist != null && tuanHoan.Name != null)
            {
                text = "Tuần Hoàn đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(TuanHoan tuanHoan)
        {
            string text = "";
            var checkExist = db.TuanHoans.FirstOrDefault(e => e.Name == tuanHoan.Name);
            if (checkExist != null && checkExist.ID != tuanHoan.ID && tuanHoan.Name != null)
            {
                text = "Tuần Hoàn đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/CTMaus/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.TuanHoan = db.TuanHoans.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Wards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TuanHoan tuanHoan)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(tuanHoan);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    tuanHoan.ChiDinh = false;
                    db.TuanHoans.Add(tuanHoan);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tuanHoan);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TuanHoans/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TuanHoan tuanHoan = db.TuanHoans.Find(id);
            if (tuanHoan == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tuanHoan }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TuanHoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TuanHoan tuanHoan)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(tuanHoan);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.TuanHoans.Find(tuanHoan.ID);
                    db.Entry(existData).CurrentValues.SetValues(tuanHoan);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tuanHoan);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/TuanHoans/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TuanHoan tuanHoans = db.TuanHoans.Find(id);
            if (tuanHoans == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tuanHoans }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/TuanHoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TuanHoan tuanHoan = db.TuanHoans.Find(id);
            try
            {
                db.TuanHoans.Remove(tuanHoan);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tuần Hoàn này đã được sử dụng. Bạn không thể xóa nó!" });
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
