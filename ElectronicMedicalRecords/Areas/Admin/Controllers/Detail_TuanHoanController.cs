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
    public class Detail_TuanHoanController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TuanHoan
        public ActionResult Index()
        {
            var detail_TuanHoan = db.Detail_TuanHoan.Include(d => d.InformationExamination).Include(d => d.TuanHoan);
            return View(detail_TuanHoan.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTuanHoan = db.Detail_TuanHoan.ToList();
            foreach (var item in multiplesModel.TuanHoan)
            {
                var checkExistDetail1 = listTuanHoan.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TuanHoan_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TuanHoan detail_TuanHoan = new Detail_TuanHoan();
                    detail_TuanHoan.TuanHoan_ID = item.ID;
                    detail_TuanHoan.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TuanHoan.Add(detail_TuanHoan);
                    db.SaveChanges();
                }
                if (item.Dangerous == true && checkExistDetail1 == null)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    var checkInfo = db.InformationExaminations.AsNoTracking().FirstOrDefault(p => p.ID == multiplesModel.InformationExamination.ID);
                    if (checkInfo.PatientStatus_ID != 44)
                    {
                        db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            foreach(var item1 in listTuanHoan)
            {
                var checkDelete = multiplesModel.TuanHoan.FirstOrDefault(p => p.ID == item1.TuanHoan_ID);
                if(checkDelete == null)
                {
                    db.Detail_TuanHoan.Remove(item1);
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
