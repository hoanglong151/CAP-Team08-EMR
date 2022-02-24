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
    public class Detail_TuanHoanController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TuanHoan
        public ActionResult Index()
        {
            var detail_TuanHoan = db.Detail_TuanHoan.Include(d => d.InformationExamination).Include(d => d.TuanHoan);
            return View(detail_TuanHoan.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTuanHoan = db.Detail_TuanHoan.ToList();
            foreach (var item in multiplesModel.TuanHoan)
            {
                var checkExistDetail1 = listTuanHoan.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TuanHoan_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TuanHoan detail_TuanHoan = new Detail_TuanHoan();
                    detail_TuanHoan.TuanHoan_ID = item.ID;
                    detail_TuanHoan.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TuanHoan.Add(detail_TuanHoan);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach(var item1 in listTuanHoan)
            {
                var checkDelete = multiplesModel.TuanHoan.FirstOrDefault(p => p.ID == item1.TuanHoan_ID);
                if(checkDelete == null)
                {
                    db.Detail_TuanHoan.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_TuanHoan> detail_TuanHoans = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_TuanHoans = detail_TuanHoans;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_TuanHoan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TuanHoan detail_TuanHoan = db.Detail_TuanHoan.Find(id);
            if (detail_TuanHoan == null)
            {
                return HttpNotFound();
            }
            return View(detail_TuanHoan);
        }

        // GET: Admin/Detail_TuanHoan/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.TuanHoan_ID = new SelectList(db.TuanHoans, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_TuanHoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,TuanHoan_ID")] Detail_TuanHoan detail_TuanHoan)
        {
            if (ModelState.IsValid)
            {
                db.Detail_TuanHoan.Add(detail_TuanHoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TuanHoan.InformationExamination_ID);
            ViewBag.TuanHoan_ID = new SelectList(db.TuanHoans, "ID", "Name", detail_TuanHoan.TuanHoan_ID);
            return View(detail_TuanHoan);
        }

        // GET: Admin/Detail_TuanHoan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TuanHoan detail_TuanHoan = db.Detail_TuanHoan.Find(id);
            if (detail_TuanHoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TuanHoan.InformationExamination_ID);
            ViewBag.TuanHoan_ID = new SelectList(db.TuanHoans, "ID", "Name", detail_TuanHoan.TuanHoan_ID);
            return View(detail_TuanHoan);
        }

        // POST: Admin/Detail_TuanHoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,TuanHoan_ID")] Detail_TuanHoan detail_TuanHoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_TuanHoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TuanHoan.InformationExamination_ID);
            ViewBag.TuanHoan_ID = new SelectList(db.TuanHoans, "ID", "Name", detail_TuanHoan.TuanHoan_ID);
            return View(detail_TuanHoan);
        }

        // GET: Admin/Detail_TuanHoan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TuanHoan detail_TuanHoan = db.Detail_TuanHoan.Find(id);
            if (detail_TuanHoan == null)
            {
                return HttpNotFound();
            }
            return View(detail_TuanHoan);
        }

        // POST: Admin/Detail_TuanHoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_TuanHoan detail_TuanHoan = db.Detail_TuanHoan.Find(id);
            db.Detail_TuanHoan.Remove(detail_TuanHoan);
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
