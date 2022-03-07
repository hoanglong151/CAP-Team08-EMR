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
    public class Detail_TieuHoaController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TieuHoa
        public ActionResult Index()
        {
            var detail_TieuHoa = db.Detail_TieuHoa.Include(d => d.InformationExamination).Include(d => d.TieuHoa);
            return View(detail_TieuHoa.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTieuHoa = db.Detail_TieuHoa.ToList();
            foreach (var item in multiplesModel.TieuHoa)
            {
                var checkExistDetail1 = listTieuHoa.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TieuHoa_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TieuHoa detail_TieuHoa = new Detail_TieuHoa();
                    detail_TieuHoa.TieuHoa_ID = item.ID;
                    detail_TieuHoa.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TieuHoa.Add(detail_TieuHoa);
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
            foreach (var item1 in listTieuHoa)
            {
                var checkDelete = multiplesModel.TieuHoa.FirstOrDefault(p => p.ID == item1.TieuHoa_ID);
                if (checkDelete == null)
                {
                    db.Detail_TieuHoa.Remove(item1);
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
