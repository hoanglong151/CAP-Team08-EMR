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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class DongMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/DongMaus
        public ActionResult Index()
        {
            return View(db.DongMaus.ToList());
        }

        // GET: Admin/DongMaus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DongMau dongMau = db.DongMaus.Find(id);
            if (dongMau == null)
            {
                return HttpNotFound();
            }
            return View(dongMau);
        }

        // GET: Admin/DongMaus/Create
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.DongMau = db.DongMaus.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/DongMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.DongMau = db.DongMaus.ToList();
            return PartialView("_Create", multiplesModel); 
        }

        // POST: Admin/DongMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit")] DongMau dongMau)
        {
            if (ModelState.IsValid)
            {
                db.DongMaus.Add(dongMau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dongMau);
        }

        // GET: Admin/DongMaus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DongMau dongMau = db.DongMaus.Find(id);
            if (dongMau == null)
            {
                return HttpNotFound();
            }
            return View(dongMau);
        }

        // POST: Admin/DongMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChiDinh,NameTest,Result,CSBT,Unit")] DongMau dongMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dongMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dongMau);
        }

        // GET: Admin/DongMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DongMau dongMau = db.DongMaus.Find(id);
            if (dongMau == null)
            {
                return HttpNotFound();
            }
            return View(dongMau);
        }

        // POST: Admin/DongMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DongMau dongMau = db.DongMaus.Find(id);
            db.DongMaus.Remove(dongMau);
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
