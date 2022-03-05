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

        // GET: Admin/NhomMaus/CreateOldPatient
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
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
