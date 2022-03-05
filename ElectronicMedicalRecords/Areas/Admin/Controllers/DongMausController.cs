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
    public class DongMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/DongMaus
        public ActionResult Index()
        {
            return View(db.DongMaus.ToList());
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var dongMaus = db.DongMaus.ToList();
            return Json(new { data = dongMaus }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/DongMaus/Create
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            multiplesModel.DongMau = db.DongMaus.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // GET: Admin/DongMaus/Create
        public ActionResult Create()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.DongMau = db.DongMaus.ToList();
            return PartialView("_Create", multiplesModel); 
        }

        // GET: Admin/DongMaus/Edit/5
        public ActionResult Edit(int id)
        {
            DongMau dongMau = db.DongMaus.Find(id);
            if (dongMau == null)
            {
                return HttpNotFound();
            }
            var DongMau = new { dongMau.ID, dongMau.NameTest, dongMau.Price, dongMau.Unit, dongMau.ChiDinh, dongMau.CSBT };
            return Json(new { data = DongMau }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/DongMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(DongMau dongMau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dongMau).State = EntityState.Modified;
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
