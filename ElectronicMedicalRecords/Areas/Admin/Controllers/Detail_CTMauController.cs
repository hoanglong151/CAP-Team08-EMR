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
    public class Detail_CTMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // POST: Admin/Detail_CTMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_CTMau detail_CTMau, List<CTMau> cTMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach(var item in cTMaus)
            {
                var checkexist = db.Detail_CTMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = informationID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    db.Detail_CTMau.Add(detail_CTMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }
        public ActionResult DetailIE(MultiplesModel multiplesModel)
        {
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_CTMau/Edit/5
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_CTMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Edit")]
        public async Task<ActionResult> EditPost(MultiplesModel multiplesModel)
        {
            if(multiplesModel.Detail_CTMaus != null)
            {
                foreach(var detail_CTMau in multiplesModel.Detail_CTMaus)
                {
                    var DetailCTMau = db.Detail_CTMau.FirstOrDefault(p => p.CTMau_ID == detail_CTMau.CTMau_ID && p.InformationExamination_ID == detail_CTMau.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailCTMau.Result = detail_CTMau.Result;
                        db.Entry(DetailCTMau).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_CTMau detail_CTMau, List<CTMau> cTMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in cTMaus)
            {
                var checkexist = db.Detail_CTMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = informationID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    db.Detail_CTMau.Add(detail_CTMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
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
