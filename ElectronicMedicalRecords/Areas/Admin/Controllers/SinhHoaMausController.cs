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
    public class SinhHoaMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/SinhHoaMaus
        public ActionResult Index()
        {
            return View(db.SinhHoaMaus.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var sinhHoaMaus = db.SinhHoaMaus.ToList();
            return Json(new { data = sinhHoaMaus }, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/SinhHoaMaus/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.SinhHoaMau = db.SinhHoaMaus.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/SinhHoaMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.SinhHoaMau = db.SinhHoaMaus.ToList();
            return PartialView("_Create", multiplesModel);
        }

        // GET: Admin/SinhHoaMaus/Edit/5
        public ActionResult Edit(int id)
        {
            SinhHoaMau sinhHoaMau = db.SinhHoaMaus.Find(id);
            if (sinhHoaMau == null)
            {
                return HttpNotFound();
            }
            var SinhHoaMau = new { sinhHoaMau.ID, sinhHoaMau.NameTest, sinhHoaMau.Price, sinhHoaMau.CSBT, sinhHoaMau.ChiDinh };
            return Json(new { data = SinhHoaMau }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/SinhHoaMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(SinhHoaMau sinhHoaMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhHoaMau).State = EntityState.Modified;
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
