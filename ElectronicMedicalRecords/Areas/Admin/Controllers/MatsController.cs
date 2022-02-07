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
    public class MatsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Mats
        public ActionResult Index()
        {
            return View(db.Mats.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailMat = db.Detail_Mat.Where(p => p.InformationExamination_ID == id).ToList();
            var listMat = db.Mats.ToList();
            foreach (var item in listDetailMat)
            {
                var changeSelect = listMat.FirstOrDefault(p => p.ID == item.Mat_ID);
                changeSelect.ChiDinh = true;
            }
            Clinical clinical = new Clinical();
            clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            multiplesModel.Clinical = clinical;
            multiplesModel.Mat = listMat;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrMat(int id)
        {
            var listDetailMat = db.Detail_Mat.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailMat)
            {
                listOfStrings.Add("" + item.Mat_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var mats = db.Mats.ToList();
            return Json(new { data = mats }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Mat mat)
        {
            string text = "";
            var checkExist = db.Mats.FirstOrDefault(e => e.Name == mat.Name);
            if (checkExist != null && mat.Name != null)
            {
                text = "Mắt đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Mat mat)
        {
            string text = "";
            var checkExist = db.Mats.FirstOrDefault(e => e.Name == mat.Name);
            if (checkExist != null && checkExist.ID != mat.ID && mat.Name != null)
            {
                text = "Mắt đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/Mats/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Clinical clinical = new Clinical();
            multiplesModel.Clinical = clinical;
            multiplesModel.Mat = db.Mats.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Mats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mat mat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(mat);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    mat.ChiDinh = false;
                    db.Mats.Add(mat);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(mat);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Mats/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mat mat = db.Mats.Find(id);
            if (mat == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = mat }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Mats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mat mat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(mat);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Mats.Find(mat.ID);
                    db.Entry(existData).CurrentValues.SetValues(mat);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(mat);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Mats/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mat mat = db.Mats.Find(id);
            if (mat == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = mat }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Mats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mat mat = db.Mats.Find(id);
            try
            {
                db.Mats.Remove(mat);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Mắt này đã được sử dụng. Bạn không thể xóa nó!" });
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
