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
    public class Detail_NhomMauController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_NhomMau
        public ActionResult Index()
        {
            var detail_NhomMau = db.Detail_NhomMau.Include(d => d.InformationExamination).Include(d => d.NhomMau);
            return View(detail_NhomMau.ToList());
        }

        // GET: Admin/Detail_NhomMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_NhomMau detail_NhomMau = db.Detail_NhomMau.Find(id);
            if (detail_NhomMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_NhomMau);
        }

        // GET: Admin/Detail_NhomMau/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.NhomMau_ID = new SelectList(db.NhomMaus, "ID", "NameTest");
            return View();
        }

        // POST: Admin/Detail_NhomMau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_NhomMau detail_NhomMau, List<NhomMau> nhomMaus, int informationID, MultiplesModel multiplesModel)
        {
            foreach(var item in nhomMaus)
            {
                var checkexist = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_NhomMau.NhomMau_ID = item.ID;
                    detail_NhomMau.InformationExamination_ID = informationID;
                    detail_NhomMau.ChiDinh = item.ChiDinh;
                    detail_NhomMau.Result = item.Result;
                    db.Detail_NhomMau.Add(detail_NhomMau);
                    await db.SaveChangesAsync();
                }
            }
            var check = db.Detail_NhomMau.AsNoTracking().FirstOrDefault(p => p.InformationExamination_ID == informationID);
            if (check != null)
            {
                multiplesModel.InformationExamination.ResultNhomMau = false;
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_NhomMau.InformationExamination_ID);
            ViewBag.NhomMau_ID = new SelectList(db.NhomMaus, "ID", "NameTest", detail_NhomMau.NhomMau_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_NhomMau> detail_NhomMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<NhomMau> nhomMaus = new List<NhomMau>();
            for (int i = 0; i < detail_NhomMaus.Count; i++)
            {
                var NhomMau_ID = detail_NhomMaus[i].NhomMau_ID;
                var NhomMauCD = db.NhomMaus.FirstOrDefault(p => p.ID == NhomMau_ID);
                NhomMauCD.ChiDinh = detail_NhomMaus[i].ChiDinh;
                NhomMauCD.Result = detail_NhomMaus[i].Result;
                detail_NhomMaus[i].InformationExamination_ID = id;
                nhomMaus.Add(NhomMauCD);
            }
            if (detail_NhomMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.NhomMau = nhomMaus;
            multiplesModel.Detail_NhomMaus = detail_NhomMaus;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_NhomMau/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_NhomMau> detail_NhomMaus = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == id).ToList();
            List<NhomMau> nhomMaus = new List<NhomMau>();
            for (int i = 0; i < detail_NhomMaus.Count; i++)
            {
                var NhomMau_ID = detail_NhomMaus[i].NhomMau_ID;
                var NhomMauCD = db.NhomMaus.FirstOrDefault(p => p.ID == NhomMau_ID);
                NhomMauCD.ChiDinh = detail_NhomMaus[i].ChiDinh;
                NhomMauCD.Result = detail_NhomMaus[i].Result;
                detail_NhomMaus[i].InformationExamination_ID = id;
                nhomMaus.Add(NhomMauCD);
            }
            if (detail_NhomMaus == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.NhomMau = nhomMaus;
            multiplesModel.Detail_NhomMaus = detail_NhomMaus;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_NhomMau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_NhomMaus != null)
            {
                foreach (var detail_NhomMau in multiplesModel.Detail_NhomMaus)
                {
                    var DetailNM = db.Detail_NhomMau.AsNoTracking().FirstOrDefault(p => p.NhomMau_ID == detail_NhomMau.NhomMau_ID && p.InformationExamination_ID == detail_NhomMau.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailNM.Result = detail_NhomMau.Result;
                        db.Entry(DetailNM).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                var checkResult = multiplesModel.Detail_NhomMaus.All(p => p.Result != null);
                if (checkResult == true)
                {
                    multiplesModel.InformationExamination.ResultNhomMau = true;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(List<NhomMau> nhomMaus, int informationID, MultiplesModel multiplesModel)
        {
            Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
            foreach (var item in nhomMaus)
            {
                var checkexist = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_NhomMau.NhomMau_ID = item.ID;
                    detail_NhomMau.InformationExamination_ID = informationID;
                    detail_NhomMau.ChiDinh = item.ChiDinh;
                    detail_NhomMau.Result = item.Result;
                    db.Detail_NhomMau.Add(detail_NhomMau);
                    await db.SaveChangesAsync();
                }
            }
            var check = db.Detail_NhomMau.AsNoTracking().FirstOrDefault(p => p.InformationExamination_ID == informationID);
            if (check != null)
            {
                multiplesModel.InformationExamination.ResultNhomMau = false;
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_NhomMau.InformationExamination_ID);
            ViewBag.NhomMau_ID = new SelectList(db.NhomMaus, "ID", "NameTest", detail_NhomMau.NhomMau_ID);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_NhomMau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_NhomMau detail_NhomMau = db.Detail_NhomMau.Find(id);
            if (detail_NhomMau == null)
            {
                return HttpNotFound();
            }
            return View(detail_NhomMau);
        }

        // POST: Admin/Detail_NhomMau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_NhomMau detail_NhomMau = db.Detail_NhomMau.Find(id);
            db.Detail_NhomMau.Remove(detail_NhomMau);
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
