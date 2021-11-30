﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class InformationExaminationsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        MultiplesModel multiplesModel = new MultiplesModel();
        // GET: Admin/InformationExaminations
        public ActionResult Index()
        {
            var informationExaminations = db.InformationExaminations.Include(i => i.Patient).Include(i => i.PatientStatu).Include(i => i.User);
            return View(informationExaminations.ToList());
        }

        public JsonResult GetNotification()
        {
            db.Configuration.ProxyCreationEnabled = true;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetInformationExamination();
            List<Patient> patient = new List<Patient>();
            foreach(var item1 in list)
            {
                var patientUser = db.Patients.FirstOrDefault(p => p.ID == item1.Patient_ID);
                patient.Add(patientUser);
            }
            var listpatient = patient.Select(s => new
            {
                ID = s.ID,
                Name = s.Name,
            }).ToList();
            return Json(new { data = list, userName = listpatient }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotificationBS()
        {
            db.Configuration.ProxyCreationEnabled = true;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.ReturnResultTest();
            List<object> Noti = new List<object>();
            var count = 0;
            foreach (var item1 in list)
            {
                var patientUser = db.Patients.FirstOrDefault(p => p.ID == item1.Patient_ID);
                if(item1.ResultCTMau == true)
                {
                    count += 1;
                }
                if(item1.ResultSHM == true)
                {
                    count += 1;
                }
                if(item1.ResultDMau == true)
                {
                    count += 1;
                }
                if(item1.ResultNhomMau == true)
                {
                    count += 1;
                }
                if(item1.ResultNuocTieu == true)
                {
                    count += 1;
                }
                if(item1.ResultMienDich == true)
                {
                    count += 1;
                }
                if(item1.ResultDichChocDo == true)
                {
                    count += 1;
                }
                if(item1.ResultViSinh == true)
                {
                    count += 1;
                }
                var NotiResult = new { patientUser.Name, count, item1.ID };
                Noti.Add(NotiResult);
                count = 0;
            }
            return Json(new { data = Noti }, JsonRequestBehavior.AllowGet);
        }


        // GET: Admin/InformationExaminations/Details/5
        public ActionResult Details(int id)
        {
            var informationExaminations = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            if (informationExaminations == null)
            {
                return HttpNotFound();
            }
            return View(informationExaminations);
        }

        // GET: Admin/InformationExaminations/Edit/5
        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = db.InformationExaminations.Find(id);
            if (informationExamination == null)
            {
                return HttpNotFound();
            }

            var UserName = db.Users.FirstOrDefault(p => p.ID == informationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.UserName = UserName.Name;
            multiplesModel.InformationExamination = informationExamination;
            return PartialView("_DetailsIE", multiplesModel);
        }


        // GET: Admin/InformationExaminations/Create
        public ActionResult Create()
        {
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(id => id.UserID == UserID);
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewBag.DateExamination = DateTime.Now;
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name");
            return PartialView("_Create");
        }

        // POST: Admin/InformationExaminations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InformationExamination informationExamination, int patientID)
        {
            if (ModelState.IsValid)
            {
                informationExamination.Patient_ID = patientID;
                informationExamination.DateEnd = DateTime.Now;
                db.InformationExaminations.Add(informationExamination);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }

            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // GET: Admin/InformationExaminations/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = db.InformationExaminations.Find(id);
            if (informationExamination == null)
            {
                return HttpNotFound();
            }

            var UserName = db.Users.FirstOrDefault(p => p.ID == informationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.UserName = UserName.Name;
            multiplesModel.InformationExamination = informationExamination;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(InformationExamination informationExamination)
        {
            if (ModelState.IsValid)
            {
                db.InformationExaminations.Add(informationExamination);
                db.SaveChanges();
                return RedirectToAction("CreateOldPatient", "MultipleModels");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // GET: Admin/InformationExaminations/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = db.InformationExaminations.Find(id);
            if (informationExamination == null)
            {
                return HttpNotFound();
            }
            
            var UserName = db.Users.FirstOrDefault(p => p.ID == informationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.UserName = UserName.Name;
            multiplesModel.InformationExamination = informationExamination;
            return PartialView("_Edit",multiplesModel);
        }

        // POST: Admin/InformationExaminations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InformationExamination informationExamination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(informationExamination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "MultipleModels");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // POST: Admin/InformationExaminations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InformationExamination checkInfoExam = db.InformationExaminations.Find(id);
            if (checkInfoExam.HeartBeat != null || checkInfoExam.Breathing != null
                    || checkInfoExam.BloodPressure != null || checkInfoExam.Weight != null
                    || checkInfoExam.Height != null || checkInfoExam.ResultCTMau != null
                    || checkInfoExam.ResultSHM != null || checkInfoExam.ResultDMau != null
                    || checkInfoExam.ResultNhomMau != null || checkInfoExam.ResultNuocTieu != null
                    || checkInfoExam.ResultMienDich != null || checkInfoExam.ResultDichChocDo != null
                    || checkInfoExam.ResultViSinh != null)
            {
                return Json(new { success = false });
            }
            else
            {
                db.InformationExaminations.Remove(checkInfoExam);
                db.SaveChanges();
                return Json(new { success = true });
            }
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
