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
    public class CayMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/CayMaus
        public ActionResult Index()
        {
            var cayMaus = db.CayMaus.Include(c => c.InformationExamination);
            return View(cayMaus.ToList());
        }

        // GET: Admin/CayMaus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CayMau cayMau = db.CayMaus.Find(id);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            return View(cayMau);
        }

        // GET: Admin/CayMaus/Create
        public ActionResult Create()
        {
            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return PartialView("_Create");
        }

        // POST: Admin/CayMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MultiplesModel multiplesModel)
        {
            if (ModelState.IsValid)
            {
                db.CayMaus.Add(multiplesModel.CayMau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", multiplesModel.CayMau.InformationExamination_ID);
            return View(multiplesModel.CayMau);
        }

        // GET: Admin/CayMaus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CayMau cayMau = db.CayMaus.Find(id);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", cayMau.InformationExamination_ID);
            return View(cayMau);
        }

        // POST: Admin/CayMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ResultNuoiCay,ImageNuoiCay,ResultDinhDanh,ImageDinhDanh,NongDo,InformationExamination_ID")] CayMau cayMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cayMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", cayMau.InformationExamination_ID);
            return View(cayMau);
        }

        // GET: Admin/CayMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CayMau cayMau = db.CayMaus.Find(id);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            return View(cayMau);
        }

        // POST: Admin/CayMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CayMau cayMau = db.CayMaus.Find(id);
            db.CayMaus.Remove(cayMau);
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
