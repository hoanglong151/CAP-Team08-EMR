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
    public class DaLieuxController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/DaLieux
        public ActionResult Index()
        {
            return View(db.DaLieux.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailDaLieu = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == id).ToList();
            var listDaLieu = db.DaLieux.ToList();
            foreach (var item in listDetailDaLieu)
            {
                var changeSelect = listDaLieu.FirstOrDefault(p => p.ID == item.DaLieu_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.DaLieu = listDaLieu;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrDaLieu(int id)
        {
            var listDetailDaLieu = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailDaLieu)
            {
                listOfStrings.Add("" + item.DaLieu_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var daLieus = db.DaLieux.ToList();
            return Json(new { data = daLieus }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(DaLieu daLieu)
        {
            string text = "";
            var checkExist = db.DaLieux.FirstOrDefault(e => e.Name == daLieu.Name);
            if (checkExist != null && daLieu.Name != null)
            {
                text = "Da Liễu đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(DaLieu daLieu)
        {
            string text = "";
            var checkExist = db.DaLieux.FirstOrDefault(e => e.Name == daLieu.Name);
            if (checkExist != null && checkExist.ID != daLieu.ID && daLieu.Name != null)
            {
                text = "Da Liễu đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/DaLieux/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.DaLieu = db.DaLieux.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/DaLieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DaLieu daLieu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(daLieu);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    daLieu.ChiDinh = false;
                    db.DaLieux.Add(daLieu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(daLieu);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/DaLieux/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DaLieu daLieu = db.DaLieux.Find(id);
            if (daLieu == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = daLieu }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DaLieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DaLieu daLieu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(daLieu);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.DaLieux.Find(daLieu.ID);
                    db.Entry(existData).CurrentValues.SetValues(daLieu);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(daLieu);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/DaLieux/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DaLieu daLieu = db.DaLieux.Find(id);
            if (daLieu == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = daLieu }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DaLieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            DaLieu daLieu = db.DaLieux.Find(id);
            try
            {
                db.DaLieux.Remove(daLieu);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Da Liễu này đã được sử dụng. Bạn không thể xóa nó!" });
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
