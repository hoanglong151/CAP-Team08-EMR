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
    public class Detail_HoHapController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_HoHap
        public ActionResult Index()
        {
            var detail_HoHap = db.Detail_HoHap.Include(d => d.HoHap).Include(d => d.InformationExamination);
            return View(detail_HoHap.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listHoHap = db.Detail_HoHap.ToList();
            foreach (var item in multiplesModel.HoHap)
            {
                var checkExistDetail1 = listHoHap.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.HoHap_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_HoHap detail_HoHap = new Detail_HoHap();
                    detail_HoHap.HoHap_ID = item.ID;
                    detail_HoHap.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_HoHap.Add(detail_HoHap);
                    db.SaveChanges();
                }
                if (checkExistDetail1.HoHap.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listHoHap)
            {
                var checkDelete = multiplesModel.HoHap.FirstOrDefault(p => p.ID == item1.HoHap_ID);
                if (checkDelete == null)
                {
                    db.Detail_HoHap.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_HoHap> detail_HoHaps = db.Detail_HoHap.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_HoHaps = detail_HoHaps;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_HoHap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HoHap detail_HoHap = db.Detail_HoHap.Find(id);
            if (detail_HoHap == null)
            {
                return HttpNotFound();
            }
            return View(detail_HoHap);
        }

        // GET: Admin/Detail_HoHap/Create
        public ActionResult Create()
        {
            ViewBag.HoHap_ID = new SelectList(db.HoHaps, "ID", "Name");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_HoHap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,HoHap_ID")] Detail_HoHap detail_HoHap)
        {
            if (ModelState.IsValid)
            {
                db.Detail_HoHap.Add(detail_HoHap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HoHap_ID = new SelectList(db.HoHaps, "ID", "Name", detail_HoHap.HoHap_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_HoHap.InformationExamination_ID);
            return View(detail_HoHap);
        }

        // GET: Admin/Detail_HoHap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HoHap detail_HoHap = db.Detail_HoHap.Find(id);
            if (detail_HoHap == null)
            {
                return HttpNotFound();
            }
            ViewBag.HoHap_ID = new SelectList(db.HoHaps, "ID", "Name", detail_HoHap.HoHap_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_HoHap.InformationExamination_ID);
            return View(detail_HoHap);
        }

        // POST: Admin/Detail_HoHap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,HoHap_ID")] Detail_HoHap detail_HoHap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_HoHap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HoHap_ID = new SelectList(db.HoHaps, "ID", "Name", detail_HoHap.HoHap_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_HoHap.InformationExamination_ID);
            return View(detail_HoHap);
        }

        // GET: Admin/Detail_HoHap/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_HoHap detail_HoHap = db.Detail_HoHap.Find(id);
            if (detail_HoHap == null)
            {
                return HttpNotFound();
            }
            return View(detail_HoHap);
        }

        // POST: Admin/Detail_HoHap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_HoHap detail_HoHap = db.Detail_HoHap.Find(id);
            db.Detail_HoHap.Remove(detail_HoHap);
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
