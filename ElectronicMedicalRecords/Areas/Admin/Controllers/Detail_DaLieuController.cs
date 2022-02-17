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
    public class Detail_DaLieuController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_DaLieu
        public ActionResult Index()
        {
            var detail_DaLieu = db.Detail_DaLieu.Include(d => d.DaLieu).Include(d => d.InformationExamination);
            return View(detail_DaLieu.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listDaLieu = db.Detail_DaLieu.ToList();
            foreach (var item in multiplesModel.DaLieu)
            {
                var checkExistDetail1 = listDaLieu.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.DaLieu_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_DaLieu detail_DaLieu = new Detail_DaLieu();
                    detail_DaLieu.DaLieu_ID = item.ID;
                    detail_DaLieu.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_DaLieu.Add(detail_DaLieu);
                    db.SaveChanges();
                }
                if (checkExistDetail1.DaLieu.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listDaLieu)
            {
                var checkDelete = multiplesModel.DaLieu.FirstOrDefault(p => p.ID == item1.DaLieu_ID);
                if (checkDelete == null)
                {
                    db.Detail_DaLieu.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_DaLieu> detail_DaLieus = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_DaLieus = detail_DaLieus;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_DaLieu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_DaLieu detail_DaLieu = db.Detail_DaLieu.Find(id);
            if (detail_DaLieu == null)
            {
                return HttpNotFound();
            }
            return View(detail_DaLieu);
        }

        // GET: Admin/Detail_DaLieu/Create
        public ActionResult Create()
        {
            ViewBag.DaLieu_ID = new SelectList(db.DaLieux, "ID", "Name");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_DaLieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,DaLieu_ID")] Detail_DaLieu detail_DaLieu)
        {
            if (ModelState.IsValid)
            {
                db.Detail_DaLieu.Add(detail_DaLieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DaLieu_ID = new SelectList(db.DaLieux, "ID", "Name", detail_DaLieu.DaLieu_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_DaLieu.InformationExamination_ID);
            return View(detail_DaLieu);
        }

        // GET: Admin/Detail_DaLieu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_DaLieu detail_DaLieu = db.Detail_DaLieu.Find(id);
            if (detail_DaLieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.DaLieu_ID = new SelectList(db.DaLieux, "ID", "Name", detail_DaLieu.DaLieu_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_DaLieu.InformationExamination_ID);
            return View(detail_DaLieu);
        }

        // POST: Admin/Detail_DaLieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,DaLieu_ID")] Detail_DaLieu detail_DaLieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_DaLieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DaLieu_ID = new SelectList(db.DaLieux, "ID", "Name", detail_DaLieu.DaLieu_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_DaLieu.InformationExamination_ID);
            return View(detail_DaLieu);
        }

        // GET: Admin/Detail_DaLieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_DaLieu detail_DaLieu = db.Detail_DaLieu.Find(id);
            if (detail_DaLieu == null)
            {
                return HttpNotFound();
            }
            return View(detail_DaLieu);
        }

        // POST: Admin/Detail_DaLieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_DaLieu detail_DaLieu = db.Detail_DaLieu.Find(id);
            db.Detail_DaLieu.Remove(detail_DaLieu);
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
