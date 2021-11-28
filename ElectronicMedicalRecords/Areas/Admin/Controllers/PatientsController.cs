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
        MultiplesModel multiplesModel = new MultiplesModel();
        // GET: Admin/Patients
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            var patients = db.Patients.Include(p => p.Gender).Include(p => p.HomeTown).Include(p => p.Nation).Include(p => p.Nation1).ToList();
            return View(patients);
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
            return PartialView("_DetailIE", multiplesModel);
        }

        [HttpPost]
        public ActionResult SearchPatient(DateTime? DateStart, DateTime? DateEnd, string Name, string Code)
        {
            PatientsController patientsController = new PatientsController();
            if (DateStart == null && DateEnd == null && Name == "" && Code == null)
            {
                TempData["Error"] = "Vui lòng nhập ít nhất 1 trường";
                return RedirectToAction("Index", "Patients");
            }
            //var findDate = new List<InformationExamination>();
            var informationExaminations = db.InformationExaminations.ToList();
            var patients = new List<Patient>();
            if (Name != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.Name.Contains(Name)).ToList();
            }
            if (DateStart.HasValue)
            {
                informationExaminations = informationExaminations.Where(p => p.DateExamine >= DateStart.Value).ToList();
            }
            if (DateEnd.HasValue)
            {
                informationExaminations = informationExaminations.Where(p => p.DateEnd <= DateEnd.Value).ToList();
            }
            if( Code != "")
            {
                informationExaminations = informationExaminations.Where(p => p.Patient.MaBN.Contains(Code)).ToList();
            }
            if (informationExaminations.Count > 0)
            {
                for (int i = 0; i < informationExaminations.Count; i++)
                {
                    var checkexist = patients.FirstOrDefault(p => p.MaBN == informationExaminations[i].Patient.MaBN);
                    if(checkexist == null)
                    {
                        var patient_ID = informationExaminations[i].Patient_ID;
                        var patient = db.Patients.FirstOrDefault(p => p.ID == patient_ID);
                        patients.Add(patient);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return View("Index", patients);
        }

        // GET: Admin/Patients/Create
        public ActionResult Create()
        {
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1");
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1");
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name");
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name");
            return PartialView("_Create");
        }

        // POST: Admin/Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var listPatient = db.Patients.ToList().LastOrDefault();
                var numPatient =  Convert.ToInt32(listPatient.MaBN.Substring(6));
                if (numPatient < 10)
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "00000" + (++numPatient);
                }
                else if (numPatient < 100)
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "0000" + (++numPatient);
                }
                else if (numPatient < 1000)
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "000" + (++numPatient);
                }
                else if (numPatient < 10000)
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "00" + (++numPatient);
                }
                else if (numPatient < 100000)
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + "0" + (++numPatient);
                }
                else
                {
                    patient.MaBN = "BN" + DateTime.Now.Year + (++numPatient);
                }
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }

            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return View(patient);
        }

        // GET: Admin/Patients/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CreateOldPatient", "MultipleModels");
            }
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewBag.Nation1_ID = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Patients/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewData["Patient.Gender_ID"] = new SelectList(db.Genders, "ID", "Gender1", patient.Gender_ID);
            ViewData["Patient.HomeTown_ID"] = new SelectList(db.HomeTowns, "ID", "HomeTown1", patient.HomeTown_ID);
            ViewData["Patient.Nation_ID"] = new SelectList(db.Nations, "ID", "Name", patient.Nation_ID);
            ViewData["Patient.Nation1_ID"] = new SelectList(db.Nation1, "ID", "Name", patient.Nation1_ID);
            multiplesModel.Patient = patient;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "MultipleModels");
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
