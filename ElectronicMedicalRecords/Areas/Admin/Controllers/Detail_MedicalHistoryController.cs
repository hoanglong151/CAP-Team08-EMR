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

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = new Patient();
            patient.ID = id;
            List<Detail_MedicalHistory> detail_MedicalHistories = db.Detail_MedicalHistory.Where(p => p.Patient_ID == id).ToList();
            multiplesModel.Detail_MedicalHistories = detail_MedicalHistories;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
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
                        detail_MedicalHistory.Selected = checkExist.ChiDinh;
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
            return RedirectToAction("Create", "MultipleModels");
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
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    Detail_MedicalHistory detail_MedicalHistory = new Detail_MedicalHistory();
                    detail_MedicalHistory.MedicalHistory_ID = item.ID;
                    detail_MedicalHistory.Patient_ID = patientID;
                    detail_MedicalHistory.Selected = item.ChiDinh;

                    db.Detail_MedicalHistory.Add(detail_MedicalHistory);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Create", "MultipleModels");
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
