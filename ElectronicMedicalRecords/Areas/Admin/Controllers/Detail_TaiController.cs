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
    public class Detail_TaiController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Tai
        public ActionResult Index()
        {
            var detail_Tai = db.Detail_Tai.Include(d => d.InformationExamination).Include(d => d.Tai);
            return View(detail_Tai.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTai = db.Detail_Tai.ToList();
            foreach (var item in multiplesModel.Tai)
            {
                var checkExistDetail1 = listTai.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.Tai_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_Tai detail_Tai = new Detail_Tai();
                    detail_Tai.Tai_ID = item.ID;
                    detail_Tai.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_Tai.Add(detail_Tai);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listTai)
            {
                var checkDelete = multiplesModel.Tai.FirstOrDefault(p => p.ID == item1.Tai_ID);
                if (checkDelete == null)
                {
                    db.Detail_Tai.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_Tai> detail_Tais = db.Detail_Tai.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_Tais = detail_Tais;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Tai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Tai detail_Tai = db.Detail_Tai.Find(id);
            if (detail_Tai == null)
            {
                return HttpNotFound();
            }
            return View(detail_Tai);
        }

        // GET: Admin/Detail_Tai/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.Tai_ID = new SelectList(db.Tais, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_Tai/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,Tai_ID")] Detail_Tai detail_Tai)
        {
            if (ModelState.IsValid)
            {
                db.Detail_Tai.Add(detail_Tai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Tai.InformationExamination_ID);
            ViewBag.Tai_ID = new SelectList(db.Tais, "ID", "Name", detail_Tai.Tai_ID);
            return View(detail_Tai);
        }

        // GET: Admin/Detail_Tai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Tai detail_Tai = db.Detail_Tai.Find(id);
            if (detail_Tai == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Tai.InformationExamination_ID);
            ViewBag.Tai_ID = new SelectList(db.Tais, "ID", "Name", detail_Tai.Tai_ID);
            return View(detail_Tai);
        }

        // POST: Admin/Detail_Tai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,Tai_ID")] Detail_Tai detail_Tai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_Tai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Tai.InformationExamination_ID);
            ViewBag.Tai_ID = new SelectList(db.Tais, "ID", "Name", detail_Tai.Tai_ID);
            return View(detail_Tai);
        }

        // GET: Admin/Detail_Tai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Tai detail_Tai = db.Detail_Tai.Find(id);
            if (detail_Tai == null)
            {
                return HttpNotFound();
            }
            return View(detail_Tai);
        }

        // POST: Admin/Detail_Tai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Tai detail_Tai = db.Detail_Tai.Find(id);
            db.Detail_Tai.Remove(detail_Tai);
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
