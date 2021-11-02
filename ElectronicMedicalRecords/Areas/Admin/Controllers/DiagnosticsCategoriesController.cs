﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using ExcelDataReader;
using OfficeOpenXml;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    public class DiagnosticsCategoriesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/DiagnosticsCategories
        public ActionResult Index()
        {
            return View(db.DiagnosticsCategories.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var diagnostic = db.DiagnosticsCategories.ToList();
            return Json(new { data = diagnostic }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/DiagnosticsCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            if (diagnosticsCategory == null)
            {
                return HttpNotFound();
            }
            return View(diagnosticsCategory);
        }

        // GET: Admin/DiagnosticsCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DiagnosticsCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiagnosticsCategory diagnosticsCategory)
        {
            if (ModelState.IsValid)
            {
                db.DiagnosticsCategories.Add(diagnosticsCategory);
                db.SaveChanges();
                return Json(new { success = true });
                //return RedirectToAction("Index");
            }
            return View(diagnosticsCategory);
        }

        // GET: Admin/DiagnosticsCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            if (diagnosticsCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = diagnosticsCategory }, JsonRequestBehavior.AllowGet);
            //return View(diagnosticsCategory);
        }

        // POST: Admin/DiagnosticsCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiagnosticsCategory diagnosticsCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diagnosticsCategory).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
                //return RedirectToAction("Index");
            }
            return View(diagnosticsCategory);
        }

        // GET: Admin/DiagnosticsCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            if (diagnosticsCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = diagnosticsCategory }, JsonRequestBehavior.AllowGet);
            //return View(diagnosticsCategory);
        }

        // POST: Admin/DiagnosticsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            db.DiagnosticsCategories.Remove(diagnosticsCategory);
            db.SaveChanges();
            return Json(new { success = true });
            //return RedirectToAction("Index");
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
