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
    public class Detail_HistoryDiseaseController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_HistoryDisease
        public ActionResult Index()
        {
            var detail_HistoryDisease = db.Detail_HistoryDisease.Include(d => d.Patient).Include(d => d.HistoryDisease);
            return View(detail_HistoryDisease.ToList());
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = new Patient();
            patient.ID = id;
            List<Detail_HistoryDisease> detail_HistoryDiseases = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id).ToList();
            multiplesModel.Detail_HistoryDiseases1 = detail_HistoryDiseases;
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
            var listHistoryDiseases1 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases2 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases3 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDisease1 = db.Detail_HistoryDisease.ToList();
            var listHistoryDisease2 = db.Detail_HistoryDisease.ToList();
            var listHistoryDisease3 = db.Detail_HistoryDisease.ToList();

            foreach (var item in listHistoryDiseases1)
            {
                var checkExist = multiplesModel.HistoryDiseases1.FirstOrDefault(p => p.ID == item.ID);
                if (checkExist != null)
                {
                    var checkExistDetail1 = listHistoryDisease1.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == checkExist.ID && p.LevelFamily == "Ông/Bà");
                    if (checkExistDetail1 != null)
                    {
                        db.Entry(checkExistDetail1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Detail_HistoryDisease detail_HistoryDisease = new Detail_HistoryDisease();
                        detail_HistoryDisease.HistoryDisease_ID = checkExist.ID;
                        detail_HistoryDisease.Patient_ID = multiplesModel.Patient.ID;
                        detail_HistoryDisease.Selected = checkExist.ChiDinh;
                        detail_HistoryDisease.LevelFamily = "Ông/Bà";
                        db.Detail_HistoryDisease.Add(detail_HistoryDisease);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var checkExistDetail = listHistoryDisease1.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == item.ID && p.LevelFamily == "Ông/Bà");
                    if (checkExistDetail != null)
                    {
                        db.Detail_HistoryDisease.Remove(checkExistDetail);
                        db.SaveChanges();
                    }
                }
            }

            foreach (var item2 in listHistoryDiseases2)
            {
                var checkExist2 = multiplesModel.HistoryDiseases2.FirstOrDefault(p => p.ID == item2.ID);
                if (checkExist2 != null)
                {
                    var checkExistDetail2 = listHistoryDisease2.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == checkExist2.ID && p.LevelFamily == "Cha/Mẹ");
                    if (checkExistDetail2 != null)
                    {
                        db.Entry(checkExistDetail2).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Detail_HistoryDisease detail_HistoryDisease2 = new Detail_HistoryDisease();
                        detail_HistoryDisease2.HistoryDisease_ID = checkExist2.ID;
                        detail_HistoryDisease2.Patient_ID = multiplesModel.Patient.ID;
                        detail_HistoryDisease2.Selected = checkExist2.ChiDinh;
                        detail_HistoryDisease2.LevelFamily = "Cha/Mẹ";
                        db.Detail_HistoryDisease.Add(detail_HistoryDisease2);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var checkExistDetail2 = listHistoryDisease2.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == item2.ID && p.LevelFamily == "Cha/Mẹ");
                    if (checkExistDetail2 != null)
                    {
                        db.Detail_HistoryDisease.Remove(checkExistDetail2);
                        db.SaveChanges();
                    }
                }
            }

            foreach (var item3 in listHistoryDiseases3)
            {
                var checkExist3 = multiplesModel.HistoryDiseases3.FirstOrDefault(p => p.ID == item3.ID);
                if (checkExist3 != null)
                {
                    var checkExistDetail3 = listHistoryDisease3.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == checkExist3.ID && p.LevelFamily == "Anh/Chị em");
                    if (checkExistDetail3 != null)
                    {
                        db.Entry(checkExistDetail3).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Detail_HistoryDisease detail_HistoryDisease3 = new Detail_HistoryDisease();
                        detail_HistoryDisease3.HistoryDisease_ID = checkExist3.ID;
                        detail_HistoryDisease3.Patient_ID = multiplesModel.Patient.ID;
                        detail_HistoryDisease3.Selected = checkExist3.ChiDinh;
                        detail_HistoryDisease3.LevelFamily = "Anh/Chị em";
                        db.Detail_HistoryDisease.Add(detail_HistoryDisease3);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var checkExistDetail3 = listHistoryDisease3.FirstOrDefault(p => p.Patient_ID == multiplesModel.Patient.ID && p.HistoryDisease_ID == item3.ID && p.LevelFamily == "Anh/Chị em");
                    if (checkExistDetail3 != null)
                    {
                        db.Detail_HistoryDisease.Remove(checkExistDetail3);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Create", "MultipleModels");
        }

        // POST: Admin/Detail_HistoryDisease/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int patientID, MultiplesModel multiplesModel)
        {
            foreach(var item1 in multiplesModel.HistoryDiseases1)
            {
                if (ModelState.IsValid && item1.ChiDinh == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease1 = new Detail_HistoryDisease();
                    detail_HistoryDisease1.HistoryDisease_ID = item1.ID;
                    detail_HistoryDisease1.Patient_ID = patientID;
                    detail_HistoryDisease1.LevelFamily = "Ông/Bà";
                    detail_HistoryDisease1.Selected = item1.ChiDinh;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease1);
                    db.SaveChanges();
                }
            }
            foreach (var item2 in multiplesModel.HistoryDiseases2)
            {
                if (ModelState.IsValid && item2.ChiDinh == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease2 = new Detail_HistoryDisease();
                    detail_HistoryDisease2.HistoryDisease_ID = item2.ID;
                    detail_HistoryDisease2.Patient_ID = patientID;
                    detail_HistoryDisease2.LevelFamily = "Cha/Mẹ";
                    detail_HistoryDisease2.Selected = item2.ChiDinh;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease2);
                    db.SaveChanges();
                }
            }
            foreach (var item3 in multiplesModel.HistoryDiseases3)
            {
                if (ModelState.IsValid && item3.ChiDinh == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease3 = new Detail_HistoryDisease();
                    detail_HistoryDisease3.HistoryDisease_ID = item3.ID;
                    detail_HistoryDisease3.Patient_ID = patientID;
                    detail_HistoryDisease3.LevelFamily = "Anh/Chị em";
                    detail_HistoryDisease3.Selected = item3.ChiDinh;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease3);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Create", "MultipleModels");
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
