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
    public class AmniocentesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Amniocentes
        public ActionResult Index()
        {
            return View(db.Amniocentes.ToList());
        }

        // GET: Admin/Amniocentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amniocente amniocente = db.Amniocentes.Find(id);
            if (amniocente == null)
            {
                return HttpNotFound();
            }
            return View(amniocente);
        }

        // GET: Admin/Amniocentes/CreateOldPatient
        public ActionResult CreateOldPatient()
        {

            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Amniocente = db.Amniocentes.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/Amniocentes/Create
        public ActionResult Create()
        {

            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Amniocente = db.Amniocentes.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/Amniocentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Amniocente amniocente)
        {
            if (ModelState.IsValid)
            {
                db.Amniocentes.Add(amniocente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(amniocente);
        }

        // GET: Admin/Amniocentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amniocente amniocente = db.Amniocentes.Find(id);
            if (amniocente == null)
            {
                return HttpNotFound();
            }
            return View(amniocente);
        }

        // POST: Admin/Amniocentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit")] Amniocente amniocente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(amniocente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(amniocente);
        }

        // GET: Admin/Amniocentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amniocente amniocente = db.Amniocentes.Find(id);
            if (amniocente == null)
            {
                return HttpNotFound();
            }
            return View(amniocente);
        }

        // POST: Admin/Amniocentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Amniocente amniocente = db.Amniocentes.Find(id);
            db.Amniocentes.Remove(amniocente);
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
