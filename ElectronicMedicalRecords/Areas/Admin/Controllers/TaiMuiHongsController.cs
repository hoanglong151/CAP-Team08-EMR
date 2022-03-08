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
    public class TaiMuiHongsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Tais
        public ActionResult Index()
        {
            return View(db.TaiMuiHongs.ToList());
        }

        public ActionResult EditSelect(MultiplesModel multiplesModel)
        {
            var listDetailTai = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            var listTaiMuiHong = db.TaiMuiHongs.AsNoTracking().ToList();
            foreach (var item in listDetailTai)
            {
                var changeSelect = listTaiMuiHong.FirstOrDefault(p => p.ID == item.TaiMuiHong_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.TaiMuiHong = listTaiMuiHong;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrTaiMuiHong(int id)
        {
            var listDetailTaiMuiHong = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailTaiMuiHong)
            {
                listOfStrings.Add("" + item.TaiMuiHong_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var taimuihongs = db.TaiMuiHongs.ToList();
            return Json(new { data = taimuihongs }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(TaiMuiHong taiMuiHong)
        {
            string text = "";
            var checkExist = db.TaiMuiHongs.FirstOrDefault(e => e.Name == taiMuiHong.Name);
            if (checkExist != null && taiMuiHong.Name != null)
            {
                text = "Tai-Mũi-Họng đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(TaiMuiHong taimuihong)
        {
            string text = "";
            var checkExist = db.TaiMuiHongs.FirstOrDefault(e => e.Name == taimuihong.Name);
            if (checkExist != null && checkExist.ID != taimuihong.ID && taimuihong.Name != null)
            {
                text = "Tai-Mũi-Họng đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/Tais/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.TaiMuiHong = db.TaiMuiHongs.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Tais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaiMuiHong taimuihong)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(taimuihong);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    taimuihong.ChiDinh = false;
                    db.TaiMuiHongs.Add(taimuihong);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(taimuihong);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Tais/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TaiMuiHong taimuihong = db.TaiMuiHongs.Find(id);
            if (taimuihong == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = taimuihong }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Tais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaiMuiHong taimuihong)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(taimuihong);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.TaiMuiHongs.Find(taimuihong.ID);
                    db.Entry(existData).CurrentValues.SetValues(taimuihong);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(taimuihong);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Tais/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TaiMuiHong taimuihong = db.TaiMuiHongs.Find(id);
            if (taimuihong == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = taimuihong }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Tais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TaiMuiHong taimuihong = db.TaiMuiHongs.Find(id);
            try
            {
                db.TaiMuiHongs.Remove(taimuihong);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Tai-Mui-Họng này đã được sử dụng. Bạn không thể xóa nó!" });
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
