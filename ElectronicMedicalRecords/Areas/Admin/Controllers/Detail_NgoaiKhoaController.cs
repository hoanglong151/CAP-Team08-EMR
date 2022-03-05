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
    public class Detail_NgoaiKhoaController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_NgoaiKhoa
        public ActionResult Index()
        {
            var detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Include(d => d.InformationExamination).Include(d => d.NgoaiKhoa);
            return View(detail_NgoaiKhoa.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listNgoaiKhoa = db.Detail_NgoaiKhoa.ToList();
            foreach (var item in multiplesModel.NgoaiKhoa)
            {
                var checkExistDetail1 = listNgoaiKhoa.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.NgoaiKhoa_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_NgoaiKhoa detail_NgoaiKhoa = new Detail_NgoaiKhoa();
                    detail_NgoaiKhoa.NgoaiKhoa_ID = item.ID;
                    detail_NgoaiKhoa.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_NgoaiKhoa.Add(detail_NgoaiKhoa);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listNgoaiKhoa)
            {
                var checkDelete = multiplesModel.NgoaiKhoa.FirstOrDefault(p => p.ID == item1.NgoaiKhoa_ID);
                if (checkDelete == null)
                {
                    db.Detail_NgoaiKhoa.Remove(item1);
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
