using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class Detail_ViSinhController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(Detail_ViSinh detail_ViSinh, List<ViSinh> viSinhs, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in viSinhs)
            {
                var checkexistOld = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexistOld == null)
                {
                    detail_ViSinh.ViSinh_ID = item.ID;
                    detail_ViSinh.InformationExamination_ID = informationID;
                    detail_ViSinh.ChiDinh = item.ChiDinh;
                    db.Detail_ViSinh.Add(detail_ViSinh);
                    db.SaveChanges();
                }
            }
            ViewBag.ViSinh_ID = new SelectList(db.Amniocentes, "ID", "NameTest", detail_ViSinh.ViSinh_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ViSinh.InformationExamination_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(MultiplesModel multiplesModel)
        {
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_ViSinh/Edit/5
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_ViSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Edit")]
        public async Task<ActionResult> EditPost(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_ViSinhs != null)
            {
                foreach (var detail_ViSinh in multiplesModel.Detail_ViSinhs)
                {
                    var DetailViSinh = db.Detail_ViSinh.FirstOrDefault(p => p.ViSinh_ID == detail_ViSinh.ViSinh_ID && p.InformationExamination_ID == detail_ViSinh.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailViSinh.Result = detail_ViSinh.Result;
                        DetailViSinh.ResultNC = detail_ViSinh.ResultNC;
                        DetailViSinh.ResultDD = detail_ViSinh.ResultDD;
                        DetailViSinh.MatDo = detail_ViSinh.MatDo;
                        db.Entry(DetailViSinh).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
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
