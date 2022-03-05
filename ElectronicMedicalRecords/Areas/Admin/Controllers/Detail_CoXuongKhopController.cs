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
    public class Detail_CoXuongKhopController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_CoXuongKhop
        public ActionResult Index()
        {
            var detail_CoXuongKhop = db.Detail_CoXuongKhop.Include(d => d.CoXuongKhop).Include(d => d.InformationExamination);
            return View(detail_CoXuongKhop.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listCoXuongKhop = db.Detail_CoXuongKhop.ToList();
            foreach (var item in multiplesModel.CoXuongKhop)
            {
                var checkExistDetail1 = listCoXuongKhop.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.CoXuongKhop_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_CoXuongKhop detail_CoXuongKhop = new Detail_CoXuongKhop();
                    detail_CoXuongKhop.CoXuongKhop_ID = item.ID;
                    detail_CoXuongKhop.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_CoXuongKhop.Add(detail_CoXuongKhop);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listCoXuongKhop)
            {
                var checkDelete = multiplesModel.CoXuongKhop.FirstOrDefault(p => p.ID == item1.CoXuongKhop_ID);
                if (checkDelete == null)
                {
                    db.Detail_CoXuongKhop.Remove(item1);
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
