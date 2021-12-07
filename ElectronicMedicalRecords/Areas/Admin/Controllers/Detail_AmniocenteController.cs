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
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên")]
    public class Detail_AmniocenteController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_Amniocente
        public ActionResult Index()
        {
            var detail_Amniocente = db.Detail_Amniocente.Include(d => d.Amniocente).Include(d => d.InformationExamination);
            return View(detail_Amniocente.ToList());
        }

        // GET: Admin/Detail_Amniocente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Amniocente detail_Amniocente = db.Detail_Amniocente.Find(id);
            if (detail_Amniocente == null)
            {
                return HttpNotFound();
            }
            return View(detail_Amniocente);
        }

        // GET: Admin/Detail_Amniocente/Create
        public ActionResult Create()
        {
            ViewBag.Amniocente_ID = new SelectList(db.Amniocentes, "ID", "NameTest");
            ViewBag.InformationExamination = new SelectList(db.InformationExaminations, "ID", "ID");
            return View();
        }

        // POST: Admin/Detail_Amniocente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detail_Amniocente detail_Amniocente, List<Amniocente> aMniocentes, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in aMniocentes)
            {
                var checkexist = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexist == null)
                {
                    detail_Amniocente.Amniocente_ID = item.ID;
                    detail_Amniocente.InformationExamination_ID = informationID;
                    detail_Amniocente.ChiDinh = item.ChiDinh;
                    db.Detail_Amniocente.Add(detail_Amniocente);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.Amniocent_ID = new SelectList(db.Amniocentes, "ID", "NameTest", detail_Amniocente.Amniocente_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Amniocente.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Amniocente> detail_Amniocentes = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_Amniocentes == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Amniocentes = detail_Amniocentes;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_Amniocente/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_Amniocente> detail_Amniocentes = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_Amniocentes == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_Amniocentes = detail_Amniocentes;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Amniocente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_Amniocentes != null)
            {
                foreach (var detail_Amniocente in multiplesModel.Detail_Amniocentes)
                {
                    var DetailAmniocente = db.Detail_Amniocente.FirstOrDefault(p => p.Amniocente_ID == detail_Amniocente.Amniocente_ID && p.InformationExamination_ID == detail_Amniocente.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailAmniocente.Result = detail_Amniocente.Result;
                        db.Entry(DetailAmniocente).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_Amniocente detail_Amniocente, List<Amniocente> aMniocentes, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in aMniocentes)
            {
                var checkexistOld = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexistOld == null)
                {
                    detail_Amniocente.Amniocente_ID = item.ID;
                    detail_Amniocente.InformationExamination_ID = informationID;
                    detail_Amniocente.ChiDinh = item.ChiDinh;
                    db.Detail_Amniocente.Add(detail_Amniocente);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.Amniocent_ID = new SelectList(db.Amniocentes, "ID", "NameTest", detail_Amniocente.Amniocente_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Amniocente.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        // GET: Admin/Detail_Amniocente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Amniocente detail_Amniocente = db.Detail_Amniocente.Find(id);
            if (detail_Amniocente == null)
            {
                return HttpNotFound();
            }
            return View(detail_Amniocente);
        }

        // POST: Admin/Detail_Amniocente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Amniocente detail_Amniocente = db.Detail_Amniocente.Find(id);
            db.Detail_Amniocente.Remove(detail_Amniocente);
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
