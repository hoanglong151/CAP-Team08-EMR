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
    public class Detail_HoHapController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_HoHap
        public ActionResult Index()
        {
            var detail_HoHap = db.Detail_HoHap.Include(d => d.HoHap).Include(d => d.InformationExamination);
            return View(detail_HoHap.ToList());
        }
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listHoHap = db.Detail_HoHap.ToList();
            foreach (var item in multiplesModel.HoHap)
            {
                var checkExistDetail1 = listHoHap.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.HoHap_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_HoHap detail_HoHap = new Detail_HoHap();
                    detail_HoHap.HoHap_ID = item.ID;
                    detail_HoHap.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_HoHap.Add(detail_HoHap);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listHoHap)
            {
                var checkDelete = multiplesModel.HoHap.FirstOrDefault(p => p.ID == item1.HoHap_ID);
                if (checkDelete == null)
                {
                    db.Detail_HoHap.Remove(item1);
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
