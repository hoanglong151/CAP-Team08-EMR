using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class ClinicalsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            Clinical clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            //if (clinical == null)
            //{
            //    return HttpNotFound();
            //}
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Clinical = clinical;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult CreateOldPatient()
        {
            ViewData["Clinical.User_ID"] = new SelectList(db.Users, "ID", "Name");
            ViewData["Clinical.InformationExamination_ID"] = new SelectList(db.InformationExaminations, "ID", "ID");
            return PartialView("_CreateOldPatient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            if (ModelState.IsValid)
            {
                multiplesModel.Clinical.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                multiplesModel.Clinical.User_ID = multiplesModel.InformationExamination.User_ID;
                db.Clinicals.Add(multiplesModel.Clinical);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", multiplesModel.Clinical.InformationExamination_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", multiplesModel.Clinical.User_ID);
            return View(multiplesModel.Clinical);
        }

        // GET: Admin/Clinicals/Create
        public ActionResult Create()
        {
            ViewData["Clinical.User_ID"] = new SelectList(db.Users, "ID", "Name");
            ViewData["Clinical.InformationExamination_ID"] = new SelectList(db.InformationExaminations, "ID", "ID");
            return PartialView("_Create");
        }

        // POST: Admin/Clinicals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MultiplesModel multiplesModel)
        {
            if (ModelState.IsValid)
            {
                multiplesModel.Clinical.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                multiplesModel.Clinical.User_ID = multiplesModel.InformationExamination.User_ID;
                db.Clinicals.Add(multiplesModel.Clinical);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", multiplesModel.Clinical.InformationExamination_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", multiplesModel.Clinical.User_ID);
            return View(multiplesModel.Clinical);
        }

        // GET: Admin/Clinicals/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            Clinical clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == id);
            if (clinical == null)
            {
                return HttpNotFound();
            }
            ViewData["Clinical.InformationExamination_ID"] = new SelectList(db.InformationExaminations, "ID", "ID", clinical.InformationExamination_ID);
            ViewData["Clinical.User_ID"] = new SelectList(db.Users, "ID", "Name", clinical.User_ID);
            multiplesModel.Clinical = clinical;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Clinicals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(multiplesModel.Clinical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "MultipleModels");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", multiplesModel.Clinical.InformationExamination_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", multiplesModel.Clinical.User_ID);
            return View(multiplesModel.Clinical);
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
