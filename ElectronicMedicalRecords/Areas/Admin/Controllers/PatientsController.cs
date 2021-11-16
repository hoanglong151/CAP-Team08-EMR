using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    public class PatientsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.Gender).Include(p => p.HomeTown).Include(p => p.Nation).Include(p => p.Nation1);
            return View(patients.ToList());
        }

        // GET: Admin/Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Admin/Patients/Create
        public ActionResult Create()
        {
            InformationExaminationsController controller = new InformationExaminationsController();
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(id => id.UserID == UserID);
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1");
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1");
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name");
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name");
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name");
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewBag.NameMedication = db.Medications.ToList();
            ViewBag.DateExamination = DateTime.Now;
            return View();
        }

        // POST: Admin/Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient, InformationExamination informationExamination, List<MedicalTestsPrescription> medicalTestsPrescription)
        {
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                informationExaminationsController.Create(informationExamination, patient.ID, medicalTestsPrescription);
                return RedirectToAction("Index");
            }

            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return View(patient);
        }

        // GET: Admin/Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return View(patient);
        }

        // POST: Admin/Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Gender_ID,BirthDate,Religion_ID,Address,HomeTown_ID,Nation_ID,Phone,InsuranceCode,MedicalHistory,HistoryDisease")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return View(patient);
        }

        // GET: Admin/Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Admin/Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
