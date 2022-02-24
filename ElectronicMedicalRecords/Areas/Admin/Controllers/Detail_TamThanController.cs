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
    public class Detail_TamThanController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TamThan
        public ActionResult Index()
        {
            var detail_TamThan = db.Detail_TamThan.Include(d => d.InformationExamination).Include(d => d.TamThan);
            return View(detail_TamThan.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTamThan = db.Detail_TamThan.ToList();
            foreach (var item in multiplesModel.TamThan)
            {
                var checkExistDetail1 = listTamThan.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TamThan_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TamThan detail_TamThan = new Detail_TamThan();
                    detail_TamThan.TamThan_ID = item.ID;
                    detail_TamThan.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TamThan.Add(detail_TamThan);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listTamThan)
            {
                var checkDelete = multiplesModel.TamThan.FirstOrDefault(p => p.ID == item1.TamThan_ID);
                if (checkDelete == null)
                {
                    db.Detail_TamThan.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_TamThan> detail_TamThans = db.Detail_TamThan.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_TamThans = detail_TamThans;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_TamThan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TamThan detail_TamThan = db.Detail_TamThan.Find(id);
            if (detail_TamThan == null)
            {
                return HttpNotFound();
            }
            return View(detail_TamThan);
        }

        // GET: Admin/Detail_TamThan/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.TamThan_ID = new SelectList(db.TamThans, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_TamThan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,TamThan_ID")] Detail_TamThan detail_TamThan)
        {
            if (ModelState.IsValid)
            {
                db.Detail_TamThan.Add(detail_TamThan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TamThan.InformationExamination_ID);
            ViewBag.TamThan_ID = new SelectList(db.TamThans, "ID", "Name", detail_TamThan.TamThan_ID);
            return View(detail_TamThan);
        }

        // GET: Admin/Detail_TamThan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TamThan detail_TamThan = db.Detail_TamThan.Find(id);
            if (detail_TamThan == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TamThan.InformationExamination_ID);
            ViewBag.TamThan_ID = new SelectList(db.TamThans, "ID", "Name", detail_TamThan.TamThan_ID);
            return View(detail_TamThan);
        }

        // POST: Admin/Detail_TamThan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,TamThan_ID")] Detail_TamThan detail_TamThan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_TamThan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TamThan.InformationExamination_ID);
            ViewBag.TamThan_ID = new SelectList(db.TamThans, "ID", "Name", detail_TamThan.TamThan_ID);
            return View(detail_TamThan);
        }

        // GET: Admin/Detail_TamThan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TamThan detail_TamThan = db.Detail_TamThan.Find(id);
            if (detail_TamThan == null)
            {
                return HttpNotFound();
            }
            return View(detail_TamThan);
        }

        // POST: Admin/Detail_TamThan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_TamThan detail_TamThan = db.Detail_TamThan.Find(id);
            db.Detail_TamThan.Remove(detail_TamThan);
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
