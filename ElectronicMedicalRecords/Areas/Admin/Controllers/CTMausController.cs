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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
    public class CTMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/CTMaus
        public ActionResult Index()
        {
            return View(db.CTMaus.ToList());
        }

        // GET: Admin/CTMaus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTMau cTMau = db.CTMaus.Find(id);
            if (cTMau == null)
            {
                return HttpNotFound();
            }
            return View(cTMau);
        }

        // GET: Admin/CTMaus/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.CTMau = db.CTMaus.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/CTMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.CTMau = db.CTMaus.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/CTMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CTMau cTMau)
        {
            if (ModelState.IsValid)
            {
                db.CTMaus.Add(cTMau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cTMau);
        }

        // GET: Admin/CTMaus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTMau cTMau = db.CTMaus.Find(id);
            if (cTMau == null)
            {
                return HttpNotFound();
            }
            return View(cTMau);
        }

        // POST: Admin/CTMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit,MayXN")] CTMau cTMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cTMau);
        }

        // GET: Admin/CTMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTMau cTMau = db.CTMaus.Find(id);
            if (cTMau == null)
            {
                return HttpNotFound();
            }
            return View(cTMau);
        }

        // POST: Admin/CTMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTMau cTMau = db.CTMaus.Find(id);
            db.CTMaus.Remove(cTMau);
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
