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
    public class Detail_SanPhuKhoaController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_SanPhuKhoa
        public ActionResult Index()
        {
            var detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Include(d => d.InformationExamination).Include(d => d.SanPhuKhoa);
            return View(detail_SanPhuKhoa.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listSanPhuKhoa = db.Detail_SanPhuKhoa.ToList();
            foreach (var item in multiplesModel.SanPhuKhoa)
            {
                var checkExistDetail1 = listSanPhuKhoa.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.SanPhuKhoa_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_SanPhuKhoa detail_SanPhuKhoa = new Detail_SanPhuKhoa();
                    detail_SanPhuKhoa.SanPhuKhoa_ID = item.ID;
                    detail_SanPhuKhoa.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_SanPhuKhoa.Add(detail_SanPhuKhoa);
                    db.SaveChanges();
                }
                if (checkExistDetail1.SanPhuKhoa.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listSanPhuKhoa)
            {
                var checkDelete = multiplesModel.SanPhuKhoa.FirstOrDefault(p => p.ID == item1.SanPhuKhoa_ID);
                if (checkDelete == null)
                {
                    db.Detail_SanPhuKhoa.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_SanPhuKhoa> detail_SanPhuKhoas = db.Detail_SanPhuKhoa.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_SanPhuKhoas = detail_SanPhuKhoas;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_SanPhuKhoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_SanPhuKhoa detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Find(id);
            if (detail_SanPhuKhoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_SanPhuKhoa);
        }

        // GET: Admin/Detail_SanPhuKhoa/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.SanPhuKhoa_ID = new SelectList(db.SanPhuKhoas, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_SanPhuKhoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,SanPhuKhoa_ID")] Detail_SanPhuKhoa detail_SanPhuKhoa)
        {
            if (ModelState.IsValid)
            {
                db.Detail_SanPhuKhoa.Add(detail_SanPhuKhoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SanPhuKhoa.InformationExamination_ID);
            ViewBag.SanPhuKhoa_ID = new SelectList(db.SanPhuKhoas, "ID", "Name", detail_SanPhuKhoa.SanPhuKhoa_ID);
            return View(detail_SanPhuKhoa);
        }

        // GET: Admin/Detail_SanPhuKhoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_SanPhuKhoa detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Find(id);
            if (detail_SanPhuKhoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SanPhuKhoa.InformationExamination_ID);
            ViewBag.SanPhuKhoa_ID = new SelectList(db.SanPhuKhoas, "ID", "Name", detail_SanPhuKhoa.SanPhuKhoa_ID);
            return View(detail_SanPhuKhoa);
        }

        // POST: Admin/Detail_SanPhuKhoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,SanPhuKhoa_ID")] Detail_SanPhuKhoa detail_SanPhuKhoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_SanPhuKhoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SanPhuKhoa.InformationExamination_ID);
            ViewBag.SanPhuKhoa_ID = new SelectList(db.SanPhuKhoas, "ID", "Name", detail_SanPhuKhoa.SanPhuKhoa_ID);
            return View(detail_SanPhuKhoa);
        }

        // GET: Admin/Detail_SanPhuKhoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_SanPhuKhoa detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Find(id);
            if (detail_SanPhuKhoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_SanPhuKhoa);
        }

        // POST: Admin/Detail_SanPhuKhoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_SanPhuKhoa detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Find(id);
            db.Detail_SanPhuKhoa.Remove(detail_SanPhuKhoa);
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
