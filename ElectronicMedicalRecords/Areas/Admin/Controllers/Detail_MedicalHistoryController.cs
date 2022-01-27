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
    public class Detail_MedicalHistoryController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_MedicalHistory
        public ActionResult Index()
        {
            var detail_MedicalHistory = db.Detail_MedicalHistory.Include(d => d.Patient).Include(d => d.MedicalHistory);
            return View(detail_MedicalHistory.ToList());
        }

        // GET: Admin/Detail_MedicalHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_MedicalHistory detail_MedicalHistory = db.Detail_MedicalHistory.Find(id);
            if (detail_MedicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(detail_MedicalHistory);
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = new Patient();
            patient.ID = id;
            List<Detail_MedicalHistory> detail_MedicalHistories = db.Detail_MedicalHistory.Where(p => p.Patient_ID == id).ToList();
            multiplesModel.Detail_MedicalHistories = detail_MedicalHistories;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = new Patient();
            patient.ID = id;
            List<Detail_MedicalHistory> detail_MedicalHistories = db.Detail_MedicalHistory.Where(p => p.Patient_ID == id).ToList();
            multiplesModel.Detail_MedicalHistories = detail_MedicalHistories;
            return PartialView("_BillCheck", multiplesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var list = db.MedicalHistories.ToList();
            var listMedicalHistory = db.Detail_MedicalHistory.ToList();
            foreach (var item in list)
            {
                var checkExist = multiplesModel.MedicalHistories.FirstOrDefault(p => p.ID == item.ID);
                if(checkExist != null)
                {
                    var checkExistDetail1 = listMedicalHistory.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.MedicalHistory_ID == checkExist.ID);
                    if(checkExistDetail1 != null)
                    {
                        db.Entry(checkExistDetail1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Detail_MedicalHistory detail_MedicalHistory = new Detail_MedicalHistory();
                        detail_MedicalHistory.MedicalHistory_ID = checkExist.ID;
                        detail_MedicalHistory.Patient_ID = multiplesModel.Patient.ID;
                        detail_MedicalHistory.Selected = checkExist.Selected;
                        db.Detail_MedicalHistory.Add(detail_MedicalHistory);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var checkExistDetail = listMedicalHistory.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.MedicalHistory_ID == item.ID);
                    if(checkExistDetail != null)
                    {
                        db.Detail_MedicalHistory.Remove(checkExistDetail);
                        db.SaveChanges();
                    }
                }
            }
            //ViewBag.Immune_ID = new SelectList(db.Immunes, "ID", "NameTest", detail_Immune.Immue_ID);
            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Immune.InformationExamination_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_MedicalHistory/Create
        public ActionResult Create()
        {
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.MedicalHistory_ID = new SelectList(db.MedicalHistories, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_MedicalHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int patientID, MultiplesModel multiplesModel)
        {
            foreach(var item in multiplesModel.MedicalHistories)
            {
                if (ModelState.IsValid && item.Selected == true)
                {
                    Detail_MedicalHistory detail_MedicalHistory = new Detail_MedicalHistory();
                    detail_MedicalHistory.MedicalHistory_ID = item.ID;
                    detail_MedicalHistory.Patient_ID = patientID;
                    detail_MedicalHistory.Selected = item.Selected;

                    db.Detail_MedicalHistory.Add(detail_MedicalHistory);
                    db.SaveChanges();
                }
            }
            //ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_MedicalHistory.Patient_ID);
            //ViewBag.MedicalHistory_ID = new SelectList(db.MedicalHistories, "ID", "Name", detail_MedicalHistory.MedicalHistory_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_MedicalHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_MedicalHistory detail_MedicalHistory = db.Detail_MedicalHistory.Find(id);
            if (detail_MedicalHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_MedicalHistory.Patient_ID);
            ViewBag.MedicalHistory_ID = new SelectList(db.MedicalHistories, "ID", "Name", detail_MedicalHistory.MedicalHistory_ID);
            return View(detail_MedicalHistory);
        }

        // POST: Admin/Detail_MedicalHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Detail_MedicalHistory detail_MedicalHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_MedicalHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_MedicalHistory.Patient_ID);
            ViewBag.MedicalHistory_ID = new SelectList(db.MedicalHistories, "ID", "Name", detail_MedicalHistory.MedicalHistory_ID);
            return View(detail_MedicalHistory);
        }

        // GET: Admin/Detail_MedicalHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_MedicalHistory detail_MedicalHistory = db.Detail_MedicalHistory.Find(id);
            if (detail_MedicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(detail_MedicalHistory);
        }

        // POST: Admin/Detail_MedicalHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_MedicalHistory detail_MedicalHistory = db.Detail_MedicalHistory.Find(id);
            db.Detail_MedicalHistory.Remove(detail_MedicalHistory);
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
