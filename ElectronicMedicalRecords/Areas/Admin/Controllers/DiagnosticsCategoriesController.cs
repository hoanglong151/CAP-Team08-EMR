using System;
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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class DiagnosticsCategoriesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/DiagnosticsCategories
        public ActionResult Index()
        {
            var diagnostics = db.DiagnosticsCategories.ToList();
            return View(diagnostics);
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var diagnostic = db.DiagnosticsCategories.ToList();
            return Json(new { data = diagnostic}, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DiagnosticsCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiagnosticsCategory diagnosticsCategory)
        {
            var text = ValidateForm(diagnosticsCategory);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    db.DiagnosticsCategories.Add(diagnosticsCategory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(diagnosticsCategory);
            }
            return Json(new { success = false, responseText = text });
        }

        public string ValidateForm(DiagnosticsCategory diagnosticsCategory)
        {
            string text = "";
            var checkExist = db.DiagnosticsCategories.FirstOrDefault(e => e.Code == diagnosticsCategory.Code);
            if (checkExist != null && diagnosticsCategory.Name != null && diagnosticsCategory.MDC != null && diagnosticsCategory.NameEnglish != null)
            {
                text = "Chẩn đoán đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(DiagnosticsCategory diagnosticsCategory)
        {
            string text = "";
            var checkExist = db.DiagnosticsCategories.FirstOrDefault(e => e.Code == diagnosticsCategory.Code);
            if (checkExist != null && checkExist.ID != diagnosticsCategory.ID && diagnosticsCategory.Name != null && diagnosticsCategory.MDC != null && diagnosticsCategory.NameEnglish != null)
            {
                text = "Chẩn đoán đã có trong danh sách";
            }
            return text;
        }

        [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
        // GET: Admin/DiagnosticsCategories/Edit/5
        public ActionResult Edit(int id)
        {
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            if (diagnosticsCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = diagnosticsCategory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DiagnosticsCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiagnosticsCategory diagnosticsCategory)
        {
            var text = ValidateFormUpdate(diagnosticsCategory);
            if(text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.DiagnosticsCategories.Find(diagnosticsCategory.ID);
                    db.Entry(existData).CurrentValues.SetValues(diagnosticsCategory);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(diagnosticsCategory);
            }
            return Json(new { success = false, responseText = text });
        }
        [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
        // GET: Admin/DiagnosticsCategories/Delete/5
        public ActionResult Delete(int id)
        {
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            if (diagnosticsCategory == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = diagnosticsCategory }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DiagnosticsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiagnosticsCategory diagnosticsCategory = db.DiagnosticsCategories.Find(id);
            try
            {
                db.DiagnosticsCategories.Remove(diagnosticsCategory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = "Danh mục chẩn đoán này đang được sử dụng" });
            }
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
