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
    public class Detail_MatController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Mat
        public ActionResult Index()
        {
            var detail_Mat = db.Detail_Mat.Include(d => d.InformationExamination).Include(d => d.Mat);
            return View(detail_Mat.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listMat = db.Detail_Mat.ToList();
            foreach (var item in multiplesModel.Mat)
            {
                var checkExistDetail1 = listMat.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.Mat_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_Mat detail_Mat = new Detail_Mat();
                    detail_Mat.Mat_ID = item.ID;
                    detail_Mat.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_Mat.Add(detail_Mat);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listMat)
            {
                var checkDelete = multiplesModel.Mat.FirstOrDefault(p => p.ID == item1.Mat_ID);
                if (checkDelete == null)
                {
                    db.Detail_Mat.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_Mat> detail_Mats = db.Detail_Mat.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            Clinical clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
            multiplesModel.Clinical = clinical;
            multiplesModel.Detail_Mats = detail_Mats;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Mat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mat detail_Mat = db.Detail_Mat.Find(id);
            if (detail_Mat == null)
            {
                return HttpNotFound();
            }
            return View(detail_Mat);
        }

        // GET: Admin/Detail_Mat/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.Mat_ID = new SelectList(db.Mats, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_Mat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,Mat_ID")] Detail_Mat detail_Mat)
        {
            if (ModelState.IsValid)
            {
                db.Detail_Mat.Add(detail_Mat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mat.InformationExamination_ID);
            ViewBag.Mat_ID = new SelectList(db.Mats, "ID", "Name", detail_Mat.Mat_ID);
            return View(detail_Mat);
        }

        // GET: Admin/Detail_Mat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mat detail_Mat = db.Detail_Mat.Find(id);
            if (detail_Mat == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mat.InformationExamination_ID);
            ViewBag.Mat_ID = new SelectList(db.Mats, "ID", "Name", detail_Mat.Mat_ID);
            return View(detail_Mat);
        }

        // POST: Admin/Detail_Mat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,Mat_ID")] Detail_Mat detail_Mat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_Mat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mat.InformationExamination_ID);
            ViewBag.Mat_ID = new SelectList(db.Mats, "ID", "Name", detail_Mat.Mat_ID);
            return View(detail_Mat);
        }

        // GET: Admin/Detail_Mat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mat detail_Mat = db.Detail_Mat.Find(id);
            if (detail_Mat == null)
            {
                return HttpNotFound();
            }
            return View(detail_Mat);
        }

        // POST: Admin/Detail_Mat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Mat detail_Mat = db.Detail_Mat.Find(id);
            db.Detail_Mat.Remove(detail_Mat);
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
