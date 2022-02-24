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
    public class Detail_TieuHoaController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_TieuHoa
        public ActionResult Index()
        {
            var detail_TieuHoa = db.Detail_TieuHoa.Include(d => d.InformationExamination).Include(d => d.TieuHoa);
            return View(detail_TieuHoa.ToList());
        }

        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            var listTieuHoa = db.Detail_TieuHoa.ToList();
            foreach (var item in multiplesModel.TieuHoa)
            {
                var checkExistDetail1 = listTieuHoa.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID && p.TieuHoa_ID == item.ID);
                if (checkExistDetail1 != null)
                {
                    db.Entry(checkExistDetail1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Detail_TieuHoa detail_TieuHoa = new Detail_TieuHoa();
                    detail_TieuHoa.TieuHoa_ID = item.ID;
                    detail_TieuHoa.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    db.Detail_TieuHoa.Add(detail_TieuHoa);
                    db.SaveChanges();
                }
                if (item.Dangerous == true)
                {
                    multiplesModel.InformationExamination.PatientStatus_ID = 44;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            foreach (var item1 in listTieuHoa)
            {
                var checkDelete = multiplesModel.TieuHoa.FirstOrDefault(p => p.ID == item1.TieuHoa_ID);
                if (checkDelete == null)
                {
                    db.Detail_TieuHoa.Remove(item1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CreateTest", "MultipleModels");
        }

        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            List<Detail_TieuHoa> detail_TieuHoas = db.Detail_TieuHoa.Where(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID).AsNoTracking().ToList();
            multiplesModel.Detail_TieuHoas = detail_TieuHoas;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_TieuHoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TieuHoa detail_TieuHoa = db.Detail_TieuHoa.Find(id);
            if (detail_TieuHoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_TieuHoa);
        }

        // GET: Admin/Detail_TieuHoa/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.TieuHoa_ID = new SelectList(db.TieuHoas, "ID", "Name");
            return View();
        }

        // POST: Admin/Detail_TieuHoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InformationExamination_ID,TieuHoa_ID")] Detail_TieuHoa detail_TieuHoa)
        {
            if (ModelState.IsValid)
            {
                db.Detail_TieuHoa.Add(detail_TieuHoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TieuHoa.InformationExamination_ID);
            ViewBag.TieuHoa_ID = new SelectList(db.TieuHoas, "ID", "Name", detail_TieuHoa.TieuHoa_ID);
            return View(detail_TieuHoa);
        }

        // GET: Admin/Detail_TieuHoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TieuHoa detail_TieuHoa = db.Detail_TieuHoa.Find(id);
            if (detail_TieuHoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TieuHoa.InformationExamination_ID);
            ViewBag.TieuHoa_ID = new SelectList(db.TieuHoas, "ID", "Name", detail_TieuHoa.TieuHoa_ID);
            return View(detail_TieuHoa);
        }

        // POST: Admin/Detail_TieuHoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InformationExamination_ID,TieuHoa_ID")] Detail_TieuHoa detail_TieuHoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_TieuHoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_TieuHoa.InformationExamination_ID);
            ViewBag.TieuHoa_ID = new SelectList(db.TieuHoas, "ID", "Name", detail_TieuHoa.TieuHoa_ID);
            return View(detail_TieuHoa);
        }

        // GET: Admin/Detail_TieuHoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_TieuHoa detail_TieuHoa = db.Detail_TieuHoa.Find(id);
            if (detail_TieuHoa == null)
            {
                return HttpNotFound();
            }
            return View(detail_TieuHoa);
        }

        // POST: Admin/Detail_TieuHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_TieuHoa detail_TieuHoa = db.Detail_TieuHoa.Find(id);
            db.Detail_TieuHoa.Remove(detail_TieuHoa);
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
