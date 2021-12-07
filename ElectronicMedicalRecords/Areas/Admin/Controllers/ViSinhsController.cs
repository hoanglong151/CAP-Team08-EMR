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
    public class ViSinhsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/ViSinhs
        public ActionResult Index()
        {
            return View(db.ViSinhs.ToList());
        }

        // GET: Admin/CTMaus/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.ViSinh = db.ViSinhs.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/ViSinhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViSinh viSinh = db.ViSinhs.Find(id);
            if (viSinh == null)
            {
                return HttpNotFound();
            }
            return View(viSinh);
        }

        // GET: Admin/ViSinhs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ViSinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NameTest,ChiDinh,Result,ResultNC,ResultDD,NongDo")] ViSinh viSinh)
        {
            if (ModelState.IsValid)
            {
                db.ViSinhs.Add(viSinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viSinh);
        }

        // GET: Admin/ViSinhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViSinh viSinh = db.ViSinhs.Find(id);
            if (viSinh == null)
            {
                return HttpNotFound();
            }
            return View(viSinh);
        }

        // POST: Admin/ViSinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NameTest,ChiDinh,Result,ResultNC,ResultDD,NongDo")] ViSinh viSinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viSinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viSinh);
        }

        // GET: Admin/ViSinhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViSinh viSinh = db.ViSinhs.Find(id);
            if (viSinh == null)
            {
                return HttpNotFound();
            }
            return View(viSinh);
        }

        // POST: Admin/ViSinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViSinh viSinh = db.ViSinhs.Find(id);
            db.ViSinhs.Remove(viSinh);
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
