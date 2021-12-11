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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class Detail_SinhHoaMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_SinhHoaMau
        public ActionResult Index()
        {
            var detail_SinhHoaMau = db.Detail_SinhHoaMau.Include(d => d.InformationExamination).Include(d => d.SinhHoaMau);
            return View(detail_SinhHoaMau.ToList());
        }

        // GET: Admin/Detail_SinhHoaMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_SinhHoaMau detail_SinhHoaMau = db.Detail_SinhHoaMau.Find(id);
            if (detail_SinhHoaMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_SinhHoaMau);
        }

        // GET: Admin/Detail_SinhHoaMau/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.SinhHoaMau_ID = new SelectList(db.SinhHoaMaus, "ID", "NameTest");
            return View();
        }

        // POST: Admin/Detail_SinhHoaMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_SinhHoaMau detail_SinhHoaMau, List<SinhHoaMau> sinhHoaMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach(var item in sinhHoaMaus)
            {
                var checkexist = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_SinhHoaMau.SinhHoaMau_ID = item.ID;
                    detail_SinhHoaMau.InformationExamination_ID = informationID;
                    detail_SinhHoaMau.ChiDinh = item.ChiDinh;
                    db.Detail_SinhHoaMau.Add(detail_SinhHoaMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SinhHoaMau.InformationExamination_ID);
            ViewBag.SinhHoaMau_ID = new SelectList(db.SinhHoaMaus, "ID", "NameTest", detail_SinhHoaMau.SinhHoaMau_ID);
            //return View(detail_SinhHoaMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_SinhHoaMau> detail_SinhHoaMaus = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_SinhHoaMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_SinhHoaMaus = detail_SinhHoaMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_SinhHoaMau> detail_SinhHoaMaus = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_SinhHoaMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_SinhHoaMaus = detail_SinhHoaMaus;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_SinhHoaMau/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_SinhHoaMau> detail_SinhHoaMaus = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_SinhHoaMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_SinhHoaMaus = detail_SinhHoaMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_SinhHoaMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_SinhHoaMaus != null)
            {
                foreach (var detail_SinhHoaMau in multiplesModel.Detail_SinhHoaMaus)
                {
                    var DetailSHM = db.Detail_SinhHoaMau.FirstOrDefault(p => p.SinhHoaMau_ID == detail_SinhHoaMau.SinhHoaMau_ID && p.InformationExamination_ID == detail_SinhHoaMau.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailSHM.Result = detail_SinhHoaMau.Result;
                        db.Entry(DetailSHM).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SinhHoaMau.InformationExamination_ID);
            //ViewBag.SinhHoaMau_ID = new SelectList(db.SinhHoaMaus, "ID", "NameTest", detail_SinhHoaMau.SinhHoaMau_ID);
            //return View(detail_SinhHoaMau);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_SinhHoaMau detail_SinhHoaMau, List<SinhHoaMau> sinhHoaMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in sinhHoaMaus)
            {
                var checkexist = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_SinhHoaMau.SinhHoaMau_ID = item.ID;
                    detail_SinhHoaMau.InformationExamination_ID = informationID;
                    detail_SinhHoaMau.ChiDinh = item.ChiDinh;
                    db.Detail_SinhHoaMau.Add(detail_SinhHoaMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_SinhHoaMau.InformationExamination_ID);
            ViewBag.SinhHoaMau_ID = new SelectList(db.SinhHoaMaus, "ID", "NameTest", detail_SinhHoaMau.SinhHoaMau_ID);
            //return View(detail_SinhHoaMau);
            return RedirectToAction("Create", "MultipleModels");
        }
        // GET: Admin/Detail_SinhHoaMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_SinhHoaMau detail_SinhHoaMau = db.Detail_SinhHoaMau.Find(id);
            if (detail_SinhHoaMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_SinhHoaMau);
        }

        // POST: Admin/Detail_SinhHoaMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_SinhHoaMau detail_SinhHoaMau = db.Detail_SinhHoaMau.Find(id);
            db.Detail_SinhHoaMau.Remove(detail_SinhHoaMau);
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
