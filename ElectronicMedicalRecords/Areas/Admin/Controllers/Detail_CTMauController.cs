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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class Detail_CTMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Detail_CTMau
        public ActionResult Index()
        {
            var detail_CTMau = db.Detail_CTMau.Include(d => d.CTMau).Include(d => d.InformationExamination);
            return View(detail_CTMau.ToList());
        }

        // GET: Admin/Detail_CTMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // GET: Admin/Detail_CTMau/Create
        public ActionResult Create()
        {
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_CTMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<CTMau> cTMaus, int informationID, MultiplesModel multiplesModel)
        {
            Detail_CTMau detail_CTMau = new Detail_CTMau();
            foreach(var item in cTMaus)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = informationID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    detail_CTMau.Result = item.Result;
                    multiplesModel.InformationExamination.TestCD = false;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.Detail_CTMau.Add(detail_CTMau);
                    db.SaveChanges();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<CTMau> cTMaus = new List<CTMau>();
            for (int i = 0; i < detail_CTMaus.Count; i++)
            {
                var CTMau_ID = detail_CTMaus[i].CTMau_ID;
                var MauCD = db.CTMaus.FirstOrDefault(p => p.ID == CTMau_ID);
                MauCD.ChiDinh = detail_CTMaus[i].ChiDinh;
                MauCD.Result = detail_CTMaus[i].Result;
                detail_CTMaus[i].InformationExamination_ID = id;
                cTMaus.Add(MauCD);
            }
            if (detail_CTMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.CTMau = cTMaus;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_CTMau/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<CTMau> cTMaus = new List<CTMau>();
            for(int i = 0; i < detail_CTMaus.Count; i++)
            {
                var CTMau_ID = detail_CTMaus[i].CTMau_ID;
                var MauCD = db.CTMaus.FirstOrDefault(p => p.ID == CTMau_ID);
                MauCD.ChiDinh = detail_CTMaus[i].ChiDinh;
                MauCD.Result = detail_CTMaus[i].Result;
                detail_CTMaus[i].InformationExamination_ID = id;
                cTMaus.Add(MauCD);
            }
            if (detail_CTMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.CTMau = cTMaus;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_CTMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Detail_CTMau> detail_CTMaus)
        {
            if(detail_CTMaus != null)
            {
                foreach(var detail_CTMau in detail_CTMaus)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(detail_CTMau).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            Detail_CTMau detail_CTMau = new Detail_CTMau();
            foreach (var item in multiplesModel.CTMau)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    detail_CTMau.Result = item.Result;
                    multiplesModel.InformationExamination.TestCD = false;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.Detail_CTMau.Add(detail_CTMau);
                    db.SaveChanges();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Detail_CTMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // POST: Admin/Detail_CTMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            db.Detail_CTMau.Remove(detail_CTMau);
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
