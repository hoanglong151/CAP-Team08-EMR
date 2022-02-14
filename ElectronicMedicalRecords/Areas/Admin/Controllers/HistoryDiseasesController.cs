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
    public class HistoryDiseasesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/HistoryDiseases
        public ActionResult Index()
        {
            return View(db.HistoryDiseases.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var historyDiseases = db.HistoryDiseases.ToList();
            return Json(new { data = historyDiseases }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(HistoryDisease historyDisease)
        {
            string text = "";
            var checkExist = db.HistoryDiseases.FirstOrDefault(e => e.Name == historyDisease.Name);
            if (checkExist != null && historyDisease.Name != null)
            {
                text = "Bệnh Tiền Sử đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(HistoryDisease historyDisease)
        {
            string text = "";
            var checkExist = db.HistoryDiseases.FirstOrDefault(e => e.Name == historyDisease.Name);
            if (checkExist != null && checkExist.ID != historyDisease.ID && historyDisease.Name != null)
            {
                text = "Bệnh Tiền Sử đã có trong danh sách";
            }
            return text;
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listHistoryDiseases1 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases2 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases3 = db.HistoryDiseases.AsNoTracking().ToList();
            var detail_HistoryDiseases1 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == multiplesModel.Patient.ID && p.LevelFamily == "Ông/Bà");
            var detail_HistoryDiseases2 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == multiplesModel.Patient.ID && p.LevelFamily == "Cha/Mẹ");
            var detail_HistoryDiseases3 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == multiplesModel.Patient.ID && p.LevelFamily == "Anh/Chị em");
            foreach (var item1 in listHistoryDiseases1)
            {
                var historyDisease1 = detail_HistoryDiseases1.FirstOrDefault(p => p.HistoryDisease_ID == item1.ID);
                if(historyDisease1 != null)
                {
                    item1.ChiDinh = historyDisease1.Selected;
                }
            }
            multiplesModel.HistoryDiseases1 = listHistoryDiseases1;

            foreach (var item2 in listHistoryDiseases2)
            {
                var historyDisease2 = detail_HistoryDiseases2.FirstOrDefault(p => p.HistoryDisease_ID == item2.ID);
                if (historyDisease2 != null)
                {
                    item2.ChiDinh = historyDisease2.Selected;
                }
            }
            multiplesModel.HistoryDiseases2 = listHistoryDiseases2;

            foreach (var item3 in listHistoryDiseases3)
            {
                var historyDisease3 = detail_HistoryDiseases3.FirstOrDefault(p => p.HistoryDisease_ID == item3.ID);
                if (historyDisease3 != null)
                {
                    item3.ChiDinh = historyDisease3.Selected;
                }
            }
            multiplesModel.HistoryDiseases3 = listHistoryDiseases3;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/HistoryDiseases/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.HistoryDiseases1 = db.HistoryDiseases.ToList();
            multiplesModel.HistoryDiseases2 = db.HistoryDiseases.ToList();
            multiplesModel.HistoryDiseases3 = db.HistoryDiseases.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/HistoryDiseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistoryDisease historyDisease)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(historyDisease);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    historyDisease.ChiDinh = false;
                    db.HistoryDiseases.Add(historyDisease);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(historyDisease);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HistoryDiseases/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            if (historyDisease == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = historyDisease }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HistoryDiseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HistoryDisease historyDisease)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(historyDisease);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.HistoryDiseases.Find(historyDisease.ID);
                    db.Entry(existData).CurrentValues.SetValues(historyDisease);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(historyDisease);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/HistoryDiseases/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            if (historyDisease == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = historyDisease }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HistoryDiseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            try
            {
                db.HistoryDiseases.Remove(historyDisease);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Bệnh Tiền Sử này đã được sử dụng. Bạn không thể xóa nó!" });
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
