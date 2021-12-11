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
    public class Detail_CTMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Detail_CTMau
        public ActionResult Index()
        {
            var detail_CTMau = db.Detail_CTMau.Include(d => d.CTMau).Include(d => d.InformationExamination);
            return View(detail_CTMau.ToList());
        }

        // GET: Admin/Detail_CTMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // GET: Admin/Detail_CTMau/Create
        public ActionResult Create()
        {
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_CTMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_CTMau detail_CTMau, List<CTMau> cTMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach(var item in cTMaus)
            {
                var checkexist = db.Detail_CTMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = informationID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    db.Detail_CTMau.Add(detail_CTMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_CTMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult BillCheck(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_CTMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/Detail_CTMau/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_CTMau> detail_CTMaus = db.Detail_CTMau.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_CTMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_CTMaus = detail_CTMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_CTMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if(multiplesModel.Detail_CTMaus != null)
            {
                foreach(var detail_CTMau in multiplesModel.Detail_CTMaus)
                {
                    var DetailCTMau = db.Detail_CTMau.FirstOrDefault(p => p.CTMau_ID == detail_CTMau.CTMau_ID && p.InformationExamination_ID == detail_CTMau.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailCTMau.Result = detail_CTMau.Result;
                        db.Entry(DetailCTMau).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_CTMau detail_CTMau, List<CTMau> cTMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in cTMaus)
            {
                var checkexist = db.Detail_CTMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_CTMau.CTMau_ID = item.ID;
                    detail_CTMau.InformationExamination_ID = informationID;
                    detail_CTMau.ChiDinh = item.ChiDinh;
                    db.Detail_CTMau.Add(detail_CTMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.CTMau_ID = new SelectList(db.CTMaus, "ID", "NameTest", detail_CTMau.CTMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_CTMau.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
        }

        // GET: Admin/Detail_CTMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            if (detail_CTMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_CTMau);
        }

        // POST: Admin/Detail_CTMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_CTMau detail_CTMau = db.Detail_CTMau.Find(id);
            db.Detail_CTMau.Remove(detail_CTMau);
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
