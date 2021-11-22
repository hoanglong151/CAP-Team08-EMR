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
    public class ImmunesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Immunes
        public ActionResult Index()
        {
            return View(db.Immunes.ToList());
        }

        // GET: Admin/Immunes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Immune immune = db.Immunes.Find(id);
            if (immune == null)
            {
                return HttpNotFound();
            }
            return View(immune);
        }

        // GET: Admin/Immunes/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Immune = db.Immunes.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/Immunes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit")] Immune immune)
        {
            if (ModelState.IsValid)
            {
                db.Immunes.Add(immune);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(immune);
        }

        // GET: Admin/Immunes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Immune immune = db.Immunes.Find(id);
            if (immune == null)
            {
                return HttpNotFound();
            }
            return View(immune);
        }

        // POST: Admin/Immunes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit")] Immune immune)
        {
            if (ModelState.IsValid)
            {
                db.Entry(immune).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(immune);
        }

        // GET: Admin/Immunes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Immune immune = db.Immunes.Find(id);
            if (immune == null)
            {
                return HttpNotFound();
            }
            return View(immune);
        }

        // POST: Admin/Immunes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Immune immune = db.Immunes.Find(id);
            db.Immunes.Remove(immune);
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
