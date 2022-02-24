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
    public class Detail_ThanTietNieuController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_ThanTietNieu
        public ActionResult Index()
        {
            var detail_ThanTietNieu = db.Detail_ThanTietNieu.Include(d => d.InformationExamination).Include(d => d.ThanTietNieu);
            return View(detail_ThanTietNieu.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listThanTietNieu = db.Detail_ThanTietNieu.ToList();
            foreach (var item in multiplesModel.ThanTietNieu)
            {
                var checkExistDetail1 = listThanTietNieu.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.ThanTietNieu_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_ThanTietNieu detail_ThanTietNieu = new Detail_ThanTietNieu();
                    detail_ThanTietNieu.ThanTietNieu_ID = item.ID;
                    detail_ThanTietNieu.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_ThanTietNieu.Add(detail_ThanTietNieu);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listThanTietNieu)
            {
                var checkDelete = multiplesModel.ThanTietNieu.FirstOrDefault(p => p.ID == item1.ThanTietNieu_ID);
                if (checkDelete == null)
                {
                    db.Detail_ThanTietNieu.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_ThanTietNieu> detail_ThanTietNieus = db.Detail_ThanTietNieu.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_ThanTietNieus = detail_ThanTietNieus;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_ThanTietNieu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanTietNieu detail_ThanTietNieu = db.Detail_ThanTietNieu.Find(id);
            if (detail_ThanTietNieu == null)
            {
                return HttpNotFound();
            }
            return View(detail_ThanTietNieu);
        }

        // GET: Admin/Detail_ThanTietNieu/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.ThanTietNieu_ID = new SelectList(db.ThanTietNieux, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_ThanTietNieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,ThanTietNieu_ID")] Detail_ThanTietNieu detail_ThanTietNieu)
        {
            if (ModelState.IsValid)
            {
                db.Detail_ThanTietNieu.Add(detail_ThanTietNieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanTietNieu.InformationExamination_ID);
            ViewBag.ThanTietNieu_ID = new SelectList(db.ThanTietNieux, "ID", "Name", detail_ThanTietNieu.ThanTietNieu_ID);
            return View(detail_ThanTietNieu);
        }

        // GET: Admin/Detail_ThanTietNieu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanTietNieu detail_ThanTietNieu = db.Detail_ThanTietNieu.Find(id);
            if (detail_ThanTietNieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanTietNieu.InformationExamination_ID);
            ViewBag.ThanTietNieu_ID = new SelectList(db.ThanTietNieux, "ID", "Name", detail_ThanTietNieu.ThanTietNieu_ID);
            return View(detail_ThanTietNieu);
        }

        // POST: Admin/Detail_ThanTietNieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,ThanTietNieu_ID")] Detail_ThanTietNieu detail_ThanTietNieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_ThanTietNieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanTietNieu.InformationExamination_ID);
            ViewBag.ThanTietNieu_ID = new SelectList(db.ThanTietNieux, "ID", "Name", detail_ThanTietNieu.ThanTietNieu_ID);
            return View(detail_ThanTietNieu);
        }

        // GET: Admin/Detail_ThanTietNieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanTietNieu detail_ThanTietNieu = db.Detail_ThanTietNieu.Find(id);
            if (detail_ThanTietNieu == null)
            {
                return HttpNotFound();
            }
            return View(detail_ThanTietNieu);
        }

        // POST: Admin/Detail_ThanTietNieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_ThanTietNieu detail_ThanTietNieu = db.Detail_ThanTietNieu.Find(id);
            db.Detail_ThanTietNieu.Remove(detail_ThanTietNieu);
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
