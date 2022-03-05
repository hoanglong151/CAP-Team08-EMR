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
    public class Detail_ThanKinhController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_ThanKinh
        public ActionResult Index()
        {
            var detail_ThanKinh = db.Detail_ThanKinh.Include(d => d.InformationExamination).Include(d => d.ThanKinh);
            return View(detail_ThanKinh.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listThanKinh = db.Detail_ThanKinh.ToList();
            foreach (var item in multiplesModel.ThanKinh)
            {
                var checkExistDetail1 = listThanKinh.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.ThanKinh_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_ThanKinh detail_ThanKinh = new Detail_ThanKinh();
                    detail_ThanKinh.ThanKinh_ID = item.ID;
                    detail_ThanKinh.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_ThanKinh.Add(detail_ThanKinh);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listThanKinh)
            {
                var checkDelete = multiplesModel.ThanKinh.FirstOrDefault(p => p.ID == item1.ThanKinh_ID);
                if (checkDelete == null)
                {
                    db.Detail_ThanKinh.Remove(item1);
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
