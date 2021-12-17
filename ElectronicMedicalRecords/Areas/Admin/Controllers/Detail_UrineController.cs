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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class Detail_UrineController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // POST: Admin/Detail_Urine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_Urine detail_UrineCreateP, List<Urine> Urines, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in Urines)
            {
                var checkexist = db.Detail_Urine.Where(p => p.InfomationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_UrineCreateP.Urine_ID = item.ID;
                    detail_UrineCreateP.InfomationExamination_ID = informationID;
                    detail_UrineCreateP.ChiDinh = item.ChiDinh;
                    db.Detail_Urine.Add(detail_UrineCreateP);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.InfomationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_UrineCreateP.InfomationExamination_ID);
            ViewBag.Urine_ID = new SelectList(db.Urines, "ID", "Name", detail_UrineCreateP.Urine_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Urines = detail_Urines;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Urines = detail_Urines;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Urine/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Urines = detail_Urines;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Urine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_Urines != null)
            {
                foreach (var detail_Urine in multiplesModel.Detail_Urines)
                {
                    var DetailUrine = db.Detail_Urine.FirstOrDefault(p => p.Urine_ID == detail_Urine.Urine_ID && p.InfomationExamination_ID == detail_Urine.InfomationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailUrine.Result = detail_Urine.Result;
                        db.Entry(DetailUrine).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_Urine detail_Urine, List<Urine> Urines, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in Urines)
            {
                var checkexist = db.Detail_Urine.Where(p => p.InfomationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_Urine.Urine_ID = item.ID;
                    detail_Urine.InfomationExamination_ID = informationID;
                    detail_Urine.ChiDinh = item.ChiDinh;
                    db.Detail_Urine.Add(detail_Urine);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.InfomationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Urine.InfomationExamination_ID);
            ViewBag.Urine_ID = new SelectList(db.Urines, "ID", "Name", detail_Urine.Urine_ID);
            return RedirectToAction("Create", "MultipleModels");
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
