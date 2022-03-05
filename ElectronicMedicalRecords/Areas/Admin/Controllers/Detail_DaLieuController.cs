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
    public class Detail_DaLieuController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_DaLieu
        public ActionResult Index()
        {
            var detail_DaLieu = db.Detail_DaLieu.Include(d => d.DaLieu).Include(d => d.InformationExamination);
            return View(detail_DaLieu.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listDaLieu = db.Detail_DaLieu.ToList();
            foreach (var item in multiplesModel.DaLieu)
            {
                var checkExistDetail1 = listDaLieu.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.DaLieu_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_DaLieu detail_DaLieu = new Detail_DaLieu();
                    detail_DaLieu.DaLieu_ID = item.ID;
                    detail_DaLieu.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_DaLieu.Add(detail_DaLieu);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listDaLieu)
            {
                var checkDelete = multiplesModel.DaLieu.FirstOrDefault(p => p.ID == item1.DaLieu_ID);
                if (checkDelete == null)
                {
                    db.Detail_DaLieu.Remove(item1);
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
