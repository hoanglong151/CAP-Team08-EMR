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
    public class NgoaiKhoasController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/NgoaiKhoas
        public ActionResult Index()
        {
            return View(db.NgoaiKhoas.ToList());
        }

        public ActionResult EditSelect(MultiplesModel multiplesModel)
        {
            var listDetailNgoaiKhoa = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            var listNgoaiKhoa = db.NgoaiKhoas.AsNoTracking().ToList();
            foreach (var item in listDetailNgoaiKhoa)
            {
                var changeSelect = listNgoaiKhoa.FirstOrDefault(p => p.ID == item.NgoaiKhoa_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.NgoaiKhoa = listNgoaiKhoa;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrNgoaiKhoa(int id)
        {
            var listDetailNgoaiKhoa = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailNgoaiKhoa)
            {
                listOfStrings.Add("" + item.NgoaiKhoa_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ngoaiKhoas = db.NgoaiKhoas.ToList();
            return Json(new { data = ngoaiKhoas }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(NgoaiKhoa ngoaiKhoa)
        {
            string text = "";
            var checkExist = db.NgoaiKhoas.FirstOrDefault(e => e.Name == ngoaiKhoa.Name);
            if (checkExist != null && ngoaiKhoa.Name != null)
            {
                text = "Ngoại Khoa đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(NgoaiKhoa ngoaiKhoa)
        {
            string text = "";
            var checkExist = db.NgoaiKhoas.FirstOrDefault(e => e.Name == ngoaiKhoa.Name);
            if (checkExist != null && checkExist.ID != ngoaiKhoa.ID && ngoaiKhoa.Name != null)
            {
                text = "Ngoại Khoa đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/NgoaiKhoas/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.NgoaiKhoa = db.NgoaiKhoas.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/NgoaiKhoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NgoaiKhoa ngoaiKhoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(ngoaiKhoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    ngoaiKhoa.ChiDinh = false;
                    db.NgoaiKhoas.Add(ngoaiKhoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(ngoaiKhoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/NgoaiKhoas/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            NgoaiKhoa ngoaiKhoa = db.NgoaiKhoas.Find(id);
            if (ngoaiKhoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = ngoaiKhoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/NgoaiKhoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NgoaiKhoa ngoaiKhoa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(ngoaiKhoa);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.NgoaiKhoas.Find(ngoaiKhoa.ID);
                    db.Entry(existData).CurrentValues.SetValues(ngoaiKhoa);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(ngoaiKhoa);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/NgoaiKhoas/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            NgoaiKhoa ngoaiKhoa = db.NgoaiKhoas.Find(id);
            if (ngoaiKhoa == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = ngoaiKhoa }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/NgoaiKhoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            NgoaiKhoa ngoaiKhoa = db.NgoaiKhoas.Find(id);
            try
            {
                db.NgoaiKhoas.Remove(ngoaiKhoa);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ngoại Khoa này đã được sử dụng. Bạn không thể xóa nó!" });
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
