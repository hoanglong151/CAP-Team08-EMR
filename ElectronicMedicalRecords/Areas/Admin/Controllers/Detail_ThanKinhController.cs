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
    public class Detail_ThanKinhController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_ThanKinh
        public ActionResult Index()
        {
            var detail_ThanKinh = db.Detail_ThanKinh.Include(d => d.InformationExamination).Include(d => d.ThanKinh);
            return View(detail_ThanKinh.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listThanKinh = db.Detail_ThanKinh.ToList();
            foreach (var item in multiplesModel.ThanKinh)
            {
                var checkExistDetail1 = listThanKinh.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.ThanKinh_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_ThanKinh detail_ThanKinh = new Detail_ThanKinh();
                    detail_ThanKinh.ThanKinh_ID = item.ID;
                    detail_ThanKinh.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_ThanKinh.Add(detail_ThanKinh);
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listThanKinh)
            {
                var checkDelete = multiplesModel.ThanKinh.FirstOrDefault(p => p.ID == item1.ThanKinh_ID);
                if (checkDelete == null)
                {
                    db.Detail_ThanKinh.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_ThanKinh> detail_ThanKinhs = db.Detail_ThanKinh.Where(p => p.InformationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_ThanKinhs = detail_ThanKinhs;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_ThanKinh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanKinh detail_ThanKinh = db.Detail_ThanKinh.Find(id);
            if (detail_ThanKinh == null)
            {
                return HttpNotFound();
            }
            return View(detail_ThanKinh);
        }

        // GET: Admin/Detail_ThanKinh/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.ThanKinh_ID = new SelectList(db.ThanKinhs, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_ThanKinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,ThanKinh_ID")] Detail_ThanKinh detail_ThanKinh)
        {
            if (ModelState.IsValid)
            {
                db.Detail_ThanKinh.Add(detail_ThanKinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanKinh.InformationExamination_ID);
            ViewBag.ThanKinh_ID = new SelectList(db.ThanKinhs, "ID", "Name", detail_ThanKinh.ThanKinh_ID);
            return View(detail_ThanKinh);
        }

        // GET: Admin/Detail_ThanKinh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanKinh detail_ThanKinh = db.Detail_ThanKinh.Find(id);
            if (detail_ThanKinh == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanKinh.InformationExamination_ID);
            ViewBag.ThanKinh_ID = new SelectList(db.ThanKinhs, "ID", "Name", detail_ThanKinh.ThanKinh_ID);
            return View(detail_ThanKinh);
        }

        // POST: Admin/Detail_ThanKinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,ThanKinh_ID")] Detail_ThanKinh detail_ThanKinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_ThanKinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ThanKinh.InformationExamination_ID);
            ViewBag.ThanKinh_ID = new SelectList(db.ThanKinhs, "ID", "Name", detail_ThanKinh.ThanKinh_ID);
            return View(detail_ThanKinh);
        }

        // GET: Admin/Detail_ThanKinh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ThanKinh detail_ThanKinh = db.Detail_ThanKinh.Find(id);
            if (detail_ThanKinh == null)
            {
                return HttpNotFound();
            }
            return View(detail_ThanKinh);
        }

        // POST: Admin/Detail_ThanKinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_ThanKinh detail_ThanKinh = db.Detail_ThanKinh.Find(id);
            db.Detail_ThanKinh.Remove(detail_ThanKinh);
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
