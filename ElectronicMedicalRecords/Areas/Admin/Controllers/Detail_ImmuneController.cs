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
    public class Detail_ImmuneController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Immune
        public ActionResult Index()
        {
            var detail_Immune = db.Detail_Immune.Include(d => d.Immune).Include(d => d.InformationExamination);
            return View(detail_Immune.ToList());
        }

        // GET: Admin/Detail_Immune/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Immune detail_Immune = db.Detail_Immune.Find(id);
            if (detail_Immune == null)
            {
                return HttpNotFound();
            }
            return View(detail_Immune);
        }

        // GET: Admin/Detail_Immune/Create
        public ActionResult Create()
        {
            ViewBag.Immue_ID = new SelectList(db.Immunes, "ID", "NameTest");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_Immune/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<Immune> iMmunes, int informationID)
        {
            Detail_Immune detail_Immune = new Detail_Immune();
            foreach (var item in iMmunes)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_Immune.Immue_ID = item.ID;
                    detail_Immune.InformationExamination_ID = informationID;
                    detail_Immune.ChiDinh = item.ChiDinh;
                    detail_Immune.Result = item.Result;
                    db.Detail_Immune.Add(detail_Immune);
                    db.SaveChanges();
                }
            }
            ViewBag.Immune_ID = new SelectList(db.Immunes, "ID", "NameTest", detail_Immune.Immue_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Immune.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_Immune/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Immune> detail_Immunes = db.Detail_Immune.Where(p => p.InformationExamination_ID == id).ToList();
            List<Immune> iMmunes = new List<Immune>();
            for (int i = 0; i < detail_Immunes.Count; i++)
            {
                var Immune_ID = detail_Immunes[i].Immue_ID;
                var ImmuneCD = db.Immunes.FirstOrDefault(p => p.ID == Immune_ID);
                ImmuneCD.ChiDinh = detail_Immunes[i].ChiDinh;
                ImmuneCD.Result = detail_Immunes[i].Result;
                detail_Immunes[i].InformationExamination_ID = id;
                iMmunes.Add(ImmuneCD);
            }
            if (detail_Immunes == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Immune = iMmunes;
            multiplesModel.Detail_Immunes = detail_Immunes;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Immune/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Detail_Immune> detail_Immunes)
        {
            if (detail_Immunes != null)
            {
                foreach (var detail_Immune in detail_Immunes)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(detail_Immune).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        // GET: Admin/Detail_Immune/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Immune detail_Immune = db.Detail_Immune.Find(id);
            if (detail_Immune == null)
            {
                return HttpNotFound();
            }
            return View(detail_Immune);
        }

        // POST: Admin/Detail_Immune/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Immune detail_Immune = db.Detail_Immune.Find(id);
            db.Detail_Immune.Remove(detail_Immune);
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
