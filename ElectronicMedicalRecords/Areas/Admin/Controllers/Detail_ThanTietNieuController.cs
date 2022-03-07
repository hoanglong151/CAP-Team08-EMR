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
    public class Detail_ThanTietNieuController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_ThanTietNieu
        public ActionResult Index()
        {
            var detail_ThanTietNieu = db.Detail_ThanTietNieu.Include(d => d.InformationExamination).Include(d => d.ThanTietNieu);
            return View(detail_ThanTietNieu.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listThanTietNieu = db.Detail_ThanTietNieu.ToList();
            foreach (var item in multiplesModel.ThanTietNieu)
            {
                var checkExistDetail1 = listThanTietNieu.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.ThanTietNieu_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_ThanTietNieu detail_ThanTietNieu = new Detail_ThanTietNieu();
                    detail_ThanTietNieu.ThanTietNieu_ID = item.ID;
                    detail_ThanTietNieu.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_ThanTietNieu.Add(detail_ThanTietNieu);
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
            foreach (var item1 in listThanTietNieu)
            {
                var checkDelete = multiplesModel.ThanTietNieu.FirstOrDefault(p => p.ID == item1.ThanTietNieu_ID);
                if (checkDelete == null)
                {
                    db.Detail_ThanTietNieu.Remove(item1);
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
