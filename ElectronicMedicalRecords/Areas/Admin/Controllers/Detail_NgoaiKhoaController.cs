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
    public class Detail_NgoaiKhoaController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_NgoaiKhoa
        public ActionResult Index()
        {
            var detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Include(d => d.InformationExamination).Include(d => d.NgoaiKhoa);
            return View(detail_NgoaiKhoa.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listNgoaiKhoa = db.Detail_NgoaiKhoa.ToList();
            foreach (var item in multiplesModel.NgoaiKhoa)
            {
                var checkExistDetail1 = listNgoaiKhoa.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.NgoaiKhoa_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_NgoaiKhoa detail_NgoaiKhoa = new Detail_NgoaiKhoa();
                    detail_NgoaiKhoa.NgoaiKhoa_ID = item.ID;
                    detail_NgoaiKhoa.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_NgoaiKhoa.Add(detail_NgoaiKhoa);
                    db.SaveChanges();
                }
                if (checkExistDetail1.NgoaiKhoa.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listNgoaiKhoa)
            {
                var checkDelete = multiplesModel.NgoaiKhoa.FirstOrDefault(p => p.ID == item1.NgoaiKhoa_ID);
                if (checkDelete == null)
                {
                    db.Detail_NgoaiKhoa.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_NgoaiKhoa> detail_NgoaiKhoas = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_NgoaiKhoas = detail_NgoaiKhoas;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_NgoaiKhoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_NgoaiKhoa detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Find(id);
            if (detail_NgoaiKhoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_NgoaiKhoa);
        }

        // GET: Admin/Detail_NgoaiKhoa/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.NgoaiKhoa_ID = new SelectList(db.NgoaiKhoas, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_NgoaiKhoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,NgoaiKhoa_ID")] Detail_NgoaiKhoa detail_NgoaiKhoa)
        {
            if (ModelState.IsValid)
            {
                db.Detail_NgoaiKhoa.Add(detail_NgoaiKhoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_NgoaiKhoa.InformationExamination_ID);
            ViewBag.NgoaiKhoa_ID = new SelectList(db.NgoaiKhoas, "ID", "Name", detail_NgoaiKhoa.NgoaiKhoa_ID);
            return View(detail_NgoaiKhoa);
        }

        // GET: Admin/Detail_NgoaiKhoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_NgoaiKhoa detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Find(id);
            if (detail_NgoaiKhoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_NgoaiKhoa.InformationExamination_ID);
            ViewBag.NgoaiKhoa_ID = new SelectList(db.NgoaiKhoas, "ID", "Name", detail_NgoaiKhoa.NgoaiKhoa_ID);
            return View(detail_NgoaiKhoa);
        }

        // POST: Admin/Detail_NgoaiKhoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,NgoaiKhoa_ID")] Detail_NgoaiKhoa detail_NgoaiKhoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_NgoaiKhoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_NgoaiKhoa.InformationExamination_ID);
            ViewBag.NgoaiKhoa_ID = new SelectList(db.NgoaiKhoas, "ID", "Name", detail_NgoaiKhoa.NgoaiKhoa_ID);
            return View(detail_NgoaiKhoa);
        }

        // GET: Admin/Detail_NgoaiKhoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_NgoaiKhoa detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Find(id);
            if (detail_NgoaiKhoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_NgoaiKhoa);
        }

        // POST: Admin/Detail_NgoaiKhoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_NgoaiKhoa detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Find(id);
            db.Detail_NgoaiKhoa.Remove(detail_NgoaiKhoa);
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
