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
    public class Detail_CTMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_CTMau
        public ActionResult Index()
        {
            var detail_CTMau = db.Detail_CTMau.Include(d => d.CTMau).Include(d => d.InformationExamination);
            return View(detail_CTMau.ToList());
        }

        // GET: Admin/Detail_CTMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // GET: Admin/Detail_CTMau/Create
        public ActionResult Create()
        {
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_CTMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<CTMau> cTMaus, int id)
        {
            Detail_CTMau detail_CTMau = new Detail_CTMau();
            foreach(var item in cTMaus)
            {
                if (ModelState.IsValid)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = id;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    detail_CTMau.Result = item.Result;
                    db.Detail_CTMau.Add(detail_CTMau);
                    db.SaveChanges();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Index");
        }

        // GET: Admin/Detail_CTMau/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            return View(detail_CTMau);
        }

        // POST: Admin/Detail_CTMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,CTMau_ID")] Detail_CTMau detail_CTMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_CTMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            return View(detail_CTMau);
        }

        // GET: Admin/Detail_CTMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // POST: Admin/Detail_CTMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            db.Detail_CTMau.Remove(detail_CTMau);
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
