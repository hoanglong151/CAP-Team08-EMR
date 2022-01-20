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

        // GET: Admin/Detail_HistoryDisease/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HistoryDisease detail_HistoryDisease = db.Detail_HistoryDisease.Find(id);
            if (detail_HistoryDisease == null)
            {
                return HttpNotFound();
            }
            return View(detail_HistoryDisease);
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

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Patient patient = new Patient();
            patient.ID = id;
            List<Detail_HistoryDisease> detail_HistoryDiseases1 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Ông/Bà").ToList();
            List<Detail_HistoryDisease> detail_HistoryDiseases2 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Cha/Mẹ").ToList();
            List<Detail_HistoryDisease> detail_HistoryDiseases3 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Anh/Chị em").ToList();
            multiplesModel.Detail_HistoryDiseases1 = detail_HistoryDiseases1;
            multiplesModel.Detail_HistoryDiseases2 = detail_HistoryDiseases2;
            multiplesModel.Detail_HistoryDiseases3 = detail_HistoryDiseases3;
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
                        detail_HistoryDisease.Selected = checkExist.Selected;
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
                        detail_HistoryDisease2.Selected = checkExist2.Selected;
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
                        detail_HistoryDisease3.Selected = checkExist3.Selected;
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

        // GET: Admin/Detail_HistoryDisease/Create
        public ActionResult Create()
        {
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name");
            ViewBag.HistoryDisease_ID = new SelectList(db.HistoryDiseases, "ID", "Name");
            return View();
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
                if (ModelState.IsValid && item1.Selected == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease1 = new Detail_HistoryDisease();
                    detail_HistoryDisease1.HistoryDisease_ID = item1.ID;
                    detail_HistoryDisease1.Patient_ID = patientID;
                    detail_HistoryDisease1.LevelFamily = item1.LevelFamily;
                    detail_HistoryDisease1.Selected = item1.Selected;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease1);
                    db.SaveChanges();
                }
            }
            foreach (var item2 in multiplesModel.HistoryDiseases2)
            {
                if (ModelState.IsValid && item2.Selected == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease2 = new Detail_HistoryDisease();
                    detail_HistoryDisease2.HistoryDisease_ID = item2.ID;
                    detail_HistoryDisease2.Patient_ID = patientID;
                    detail_HistoryDisease2.LevelFamily = item2.LevelFamily;
                    detail_HistoryDisease2.Selected = item2.Selected;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease2);
                    db.SaveChanges();
                }
            }
            foreach (var item3 in multiplesModel.HistoryDiseases3)
            {
                if (ModelState.IsValid && item3.Selected == true)
                {
                    Detail_HistoryDisease detail_HistoryDisease3 = new Detail_HistoryDisease();
                    detail_HistoryDisease3.HistoryDisease_ID = item3.ID;
                    detail_HistoryDisease3.Patient_ID = patientID;
                    detail_HistoryDisease3.LevelFamily = item3.LevelFamily;
                    detail_HistoryDisease3.Selected = item3.Selected;

                    db.Detail_HistoryDisease.Add(detail_HistoryDisease3);
                    db.SaveChanges();
                }
            }
            //ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_HistoryDisease.Patient_ID);
            //ViewBag.HistoryDisease_ID = new SelectList(db.HistoryDiseases, "ID", "Name", detail_HistoryDisease.HistoryDisease_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_HistoryDisease/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HistoryDisease detail_HistoryDisease = db.Detail_HistoryDisease.Find(id);
            if (detail_HistoryDisease == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_HistoryDisease.Patient_ID);
            ViewBag.HistoryDisease_ID = new SelectList(db.HistoryDiseases, "ID", "Name", detail_HistoryDisease.HistoryDisease_ID);
            return View(detail_HistoryDisease);
        }

        // POST: Admin/Detail_HistoryDisease/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HistoryDisease_ID,Patient_ID")] Detail_HistoryDisease detail_HistoryDisease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_HistoryDisease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", detail_HistoryDisease.Patient_ID);
            ViewBag.HistoryDisease_ID = new SelectList(db.HistoryDiseases, "ID", "Name", detail_HistoryDisease.HistoryDisease_ID);
            return View(detail_HistoryDisease);
        }

        // GET: Admin/Detail_HistoryDisease/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HistoryDisease detail_HistoryDisease = db.Detail_HistoryDisease.Find(id);
            if (detail_HistoryDisease == null)
            {
                return HttpNotFound();
            }
            return View(detail_HistoryDisease);
        }

        // POST: Admin/Detail_HistoryDisease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_HistoryDisease detail_HistoryDisease = db.Detail_HistoryDisease.Find(id);
            db.Detail_HistoryDisease.Remove(detail_HistoryDisease);
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
