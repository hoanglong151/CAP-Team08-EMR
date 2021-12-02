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
        public async Task<ActionResult> Create(List<Amniocente> aMniocentes, int informationID, MultiplesModel multiplesModel)
        {
            Detail_Amniocente detail_Amniocente = new Detail_Amniocente();
            foreach (var item in aMniocentes)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_Amniocente.Amniocente_ID = item.ID;
                    detail_Amniocente.InformationExamination_ID = informationID;
                    detail_Amniocente.ChiDinh = item.ChiDinh;
                    detail_Amniocente.Result = item.Result;
                    db.Detail_Amniocente.Add(detail_Amniocente);
                    await db.SaveChangesAsync();
                }
            }
            var check = db.Detail_Amniocente.AsNoTracking().FirstOrDefault(p => p.InformationExamination_ID == informationID);
            if (check != null)
            {
                multiplesModel.InformationExamination.ResultDichChocDo = false;
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            List<Amniocente> aMniocentes = new List<Amniocente>();
            for (int i = 0; i < detail_Amniocentes.Count; i++)
            {
                var Amniocente_ID = detail_Amniocentes[i].Amniocente_ID;
                var AmniocenteCD = db.Amniocentes.FirstOrDefault(p => p.ID == Amniocente_ID);
                AmniocenteCD.ChiDinh = detail_Amniocentes[i].ChiDinh;
                AmniocenteCD.Result = detail_Amniocentes[i].Result;
                detail_Amniocentes[i].InformationExamination_ID = id;
                aMniocentes.Add(AmniocenteCD);
            }
            if (detail_Amniocentes == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Amniocente = aMniocentes;
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
            List<Amniocente> aMniocentes = new List<Amniocente>();
            for (int i = 0; i < detail_Amniocentes.Count; i++)
            {
                var Amniocente_ID = detail_Amniocentes[i].Amniocente_ID;
                var AmniocenteCD = db.Amniocentes.FirstOrDefault(p => p.ID == Amniocente_ID);
                AmniocenteCD.ChiDinh = detail_Amniocentes[i].ChiDinh;
                AmniocenteCD.Result = detail_Amniocentes[i].Result;
                detail_Amniocentes[i].InformationExamination_ID = id;
                aMniocentes.Add(AmniocenteCD);
            }
            if (detail_Amniocentes == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Amniocente = aMniocentes;
            multiplesModel.Detail_Amniocentes = detail_Amniocentes;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_Amniocente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_Amniocentes != null)
            {
                foreach (var detail_Amniocente in multiplesModel.Detail_Amniocentes)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(detail_Amniocente).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                var checkResult = multiplesModel.Detail_Amniocentes.All(p => p.Result != null);
                if(checkResult == true)
                {
                    multiplesModel.InformationExamination.ResultDichChocDo = true;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            Detail_Amniocente detail_Amniocente = new Detail_Amniocente();
            foreach (var item in multiplesModel.Amniocente)
            {
                if (ModelState.IsValid && item.ChiDinh == true)
                {
                    detail_Amniocente.Amniocente_ID = item.ID;
                    detail_Amniocente.InformationExamination_ID = multiplesModel.InformationExamination.ID;
                    detail_Amniocente.ChiDinh = item.ChiDinh;
                    detail_Amniocente.Result = item.Result;
                    multiplesModel.InformationExamination.ResultDichChocDo = false;
                    db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                    db.Detail_Amniocente.Add(detail_Amniocente);
                    db.SaveChanges();
                }
            }
            ViewBag.Amniocent_ID = new SelectList(db.Amniocentes, "ID", "NameTest", detail_Amniocente.Amniocente_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_Amniocente.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("CreateOldPatient", "MultipleModels");
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
