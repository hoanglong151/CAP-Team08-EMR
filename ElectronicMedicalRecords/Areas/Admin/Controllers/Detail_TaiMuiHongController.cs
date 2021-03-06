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
                if (item.Dangerous == true && checkExistDetail1 == null)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    var checkInfo = db.InformationExaminations.AsNoTracking().FirstOrDefault(p => p.ID == multiplesModel.InformationExamination.ID);
                    if(checkInfo.PatientStatus_ID != 44)
                    {
                        db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                        db.SaveChanges();
                    }
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
            return PartialView("_BillCheck", multiplesModel);
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
