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
    public class Detail_ImmuneController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // POST: Admin/Detail_Immune/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_Immune detail_Immune, List<Immune> iMmunes, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in iMmunes)
            {
                var checkexist = db.Detail_Immune.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_Immune.Immue_ID = item.ID;
                    detail_Immune.InformationExamination_ID = informationID;
                    detail_Immune.ChiDinh = item.ChiDinh;
                    db.Detail_Immune.Add(detail_Immune);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.Immune_ID = new SelectList(db.Immunes, "ID", "NameTest", detail_Immune.Immue_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Immune.InformationExamination_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Immune> detail_Immunes = db.Detail_Immune.Where(p => p.InformationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Immunes = detail_Immunes;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Immune> detail_Immunes = db.Detail_Immune.Where(p => p.InformationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Immunes = detail_Immunes;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Immune/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Immune> detail_Immunes = db.Detail_Immune.Where(p => p.InformationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Immunes = detail_Immunes;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Immune/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_Immunes != null)
            {
                foreach (var detail_Immune in multiplesModel.Detail_Immunes)
                {
                    var DetailImmune = db.Detail_Immune.FirstOrDefault(p => p.Immue_ID == detail_Immune.Immue_ID && p.InformationExamination_ID == detail_Immune.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailImmune.Result = detail_Immune.Result;
                        db.Entry(DetailImmune).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_Immune detail_Immune, List<Immune> iMmunes, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in iMmunes)
            {
                var checkexist = db.Detail_Immune.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_Immune.Immue_ID = item.ID;
                    detail_Immune.InformationExamination_ID = informationID;
                    detail_Immune.ChiDinh = item.ChiDinh;
                    db.Detail_Immune.Add(detail_Immune);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.Immune_ID = new SelectList(db.Immunes, "ID", "NameTest", detail_Immune.Immue_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Immune.InformationExamination_ID);
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
