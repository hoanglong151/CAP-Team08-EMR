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
    public class ImmunesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Immunes
        public ActionResult Index()
        {
            return View(db.Immunes.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var immunes = db.Immunes.ToList();
            return Json(new { data = immunes }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Immunes/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.Immune = db.Immunes.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/Immunes/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Immune = db.Immunes.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // GET: Admin/Immunes/Edit/5
        public ActionResult Edit(int id)
        {
            Immune immune = db.Immunes.Find(id);
            if (immune == null)
            {
                return HttpNotFound();
            }
            var Immune = new { immune.Price, immune.ID, immune.NameTest, immune.Unit, immune.CSBT, immune.ChiDinh };
            return Json(new { data = Immune }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Immunes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(Immune immune)
        {
            if (ModelState.IsValid)
            {
                db.Entry(immune).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, responseText = "Không thể cập nhật giá" });
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
