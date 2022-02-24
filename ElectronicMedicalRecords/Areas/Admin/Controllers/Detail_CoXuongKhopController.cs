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
    public class Detail_CoXuongKhopController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_CoXuongKhop
        public ActionResult Index()
        {
            var detail_CoXuongKhop = db.Detail_CoXuongKhop.Include(d => d.CoXuongKhop).Include(d => d.InformationExamination);
            return View(detail_CoXuongKhop.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listCoXuongKhop = db.Detail_CoXuongKhop.ToList();
            foreach (var item in multiplesModel.CoXuongKhop)
            {
                var checkExistDetail1 = listCoXuongKhop.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.CoXuongKhop_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_CoXuongKhop detail_CoXuongKhop = new Detail_CoXuongKhop();
                    detail_CoXuongKhop.CoXuongKhop_ID = item.ID;
                    detail_CoXuongKhop.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_CoXuongKhop.Add(detail_CoXuongKhop);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listCoXuongKhop)
            {
                var checkDelete = multiplesModel.CoXuongKhop.FirstOrDefault(p => p.ID == item1.CoXuongKhop_ID);
                if (checkDelete == null)
                {
                    db.Detail_CoXuongKhop.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_CoXuongKhop> detail_CoXuongKhops = db.Detail_CoXuongKhop.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_CoXuongKhops = detail_CoXuongKhops;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_CoXuongKhop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CoXuongKhop detail_CoXuongKhop = db.Detail_CoXuongKhop.Find(id);
            if (detail_CoXuongKhop == null)
            {
                return HttpNotFound();
            }
            return View(detail_CoXuongKhop);
        }

        // GET: Admin/Detail_CoXuongKhop/Create
        public ActionResult Create()
        {
            ViewBag.CoXuongKhop_ID = new SelectList(db.CoXuongKhops, "ID", "Name");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_CoXuongKhop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,CoXuongKhop_ID")] Detail_CoXuongKhop detail_CoXuongKhop)
        {
            if (ModelState.IsValid)
            {
                db.Detail_CoXuongKhop.Add(detail_CoXuongKhop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CoXuongKhop_ID = new SelectList(db.CoXuongKhops, "ID", "Name", detail_CoXuongKhop.CoXuongKhop_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CoXuongKhop.InformationExamination_ID);
            return View(detail_CoXuongKhop);
        }

        // GET: Admin/Detail_CoXuongKhop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CoXuongKhop detail_CoXuongKhop = db.Detail_CoXuongKhop.Find(id);
            if (detail_CoXuongKhop == null)
            {
                return HttpNotFound();
            }
            ViewBag.CoXuongKhop_ID = new SelectList(db.CoXuongKhops, "ID", "Name", detail_CoXuongKhop.CoXuongKhop_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CoXuongKhop.InformationExamination_ID);
            return View(detail_CoXuongKhop);
        }

        // POST: Admin/Detail_CoXuongKhop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,CoXuongKhop_ID")] Detail_CoXuongKhop detail_CoXuongKhop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_CoXuongKhop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CoXuongKhop_ID = new SelectList(db.CoXuongKhops, "ID", "Name", detail_CoXuongKhop.CoXuongKhop_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CoXuongKhop.InformationExamination_ID);
            return View(detail_CoXuongKhop);
        }

        // GET: Admin/Detail_CoXuongKhop/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CoXuongKhop detail_CoXuongKhop = db.Detail_CoXuongKhop.Find(id);
            if (detail_CoXuongKhop == null)
            {
                return HttpNotFound();
            }
            return View(detail_CoXuongKhop);
        }

        // POST: Admin/Detail_CoXuongKhop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_CoXuongKhop detail_CoXuongKhop = db.Detail_CoXuongKhop.Find(id);
            db.Detail_CoXuongKhop.Remove(detail_CoXuongKhop);
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
