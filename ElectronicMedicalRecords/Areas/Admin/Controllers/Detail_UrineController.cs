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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class Detail_UrineController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Urine
        public ActionResult Index()
        {
            var detail_Urine = db.Detail_Urine.Include(d => d.InformationExamination).Include(d => d.Urine);
            return View(detail_Urine.ToList());
        }

        // GET: Admin/Detail_Urine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Urine detail_Urine = db.Detail_Urine.Find(id);
            if (detail_Urine == null)
            {
                return HttpNotFound();
            }
            return View(detail_Urine);
        }

        // GET: Admin/Detail_Urine/Create
        public ActionResult Create()
        {
            ViewBag.InfomationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.Urine_ID = new SelectList(db.Urines, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_Urine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<Urine> Urines, int informationID, MultiplesModel multiplesModel)
        {
            Detail_Urine detail_Urine = new Detail_Urine();
            foreach (var item in Urines)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_Urine.Urine_ID = item.ID;
                    detail_Urine.InfomationExamination_ID = informationID;
                    detail_Urine.ChiDinh = item.ChiDinh;
                    detail_Urine.Result = item.Result;
                    multiplesModel.InformationExamination.ResultNuocTieu = false;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.Detail_Urine.Add(detail_Urine);
                    db.SaveChanges();
                }

            }
            ViewBag.InfomationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Urine.InfomationExamination_ID);
            ViewBag.Urine_ID = new SelectList(db.Urines, "ID", "Name", detail_Urine.Urine_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == id).ToList();
            List<Urine> Urines = new List<Urine>();
            for (int i = 0; i < detail_Urines.Count; i++)
            {
                var Urine_ID = detail_Urines[i].Urine_ID;
                var UrineCD = db.Urines.FirstOrDefault(p => p.ID == Urine_ID);
                UrineCD.ChiDinh = detail_Urines[i].ChiDinh;
                UrineCD.Result = detail_Urines[i].Result;
                detail_Urines[i].InfomationExamination_ID = id;
                Urines.Add(UrineCD);
            }
            if (detail_Urines == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Urine = Urines;
            multiplesModel.Detail_Urines = detail_Urines;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_Urine/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Urine> detail_Urines = db.Detail_Urine.Where(p => p.InfomationExamination_ID == id).ToList();
            List<Urine> Urines = new List<Urine>();
            for (int i = 0; i < detail_Urines.Count; i++)
            {
                var Urine_ID = detail_Urines[i].Urine_ID;
                var UrineCD = db.Urines.FirstOrDefault(p => p.ID == Urine_ID);
                UrineCD.ChiDinh = detail_Urines[i].ChiDinh;
                UrineCD.Result = detail_Urines[i].Result;
                detail_Urines[i].InfomationExamination_ID = id;
                Urines.Add(UrineCD);
            }
            if (detail_Urines == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Urine = Urines;
            multiplesModel.Detail_Urines = detail_Urines;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Urine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_Urines != null)
            {
                foreach (var detail_Urine in multiplesModel.Detail_Urines)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(detail_Urine).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                var checkResult = multiplesModel.Detail_Urines.All(p => p.Result != null);
                if (checkResult == true)
                {
                    multiplesModel.InformationExamination.ResultNuocTieu = true;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            Detail_Urine detail_Urine = new Detail_Urine();
            foreach (var item in multiplesModel.Urine)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_Urine.Urine_ID = item.ID;
                    detail_Urine.InfomationExamination_ID = multiplesModel.InformationExamination.ID;
                    detail_Urine.ChiDinh = item.ChiDinh;
                    detail_Urine.Result = item.Result;
                    multiplesModel.InformationExamination.ResultNuocTieu = false;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.Detail_Urine.Add(detail_Urine);
                    db.SaveChanges();
                }

            }
            ViewBag.InfomationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Urine.InfomationExamination_ID);
            ViewBag.Urine_ID = new SelectList(db.Urines, "ID", "Name", detail_Urine.Urine_ID);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Detail_Urine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Urine detail_Urine = db.Detail_Urine.Find(id);
            if (detail_Urine == null)
            {
                return HttpNotFound();
            }
            return View(detail_Urine);
        }

        // POST: Admin/Detail_Urine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Urine detail_Urine = db.Detail_Urine.Find(id);
            db.Detail_Urine.Remove(detail_Urine);
            db.SaveChanges();
            return RedirectToAction("Index");
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
