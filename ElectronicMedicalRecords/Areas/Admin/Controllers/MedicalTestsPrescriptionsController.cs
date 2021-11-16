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
    public class MedicalTestsPrescriptionsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/MedicalTestsPrescriptions
        public ActionResult Index()
        {
            var medicalTestsPrescriptions = db.MedicalTestsPrescriptions.Include(m => m.InformationExamination);
            return View(medicalTestsPrescriptions.ToList());
        }

        // GET: Admin/MedicalTestsPrescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalTestsPrescription medicalTestsPrescription = db.MedicalTestsPrescriptions.Find(id);
            if (medicalTestsPrescription == null)
            {
                return HttpNotFound();
            }
            return View(medicalTestsPrescription);
        }

        // GET: Admin/MedicalTestsPrescriptions/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/MedicalTestsPrescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<MedicalTestsPrescription> medicalTestsPrescription, int patientID)
        {
            foreach(var item in medicalTestsPrescription)
            {
                if (ModelState.IsValid)
                {
                    if(item.Name != null || item.Result != null)
                    {
                        item.InformationExamination_ID = patientID;
                        db.MedicalTestsPrescriptions.Add(item);
                        db.SaveChanges();
                    }
                    continue;
                    //return RedirectToAction("Index");
                }
            }
            return View(medicalTestsPrescription);
        }

        // GET: Admin/MedicalTestsPrescriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalTestsPrescription medicalTestsPrescription = db.MedicalTestsPrescriptions.Find(id);
            if (medicalTestsPrescription == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", medicalTestsPrescription.InformationExamination_ID);
            return View(medicalTestsPrescription);
        }

        // POST: Admin/MedicalTestsPrescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Result,InformationExamination_ID")] MedicalTestsPrescription medicalTestsPrescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalTestsPrescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", medicalTestsPrescription.InformationExamination_ID);
            return View(medicalTestsPrescription);
        }

        // GET: Admin/MedicalTestsPrescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalTestsPrescription medicalTestsPrescription = db.MedicalTestsPrescriptions.Find(id);
            if (medicalTestsPrescription == null)
            {
                return HttpNotFound();
            }
            return View(medicalTestsPrescription);
        }

        // POST: Admin/MedicalTestsPrescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalTestsPrescription medicalTestsPrescription = db.MedicalTestsPrescriptions.Find(id);
            db.MedicalTestsPrescriptions.Remove(medicalTestsPrescription);
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
