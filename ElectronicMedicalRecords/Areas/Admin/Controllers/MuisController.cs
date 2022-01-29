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
    public class MuisController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Muis
        public ActionResult Index()
        {
            return View(db.Muis.ToList());
        }

        public ActionResult EditSelect(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var listDetailMui = db.Detail_Mui.Where(p => p.InformationExamination_ID == id).ToList();
            var listMui = db.Muis.ToList();
            foreach (var item in listDetailMui)
            {
                var changeSelect = listMui.FirstOrDefault(p => p.ID == item.Mui_ID);
                changeSelect.ChiDinh = true;
            }
            multiplesModel.Mui = listMui;
            return PartialView("_EditSelect", multiplesModel);
        }

        public JsonResult GetArrMui(int id)
        {
            var listDetailMui = db.Detail_Mui.Where(p => p.InformationExamination_ID == id).ToList();
            var listOfStrings = new List<string>();
            foreach (var item in listDetailMui)
            {
                listOfStrings.Add("" + item.Mui_ID);
            }
            return Json(new { success = true, res = listOfStrings.ToArray() });
        }

        public ActionResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var muis = db.Muis.ToList();
            return Json(new { data = muis }, JsonRequestBehavior.AllowGet);
        }

        public string ValidateForm(Mui mui)
        {
            string text = "";
            var checkExist = db.Muis.FirstOrDefault(e => e.Name == mui.Name);
            if (checkExist != null && mui.Name != null)
            {
                text = "Mũi đã có trong danh sách";
            }
            return text;
        }

        public string ValidateFormUpdate(Mui mui)
        {
            string text = "";
            var checkExist = db.Muis.FirstOrDefault(e => e.Name == mui.Name);
            if (checkExist != null && checkExist.ID != mui.ID && mui.Name != null)
            {
                text = "Mũi đã có trong danh sách";
            }
            return text;
        }

        // GET: Admin/Muis/CreateOldPatient
        public ActionResult CreateOldPatient()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Mui = db.Muis.ToList();
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        // POST: Admin/Muis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mui mui)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateForm(mui);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    mui.ChiDinh = false;
                    db.Muis.Add(mui);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(mui);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Muis/Edit/5
        public ActionResult Edit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mui mui = db.Muis.Find(id);
            if (mui == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = mui }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Muis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mui mui)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var text = ValidateFormUpdate(mui);
            if (text == "")
            {
                if (ModelState.IsValid)
                {
                    var existData = db.Muis.Find(mui.ID);
                    db.Entry(existData).CurrentValues.SetValues(mui);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return View(mui);
            }
            return Json(new { success = false, responseText = text });
        }

        // GET: Admin/Muis/Delete/5
        public ActionResult Delete(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mui mui = db.Muis.Find(id);
            if (mui == null)
            {
                return HttpNotFound();
            }
            return Json(new { data = mui }, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Muis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Mui mui = db.Muis.Find(id);
            try
            {
                db.Muis.Remove(mui);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Mũi này đã được sử dụng. Bạn không thể xóa nó!" });
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
