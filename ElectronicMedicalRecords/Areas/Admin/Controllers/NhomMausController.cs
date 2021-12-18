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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class NhomMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/NhomMaus
        public ActionResult Index()
        {
            return View(db.NhomMaus.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nhomMaus = db.NhomMaus.ToList();
            return Json(new { data = nhomMaus }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/NhomMaus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomMau nhomMau = db.NhomMaus.Find(id);
            if (nhomMau == null)
            {
                return HttpNotFound();
            }
            return View(nhomMau);
        }

        // GET: Admin/NhomMaus/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.NhomMau = db.NhomMaus.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/NhomMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.NhomMau = db.NhomMaus.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // POST: Admin/NhomMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NameTest,Result")] NhomMau nhomMau)
        {
            if (ModelState.IsValid)
            {
                db.NhomMaus.Add(nhomMau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhomMau);
        }

        // GET: Admin/NhomMaus/Edit/5
        public ActionResult Edit(int id)
        {
            NhomMau nhomMau = db.NhomMaus.Find(id);
            if (nhomMau == null)
            {
                return HttpNotFound();
            }
            var NhomMau = new { nhomMau.ChiDinh, nhomMau.ID, nhomMau.NameTest, nhomMau.Price };
            return Json(new { data = NhomMau }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/NhomMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(NhomMau nhomMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhomMau).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, responseText = "Không thể cập nhật giá" });
        }

        // GET: Admin/NhomMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomMau nhomMau = db.NhomMaus.Find(id);
            if (nhomMau == null)
            {
                return HttpNotFound();
            }
            return View(nhomMau);
        }

        // POST: Admin/NhomMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhomMau nhomMau = db.NhomMaus.Find(id);
            db.NhomMaus.Remove(nhomMau);
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
