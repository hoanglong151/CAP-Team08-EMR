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
    public class UrinesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Urines
        public ActionResult Index()
        {
            return View(db.Urines.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var urines = db.Urines.ToList();
            return Json(new { data = urines }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Urines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urine urine = db.Urines.Find(id);
            if (urine == null)
            {
                return HttpNotFound();
            }
            return View(urine);
        }

        // GET: Admin/Urines/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Urine = db.Urines.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/Urines/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Urine = db.Urines.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/Urines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChiDinh,Name,Result,CSBT,Unit")] Urine urine)
        {
            if (ModelState.IsValid)
            {
                db.Urines.Add(urine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(urine);
        }

        // GET: Admin/Urines/Edit/5
        public ActionResult Edit(int id)
        {
            Urine urine = db.Urines.Find(id);
            if (urine == null)
            {
                return HttpNotFound();
            }
            var Urine = new { urine.ID, urine.Name, urine.Price, urine.Unit, urine.CSBT, urine.ChiDinh };
            return Json(new { data = Urine }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Urines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(Urine urine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urine).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, responseText = "Không thể cập nhật giá" });
        }

        // GET: Admin/Urines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urine urine = db.Urines.Find(id);
            if (urine == null)
            {
                return HttpNotFound();
            }
            return View(urine);
        }

        // POST: Admin/Urines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urine urine = db.Urines.Find(id);
            db.Urines.Remove(urine);
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
