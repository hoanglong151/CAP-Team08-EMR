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
    public class Detail_ViSinhController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/Detail_ViSinh
        public ActionResult Index()
        {
            var detail_ViSinh = db.Detail_ViSinh.Include(d => d.InformationExamination).Include(d => d.ViSinh);
            return View(detail_ViSinh.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOldPatient(Detail_ViSinh detail_ViSinh, List<ViSinh> viSinhs, int informationID, MultiplesModel multiplesModel)
        {
            foreach (var item in viSinhs)
            {
                var checkexistOld = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == informationID).FirstOrDefault(c => c.ID == item.ID);
                if (ModelState.IsValid && item.ChiDinh == true && checkexistOld == null)
                {
                    detail_ViSinh.ViSinh_ID = item.ID;
                    detail_ViSinh.InformationExamination_ID = informationID;
                    detail_ViSinh.ChiDinh = item.ChiDinh;
                    db.Detail_ViSinh.Add(detail_ViSinh);
                    await db.SaveChangesAsync();
                }
            }
            ViewBag.ViSinh_ID = new SelectList(db.Amniocentes, "ID", "NameTest", detail_ViSinh.ViSinh_ID);
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ViSinh.InformationExamination_ID);
            //return View(detail_CTMau);
            return RedirectToAction("Create", "MultipleModels");
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_ViSinh> detail_ViSinhs = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == id).ToList();
            if (detail_ViSinhs == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.Detail_ViSinhs = detail_ViSinhs;
            return PartialView("_DetailIE", multiplesModel);
        }

        // GET: Admin/Detail_ViSinh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ViSinh detail_ViSinh = db.Detail_ViSinh.Find(id);
            if (detail_ViSinh == null)
            {
                return HttpNotFound();
            }
            return View(detail_ViSinh);
        }

        // GET: Admin/Detail_ViSinh/Create
        public ActionResult Create()
        {
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            ViewBag.ViSinh_ID = new SelectList(db.ViSinhs, "ID", "NameTest");
            return View();
        }

        // POST: Admin/Detail_ViSinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ViSinh_ID,ChiDinh,Result,ResultNC,ResultDD,MatDo,InformationExamination_ID")] Detail_ViSinh detail_ViSinh)
        {
            if (ModelState.IsValid)
            {
                db.Detail_ViSinh.Add(detail_ViSinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", detail_ViSinh.InformationExamination_ID);
            ViewBag.ViSinh_ID = new SelectList(db.ViSinhs, "ID", "NameTest", detail_ViSinh.ViSinh_ID);
            return View(detail_ViSinh);
        }

        // GET: Admin/Detail_ViSinh/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            List<Detail_ViSinh> detail_ViSinhs = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == id).ToList();
            List<ViSinh> ViSinhs = new List<ViSinh>();
            for (int i = 0; i < detail_ViSinhs.Count; i++)
            {
                var ViSinh_ID = detail_ViSinhs[i].ViSinh_ID;
                var ViSinhCD = db.ViSinhs.FirstOrDefault(p => p.ID == ViSinh_ID);
                ViSinhCD.ChiDinh = detail_ViSinhs[i].ChiDinh;
                detail_ViSinhs[i].InformationExamination_ID = id;
                ViSinhs.Add(ViSinhCD);
            }
            if (detail_ViSinhs == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            multiplesModel.ViSinh = ViSinhs;
            multiplesModel.Detail_ViSinhs = detail_ViSinhs;
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/Detail_ViSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MultiplesModel multiplesModel)
        {
            if (multiplesModel.Detail_ViSinhs != null)
            {
                foreach (var detail_ViSinh in multiplesModel.Detail_ViSinhs)
                {
                    var DetailViSinh = db.Detail_ViSinh.FirstOrDefault(p => p.ViSinh_ID == detail_ViSinh.ViSinh_ID && p.InformationExamination_ID == detail_ViSinh.InformationExamination_ID);
                    if (ModelState.IsValid)
                    {
                        DetailViSinh.Result = detail_ViSinh.Result;
                        DetailViSinh.ResultNC = detail_ViSinh.ResultNC;
                        DetailViSinh.ResultDD = detail_ViSinh.ResultDD;
                        DetailViSinh.MatDo = detail_ViSinh.MatDo;
                        db.Entry(DetailViSinh).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Edit", "MultipleModels");
            }
            return RedirectToAction("Edit", "MultipleModels");
        }

        // GET: Admin/Detail_ViSinh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_ViSinh detail_ViSinh = db.Detail_ViSinh.Find(id);
            if (detail_ViSinh == null)
            {
                return HttpNotFound();
            }
            return View(detail_ViSinh);
        }

        // POST: Admin/Detail_ViSinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_ViSinh detail_ViSinh = db.Detail_ViSinh.Find(id);
            db.Detail_ViSinh.Remove(detail_ViSinh);
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
