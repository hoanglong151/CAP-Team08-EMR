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
    public class HoHapsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/HoHaps
        public ActionResult Index()
        {
            return View(db.HoHaps.ToList());
        }

        // GET: Admin/HoHaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoHap hoHap = db.HoHaps.Find(id);
            if (hoHap == null)
            {
                return HttpNotFound();
            }
            return View(hoHap);
        }

        // GET: Admin/HoHaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HoHaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChiDinh,Name,Dangerous")] HoHap hoHap)
        {
            if (ModelState.IsValid)
            {
                db.HoHaps.Add(hoHap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoHap);
        }

        // GET: Admin/HoHaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoHap hoHap = db.HoHaps.Find(id);
            if (hoHap == null)
            {
                return HttpNotFound();
            }
            return View(hoHap);
        }

        // POST: Admin/HoHaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,Name,Dangerous")] HoHap hoHap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoHap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoHap);
        }

        // GET: Admin/HoHaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoHap hoHap = db.HoHaps.Find(id);
            if (hoHap == null)
            {
                return HttpNotFound();
            }
            return View(hoHap);
        }

        // POST: Admin/HoHaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoHap hoHap = db.HoHaps.Find(id);
            db.HoHaps.Remove(hoHap);
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
