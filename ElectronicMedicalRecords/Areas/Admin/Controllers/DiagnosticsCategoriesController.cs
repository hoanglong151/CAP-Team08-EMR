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
            var diagnostic = db.DiagnosticsCategories.ToList();
            return Json(new { data = diagnostic }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Diagnostic");
                ws.Cells["A1"].LoadFromCollection(db.DiagnosticsCategories, true);
                // Load your collection "accounts"

                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Diagnostic.xlsx");
                // Replace filename with your custom Excel-sheet name.

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportExcel(HttpPostedFileBase[] uploads)
        {
            DataTable dt = new DataTable();
            IExcelDataReader reader = null;
            DataSet result = new DataSet();
            for (int k = 0; k < uploads.Length; k++)
            {
                if (ModelState.IsValid)
                {

                    if (uploads[k] != null && uploads[k].ContentLength > 0)
                    {
                        // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                        // to get started. This is how we avoid dependencies on ACE or Interop:
                        Stream stream = uploads[k].InputStream;

                        if (uploads[k].FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (uploads[k].FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View();
                        }
                        int fieldcount = reader.FieldCount;
                        int rowcount = reader.RowCount;
                        DataRow row;
                        DataTable dt_ = new DataTable();
                        try
                        {
                            dt_ = reader.AsDataSet().Tables[0];
                            if (dt.Columns.Count == 0)
                            {
                                for (int i = 0; i < dt_.Columns.Count; i++)
                                {
                                    dt.Columns.Add(dt_.Rows[0][i].ToString());
                                }
                            }
                            int rowcounter = 0;
                            for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                            {
                                row = dt.NewRow();

                                for (int col = 0; col < dt_.Columns.Count; col++)
                                {
                                    row[col] = dt_.Rows[row_][col].ToString();
                                    rowcounter++;
                                }
                                dt.Rows.Add(row);
                            }

                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("File", "Unable to Upload file!");
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
            }
            dt = dt.Rows.Cast<DataRow>()
            .Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string)))
            .CopyToDataTable();

            result.Tables.Add(dt);
            reader.Close();
            reader.Dispose();
            DataTable tmp = result.Tables[0];
            Session["tmpdata"] = tmp;  //store datatable into session

            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo.DiagnosticsCategory";

                    //[OPTIONAL]: Map the Excel columns with that of the database table
                    sqlBulkCopy.ColumnMappings.Add("Mã", "Code");
                    sqlBulkCopy.ColumnMappings.Add("Tên bệnh", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Tên bệnh tiếng Anh", "NameEnglish");
                    sqlBulkCopy.ColumnMappings.Add("MDC ", "MDC");

                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            return RedirectToAction("Index");
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
