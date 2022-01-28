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
    public class Detail_MuiController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Mui
        public ActionResult Index()
        {
            var detail_Mui = db.Detail_Mui.Include(d => d.InformationExamination).Include(d => d.Mui);
            return View(detail_Mui.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listMui = db.Detail_Mui.ToList();
            foreach (var item in multiplesModel.Mui)
            {
                var checkExistDetail1 = listMui.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.Mui_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_Mui detail_Mui = new Detail_Mui();
                    detail_Mui.Mui_ID = item.ID;
                    detail_Mui.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_Mui.Add(detail_Mui);
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listMui)
            {
                var checkDelete = multiplesModel.Mui.FirstOrDefault(p => p.ID == item1.Mui_ID);
                if (checkDelete == null)
                {
                    db.Detail_Mui.Remove(item1);
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
            List<Detail_Mui> detail_Muis = db.Detail_Mui.Where(p => p.InformationExamination_ID == id).ToList();
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Muis = detail_Muis;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_Mui/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mui detail_Mui = db.Detail_Mui.Find(id);
            if (detail_Mui == null)
            {
                return HttpNotFound();
            }
            return View(detail_Mui);
        }

        // GET: Admin/Detail_Mui/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.Mui_ID = new SelectList(db.Muis, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_Mui/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,Mui_ID")] Detail_Mui detail_Mui)
        {
            if (ModelState.IsValid)
            {
                db.Detail_Mui.Add(detail_Mui);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mui.InformationExamination_ID);
            ViewBag.Mui_ID = new SelectList(db.Muis, "ID", "Name", detail_Mui.Mui_ID);
            return View(detail_Mui);
        }

        // GET: Admin/Detail_Mui/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mui detail_Mui = db.Detail_Mui.Find(id);
            if (detail_Mui == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mui.InformationExamination_ID);
            ViewBag.Mui_ID = new SelectList(db.Muis, "ID", "Name", detail_Mui.Mui_ID);
            return View(detail_Mui);
        }

        // POST: Admin/Detail_Mui/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,Mui_ID")] Detail_Mui detail_Mui)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_Mui).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Mui.InformationExamination_ID);
            ViewBag.Mui_ID = new SelectList(db.Muis, "ID", "Name", detail_Mui.Mui_ID);
            return View(detail_Mui);
        }

        // GET: Admin/Detail_Mui/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Mui detail_Mui = db.Detail_Mui.Find(id);
            if (detail_Mui == null)
            {
                return HttpNotFound();
            }
            return View(detail_Mui);
        }

        // POST: Admin/Detail_Mui/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Mui detail_Mui = db.Detail_Mui.Find(id);
            db.Detail_Mui.Remove(detail_Mui);
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
