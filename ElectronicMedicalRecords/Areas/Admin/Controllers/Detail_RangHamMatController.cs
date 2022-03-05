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
    public class Detail_RangHamMatController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_RangHamMat
        public ActionResult Index()
        {
            var detail_RangHamMat = db.Detail_RangHamMat.Include(d => d.InformationExamination).Include(d => d.RangHamMat);
            return View(detail_RangHamMat.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listRangHamMat = db.Detail_RangHamMat.ToList();
            foreach (var item in multiplesModel.RangHamMat)
            {
                var checkExistDetail1 = listRangHamMat.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.RangHamMat_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_RangHamMat detail_RangHamMat = new Detail_RangHamMat();
                    detail_RangHamMat.RangHamMat_ID = item.ID;
                    detail_RangHamMat.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_RangHamMat.Add(detail_RangHamMat);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listRangHamMat)
            {
                var checkDelete = multiplesModel.RangHamMat.FirstOrDefault(p => p.ID == item1.RangHamMat_ID);
                if (checkDelete == null)
                {
                    db.Detail_RangHamMat.Remove(item1);
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
