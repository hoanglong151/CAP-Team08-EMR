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
    public class Detail_TaiMuiHongController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Tai
        public ActionResult Index()
        {
            var detail_Tai = db.Detail_TaiMuiHong.Include(d => d.InformationExamination).Include(d => d.TaiMuiHong);
            return View(detail_Tai.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTai = db.Detail_TaiMuiHong.ToList();
            foreach (var item in multiplesModel.TaiMuiHong)
            {
                var checkExistDetail1 = listTai.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TaiMuiHong_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TaiMuiHong detail_TaiMuiHong = new Detail_TaiMuiHong();
                    detail_TaiMuiHong.TaiMuiHong_ID = item.ID;
                    detail_TaiMuiHong.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TaiMuiHong.Add(detail_TaiMuiHong);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listTai)
            {
                var checkDelete = multiplesModel.TaiMuiHong.FirstOrDefault(p => p.ID == item1.TaiMuiHong_ID);
                if (checkDelete == null)
                {
                    db.Detail_TaiMuiHong.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_TaiMuiHong> detail_TaiMuiHongs = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_TaiMuiHongs = detail_TaiMuiHongs;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Tai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TaiMuiHong detail_TaiMuiHong = db.Detail_TaiMuiHong.Find(id);
            if (detail_TaiMuiHong == null)
            {
                return HttpNotFound();
            }
            return View(detail_TaiMuiHong);
        }

        // POST: Admin/Detail_Tai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_TaiMuiHong detail_Tai = db.Detail_TaiMuiHong.Find(id);
            db.Detail_TaiMuiHong.Remove(detail_Tai);
            db.SaveChanges();
            return RedirectToAction("Index");
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
