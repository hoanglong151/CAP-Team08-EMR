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
    public class Detail_TamThanController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TamThan
        public ActionResult Index()
        {
            var detail_TamThan = db.Detail_TamThan.Include(d => d.InformationExamination).Include(d => d.TamThan);
            return View(detail_TamThan.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTamThan = db.Detail_TamThan.ToList();
            foreach (var item in multiplesModel.TamThan)
            {
                var checkExistDetail1 = listTamThan.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TamThan_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TamThan detail_TamThan = new Detail_TamThan();
                    detail_TamThan.TamThan_ID = item.ID;
                    detail_TamThan.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TamThan.Add(detail_TamThan);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listTamThan)
            {
                var checkDelete = multiplesModel.TamThan.FirstOrDefault(p => p.ID == item1.TamThan_ID);
                if (checkDelete == null)
                {
                    db.Detail_TamThan.Remove(item1);
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
