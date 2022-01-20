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
    public class HistoryDiseasesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/HistoryDiseases
        public ActionResult Index()
        {
            return View(db.HistoryDiseases.ToList());
        }

        // GET: Admin/HistoryDiseases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            if (historyDisease == null)
            {
                return HttpNotFound();
            }
            return View(historyDisease);
        }

        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listHistoryDiseases1 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases2 = db.HistoryDiseases.AsNoTracking().ToList();
            var listHistoryDiseases3 = db.HistoryDiseases.AsNoTracking().ToList();
            var detail_HistoryDiseases1 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Ông/Bà");
            var detail_HistoryDiseases2 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Cha/Mẹ");
            var detail_HistoryDiseases3 = db.Detail_HistoryDisease.Where(p => p.Patient_ID == id && p.LevelFamily == "Anh/Chị em");
            foreach (var item1 in listHistoryDiseases1)
            {
                var historyDisease1 = detail_HistoryDiseases1.FirstOrDefault(p => p.HistoryDisease_ID == item1.ID);
                if(historyDisease1 != null)
                {
                    item1.Selected = historyDisease1.Selected;
                }
            }
            multiplesModel.HistoryDiseases1 = listHistoryDiseases1;

            foreach (var item2 in listHistoryDiseases2)
            {
                var historyDisease2 = detail_HistoryDiseases2.FirstOrDefault(p => p.HistoryDisease_ID == item2.ID);
                if (historyDisease2 != null)
                {
                    item2.Selected = historyDisease2.Selected;
                }
            }
            multiplesModel.HistoryDiseases2 = listHistoryDiseases2;

            foreach (var item3 in listHistoryDiseases3)
            {
                var historyDisease3 = detail_HistoryDiseases3.FirstOrDefault(p => p.HistoryDisease_ID == item3.ID);
                if (historyDisease3 != null)
                {
                    item3.Selected = historyDisease3.Selected;
                }
            }
            multiplesModel.HistoryDiseases3 = listHistoryDiseases3;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/HistoryDiseases/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.HistoryDiseases1 = db.HistoryDiseases.ToList();
            multiplesModel.HistoryDiseases2 = db.HistoryDiseases.ToList();
            multiplesModel.HistoryDiseases3 = db.HistoryDiseases.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/HistoryDiseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Dangerous")] HistoryDisease historyDisease)
        {
            if (ModelState.IsValid)
            {
                db.HistoryDiseases.Add(historyDisease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(historyDisease);
        }

        // GET: Admin/HistoryDiseases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            if (historyDisease == null)
            {
                return HttpNotFound();
            }
            return View(historyDisease);
        }

        // POST: Admin/HistoryDiseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Dangerous")] HistoryDisease historyDisease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historyDisease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(historyDisease);
        }

        // GET: Admin/HistoryDiseases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            if (historyDisease == null)
            {
                return HttpNotFound();
            }
            return View(historyDisease);
        }

        // POST: Admin/HistoryDiseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistoryDisease historyDisease = db.HistoryDiseases.Find(id);
            db.HistoryDiseases.Remove(historyDisease);
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
