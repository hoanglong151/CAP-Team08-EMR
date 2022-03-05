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
    public class Detail_MatController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Mat
        public ActionResult Index()
        {
            var detail_Mat = db.Detail_Mat.Include(d => d.InformationExamination).Include(d => d.Mat);
            return View(detail_Mat.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listMat = db.Detail_Mat.ToList();
            foreach (var item in multiplesModel.Mat)
            {
                var checkExistDetail1 = listMat.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.Mat_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_Mat detail_Mat = new Detail_Mat();
                    detail_Mat.Mat_ID = item.ID;
                    detail_Mat.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_Mat.Add(detail_Mat);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listMat)
            {
                var checkDelete = multiplesModel.Mat.FirstOrDefault(p => p.ID == item1.Mat_ID);
                if (checkDelete == null)
                {
                    db.Detail_Mat.Remove(item1);
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
