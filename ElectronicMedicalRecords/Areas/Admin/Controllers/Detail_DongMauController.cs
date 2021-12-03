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
    public class Detail_DongMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_DongMau
        public ActionResult Index()
        {
            var detail_DongMau = db.Detail_DongMau.Include(d => d.DongMau).Include(d => d.InformationExamination);
            return View(detail_DongMau.ToList());
        }

        // GET: Admin/Detail_DongMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_DongMau detail_DongMau = db.Detail_DongMau.Find(id);
            if (detail_DongMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_DongMau);
        }

        // GET: Admin/Detail_DongMau/Create
        public ActionResult Create()
        {
            ViewBag.DongMau_ID = new SelectList(db.DongMaus, "ID", "NameTest");
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_DongMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_DongMau detail_DongMau, List<DongMau> dongMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach(var item in dongMaus)
            {
                var checkexist = db.Detail_DongMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_DongMau.DongMau_ID = item.ID;
                    detail_DongMau.InformationExamination_ID = informationID;
                    detail_DongMau.Result = item.Result;
                    detail_DongMau.ChiDinh = item.ChiDinh;
                    db.Detail_DongMau.Add(detail_DongMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.DongMau_ID = new SelectList(db.DongMaus, "ID", "NameTest", detail_DongMau.DongMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_DongMau.InformationExamination_ID);
            //return View(detail_DongMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_DongMau> detail_DongMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<DongMau> dongMaus = new List<DongMau>();
            for (int i = 0; i < detail_DongMaus.Count; i++)
            {
                var DongMau_ID = detail_DongMaus[i].DongMau_ID;
                var DongMauCD = db.DongMaus.FirstOrDefault(p => p.ID == DongMau_ID);
                DongMauCD.ChiDinh = detail_DongMaus[i].ChiDinh;
                DongMauCD.Result = detail_DongMaus[i].Result;
                detail_DongMaus[i].InformationExamination_ID = id;
                dongMaus.Add(DongMauCD);
            }
            if (detail_DongMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.DongMau = dongMaus;
            multiplesModel.Detail_DongMaus = detail_DongMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_DongMau/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_DongMau> detail_DongMaus = db.Detail_DongMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<DongMau> dongMaus = new List<DongMau>();
            for (int i = 0; i < detail_DongMaus.Count; i++)
            {
                var DongMau_ID = detail_DongMaus[i].DongMau_ID;
                var DongMauCD = db.DongMaus.FirstOrDefault(p => p.ID == DongMau_ID);
                DongMauCD.ChiDinh = detail_DongMaus[i].ChiDinh;
                DongMauCD.Result = detail_DongMaus[i].Result;
                detail_DongMaus[i].InformationExamination_ID = id;
                dongMaus.Add(DongMauCD);
            }
            if (detail_DongMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.DongMau = dongMaus;
            multiplesModel.Detail_DongMaus = detail_DongMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_DongMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_DongMaus != null)
            {
                foreach (var detail_DongMau in multiplesModel.Detail_DongMaus)
                {
                    var DetailDM = db.Detail_DongMau.FirstOrDefault(p => p.DongMau_ID == detail_DongMau.DongMau_ID && p.InformationExamination_ID == detail_DongMau.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailDM.Result = detail_DongMau.Result;
                        db.Entry(DetailDM).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_DongMau detail_DongMau, List<DongMau> dongMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in dongMaus)
            {
                var checkexist = db.Detail_DongMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_DongMau.DongMau_ID = item.ID;
                    detail_DongMau.InformationExamination_ID = informationID;
                    detail_DongMau.Result = item.Result;
                    detail_DongMau.ChiDinh = item.ChiDinh;
                    db.Detail_DongMau.Add(detail_DongMau);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.DongMau_ID = new SelectList(db.DongMaus, "ID", "NameTest", detail_DongMau.DongMau_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_DongMau.InformationExamination_ID);
            //return View(detail_DongMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_DongMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_DongMau detail_DongMau = db.Detail_DongMau.Find(id);
            if (detail_DongMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_DongMau);
        }

        // POST: Admin/Detail_DongMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_DongMau detail_DongMau = db.Detail_DongMau.Find(id);
            db.Detail_DongMau.Remove(detail_DongMau);
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
