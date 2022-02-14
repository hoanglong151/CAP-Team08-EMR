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
    public class TaisController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Tais
        public ActionResult Index()
        {
            return View(db.Tais.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailTai = db.Detail_Tai.Where(p => p.InformationExamination_ID == id).ToList();
            var listTai = db.Tais.ToList();
            foreach (var item in listDetailTai)
            {
                var changeSelect = listTai.FirstOrDefault(p => p.ID == item.Tai_ID);
                changeSelect.ChiDinh = true;
            }
            Clinical clinical = new Clinical();
            clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            multiplesModel.Clinical = clinical;
            multiplesModel.Tai = listTai;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrTai(int id)
        {
            var listDetailTai = db.Detail_Tai.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailTai)
            {
                listOfStrings.Add("" + item.Tai_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tais = db.Tais.ToList();
            return Json(new { data = tais }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Tai tai)
        {
            string text = "";
            var checkExist = db.Tais.FirstOrDefault(e => e.Name == tai.Name);
            if (checkExist != null && tai.Name != null)
            {
                text = "Tai đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Tai tai)
        {
            string text = "";
            var checkExist = db.Tais.FirstOrDefault(e => e.Name == tai.Name);
            if (checkExist != null && checkExist.ID != tai.ID && tai.Name != null)
            {
                text = "Tai đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/Tais/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.Tai = db.Tais.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Tais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tai tai)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(tai);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    tai.ChiDinh = false;
                    db.Tais.Add(tai);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tai);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Tais/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Tai tai = db.Tais.Find(id);
            if (tai == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tai }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Tais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tai tai)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(tai);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Tais.Find(tai.ID);
                    db.Entry(existData).CurrentValues.SetValues(tai);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(tai);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Tais/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Tai tai = db.Tais.Find(id);
            if (tai == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = tai }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Tais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Tai tai = db.Tais.Find(id);
            try
            {
                db.Tais.Remove(tai);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tai này đã được sử dụng. Bạn không thể xóa nó!" });
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
