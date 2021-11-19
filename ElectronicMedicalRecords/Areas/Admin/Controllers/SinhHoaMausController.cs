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
    public class SinhHoaMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/SinhHoaMaus
        public ActionResult Index()
        {
            return View(db.SinhHoaMaus.ToList());
        }

        // GET: Admin/SinhHoaMaus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhHoaMau sinhHoaMau = db.SinhHoaMaus.Find(id);
            if (sinhHoaMau == null)
            {
                return HttpNotFound();
            }
            return View(sinhHoaMau);
        }

        // GET: Admin/SinhHoaMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.SinhHoaMau = db.SinhHoaMaus.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/SinhHoaMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChiDinh,NameTest,CSBT,Result")] SinhHoaMau sinhHoaMau)
        {
            if (ModelState.IsValid)
            {
                db.SinhHoaMaus.Add(sinhHoaMau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sinhHoaMau);
        }

        // GET: Admin/SinhHoaMaus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhHoaMau sinhHoaMau = db.SinhHoaMaus.Find(id);
            if (sinhHoaMau == null)
            {
                return HttpNotFound();
            }
            return View(sinhHoaMau);
        }

        // POST: Admin/SinhHoaMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,NameTest,CSBT,Result")] SinhHoaMau sinhHoaMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhHoaMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sinhHoaMau);
        }

        // GET: Admin/SinhHoaMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhHoaMau sinhHoaMau = db.SinhHoaMaus.Find(id);
            if (sinhHoaMau == null)
            {
                return HttpNotFound();
            }
            return View(sinhHoaMau);
        }

        // POST: Admin/SinhHoaMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SinhHoaMau sinhHoaMau = db.SinhHoaMaus.Find(id);
            db.SinhHoaMaus.Remove(sinhHoaMau);
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
