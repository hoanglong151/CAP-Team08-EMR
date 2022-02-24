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
    public class Detail_RangHamMatController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_RangHamMat
        public ActionResult Index()
        {
            var detail_RangHamMat = db.Detail_RangHamMat.Include(d => d.InformationExamination).Include(d => d.RangHamMat);
            return View(detail_RangHamMat.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listRangHamMat = db.Detail_RangHamMat.ToList();
            foreach (var item in multiplesModel.RangHamMat)
            {
                var checkExistDetail1 = listRangHamMat.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.RangHamMat_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_RangHamMat detail_RangHamMat = new Detail_RangHamMat();
                    detail_RangHamMat.RangHamMat_ID = item.ID;
                    detail_RangHamMat.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_RangHamMat.Add(detail_RangHamMat);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listRangHamMat)
            {
                var checkDelete = multiplesModel.RangHamMat.FirstOrDefault(p => p.ID == item1.RangHamMat_ID);
                if (checkDelete == null)
                {
                    db.Detail_RangHamMat.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_RangHamMat> detail_RangHamMats = db.Detail_RangHamMat.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_RangHamMats = detail_RangHamMats;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_RangHamMat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_RangHamMat detail_RangHamMat = db.Detail_RangHamMat.Find(id);
            if (detail_RangHamMat == null)
            {
                return HttpNotFound();
            }
            return View(detail_RangHamMat);
        }

        // GET: Admin/Detail_RangHamMat/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.RangHamMat_ID = new SelectList(db.RangHamMats, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_RangHamMat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,RangHamMat_ID")] Detail_RangHamMat detail_RangHamMat)
        {
            if (ModelState.IsValid)
            {
                db.Detail_RangHamMat.Add(detail_RangHamMat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_RangHamMat.InformationExamination_ID);
            ViewBag.RangHamMat_ID = new SelectList(db.RangHamMats, "ID", "Name", detail_RangHamMat.RangHamMat_ID);
            return View(detail_RangHamMat);
        }

        // GET: Admin/Detail_RangHamMat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_RangHamMat detail_RangHamMat = db.Detail_RangHamMat.Find(id);
            if (detail_RangHamMat == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_RangHamMat.InformationExamination_ID);
            ViewBag.RangHamMat_ID = new SelectList(db.RangHamMats, "ID", "Name", detail_RangHamMat.RangHamMat_ID);
            return View(detail_RangHamMat);
        }

        // POST: Admin/Detail_RangHamMat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,RangHamMat_ID")] Detail_RangHamMat detail_RangHamMat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_RangHamMat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_RangHamMat.InformationExamination_ID);
            ViewBag.RangHamMat_ID = new SelectList(db.RangHamMats, "ID", "Name", detail_RangHamMat.RangHamMat_ID);
            return View(detail_RangHamMat);
        }

        // GET: Admin/Detail_RangHamMat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_RangHamMat detail_RangHamMat = db.Detail_RangHamMat.Find(id);
            if (detail_RangHamMat == null)
            {
                return HttpNotFound();
            }
            return View(detail_RangHamMat);
        }

        // POST: Admin/Detail_RangHamMat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_RangHamMat detail_RangHamMat = db.Detail_RangHamMat.Find(id);
            db.Detail_RangHamMat.Remove(detail_RangHamMat);
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
