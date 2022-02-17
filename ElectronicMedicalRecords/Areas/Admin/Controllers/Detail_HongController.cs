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
    public class Detail_HongController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Hong
        public ActionResult Index()
        {
            var detail_Hong = db.Detail_Hong.Include(d => d.Hong).Include(d => d.InformationExamination);
            return View(detail_Hong.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listHong = db.Detail_Hong.ToList();
            foreach (var item in multiplesModel.Hong)
            {
                var checkExistDetail1 = listHong.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.Hong_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_Hong detail_Hong = new Detail_Hong();
                    detail_Hong.Hong_ID = item.ID;
                    detail_Hong.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_Hong.Add(detail_Hong);
                    db.SaveChanges();
                }
                if (checkExistDetail1.Hong.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listHong)
            {
                var checkDelete = multiplesModel.Hong.FirstOrDefault(p => p.ID == item1.Hong_ID);
                if (checkDelete == null)
                {
                    db.Detail_Hong.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_Hong> detail_Hongs = db.Detail_Hong.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_Hongs = detail_Hongs;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Hong/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Hong detail_Hong = db.Detail_Hong.Find(id);
            if (detail_Hong == null)
            {
                return HttpNotFound();
            }
            return View(detail_Hong);
        }

        // GET: Admin/Detail_Hong/Create
        public ActionResult Create()
        {
            ViewBag.Hong_ID = new SelectList(db.Hongs, "ID", "Name");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_Hong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,Hong_ID")] Detail_Hong detail_Hong)
        {
            if (ModelState.IsValid)
            {
                db.Detail_Hong.Add(detail_Hong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Hong_ID = new SelectList(db.Hongs, "ID", "Name", detail_Hong.Hong_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Hong.InformationExamination_ID);
            return View(detail_Hong);
        }

        // GET: Admin/Detail_Hong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Hong detail_Hong = db.Detail_Hong.Find(id);
            if (detail_Hong == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hong_ID = new SelectList(db.Hongs, "ID", "Name", detail_Hong.Hong_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Hong.InformationExamination_ID);
            return View(detail_Hong);
        }

        // POST: Admin/Detail_Hong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,Hong_ID")] Detail_Hong detail_Hong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_Hong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Hong_ID = new SelectList(db.Hongs, "ID", "Name", detail_Hong.Hong_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Hong.InformationExamination_ID);
            return View(detail_Hong);
        }

        // GET: Admin/Detail_Hong/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Hong detail_Hong = db.Detail_Hong.Find(id);
            if (detail_Hong == null)
            {
                return HttpNotFound();
            }
            return View(detail_Hong);
        }

        // POST: Admin/Detail_Hong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Hong detail_Hong = db.Detail_Hong.Find(id);
            db.Detail_Hong.Remove(detail_Hong);
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
