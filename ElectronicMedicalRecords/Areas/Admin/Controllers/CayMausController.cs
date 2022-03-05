using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ElectronicMedicalRecords.Models;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class CayMausController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/CayMaus
        public ActionResult Index()
        {
            var cayMaus = db.CayMaus.Include(c => c.InformationExamination);
            return View(cayMaus.ToList());
        }

        public ActionResult DetailIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            CayMau cayMau = db.CayMaus.FirstOrDefault(p => p.InformationExamination_ID == informationExamination.ID);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            //multiplesModel.CayMau = cayMau;
            return PartialView("_DetailIE", multiplesModel);
        }

        public ActionResult CreateOldPatient()
        {
            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return PartialView("_CreateOldPatient");
        }

        // GET: Admin/CayMaus/Create
        public ActionResult Create()
        {
            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID");
            return PartialView("_Create");
        }

        // POST: Admin/CayMaus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Create(MultiplesModel multiplesModel, HttpServerUtilityBase Server)
        {
            //var myAccount = new Account
            //{
            //    ApiKey = ConfigurationManager.AppSettings["CloudinaryAPIKey"]
            //        ,
            //    ApiSecret = ConfigurationManager.AppSettings["CloudinarySecret"]
            //        ,
            //    Cloud = ConfigurationManager.AppSettings["CloudinaryName"]
            //};
            //Cloudinary _cloudinary = new Cloudinary(myAccount);
            //foreach(var imageNC in multiplesModel.CayMau.ImageNuoiCayFiles)
            //{
            //    var extensionNC = Path.GetExtension(imageNC.FileName.ToLowerInvariant());
            //    var fileNameNC = Guid.NewGuid() + extensionNC;
            //    var directNC = Path.Combine(Server.MapPath("~/UploadImage"), fileNameNC);
            //    imageNC.SaveAs(directNC);
            //    var uploadParams = new ImageUploadParams()
            //    {
            //        File = new FileDescription(directNC),
            //        Folder = "ViSinh",
            //    };
            //    var uploadResult = _cloudinary.Upload(uploadParams);
            //    multiplesModel.CayMau.ImageNuoiCay += uploadResult.SecureUri.AbsoluteUri + ",";
            //}
            //foreach (var imageDD in multiplesModel.CayMau.ImageDinhDanhFiles)
            //{
            //    var extensionDD = Path.GetExtension(imageDD.FileName.ToLowerInvariant());
            //    var fileNameDD = Guid.NewGuid() + extensionDD;
            //    var directDD = Path.Combine(Server.MapPath("~/UploadImage"), fileNameDD);
            //    imageDD.SaveAs(directDD);
            //    var uploadParams = new ImageUploadParams()
            //    {
            //        File = new FileDescription(directDD),
            //        Folder = "ViSinh",
            //    };
            //    var uploadResult = _cloudinary.Upload(uploadParams);
            //    multiplesModel.CayMau.ImageDinhDanh += uploadResult.SecureUri.AbsoluteUri + ",";
            //}
            //if (ModelState.IsValid && multiplesModel.CayMau.ChiDinh == true)
            //{
            //    multiplesModel.CayMau.InformationExamination_ID = multiplesModel.InformationExamination.ID;
            //    //multiplesModel.CayMau.ImageNuoiCay.Remove(multiplesModel.CayMau.ImageNuoiCay.Length - 1);
            //    //multiplesModel.CayMau.ImageDinhDanh.Remove(multiplesModel.CayMau.ImageDinhDanh.Length - 1);
            //    multiplesModel.InformationExamination.ResultViSinh = false;
            //    db.CayMaus.Add(multiplesModel.CayMau);
            //    db.SaveChanges();
            //    return RedirectToAction("Create", "MultipleModels"); 
            //}

            //ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", multiplesModel.CayMau.InformationExamination_ID);
            //return View(multiplesModel.CayMau);
            return View();
        }

        // GET: Admin/CayMaus/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            InformationExamination informationExamination = new InformationExamination();
            informationExamination.ID = id;
            CayMau cayMau = db.CayMaus.FirstOrDefault(p => p.InformationExamination_ID == informationExamination.ID);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            multiplesModel.InformationExamination = informationExamination;
            //multiplesModel.CayMau = cayMau;
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", cayMau.InformationExamination_ID);
            return PartialView("_Edit", multiplesModel);
        }

        // POST: Admin/CayMaus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CayMau cayMau, HttpServerUtilityBase Server)
        {
            var myAccount = new Account
            {
                ApiKey = ConfigurationManager.AppSettings["CloudinaryAPIKey"],
                ApiSecret = ConfigurationManager.AppSettings["CloudinarySecret"],
                Cloud = ConfigurationManager.AppSettings["CloudinaryName"]
            };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            List<string> listOldImageNC = cayMau.ImageNuoiCay.Remove(cayMau.ImageNuoiCay.Length - 1).Split(',').ToList();
            List<string> listOldImageDD = cayMau.ImageDinhDanh.Remove(cayMau.ImageDinhDanh.Length - 1).Split(',').ToList();
            for(int nc = 0; nc < listOldImageNC.Count; nc++)
            {
                listOldImageNC[nc] = listOldImageNC[nc] + ",";
            }
            for (int dd = 0; dd < listOldImageDD.Count; dd++)
            {
                listOldImageDD[dd] = listOldImageDD[dd] + ",";
            }
            for (int i = 0; i < cayMau.ImageNuoiCayFiles.Length; i++)
            {
                if (cayMau.ImageNuoiCayFiles[i] != null)
                {
                    var extensionNC = Path.GetExtension(cayMau.ImageNuoiCayFiles[i].FileName.ToLowerInvariant());
                    var fileNameNC = Guid.NewGuid() + extensionNC;
                    var directNC = Path.Combine(Server.MapPath("~/UploadImage"), fileNameNC);
                    cayMau.ImageNuoiCayFiles[i].SaveAs(directNC);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(directNC),
                        Folder = "ViSinh",
                    };
                    var uploadResult = _cloudinary.Upload(uploadParams);
                    if ((i + 1) < listOldImageNC.Count)
                    {
                        listOldImageNC[i] = uploadResult.SecureUrl.AbsoluteUri + ",";
                        cayMau.ImageNuoiCay = string.Join("", listOldImageNC);
                    }
                    else if (cayMau.ExistImageNC != null && cayMau.ExistImageNC.Length < cayMau.ImageNuoiCayFiles.Length && (i + 1) > listOldImageNC.Count)
                    {
                        cayMau.ImageNuoiCay += uploadResult.SecureUrl.AbsoluteUri + ",";
                    }
                    else
                    {
                        listOldImageNC[i] = uploadResult.SecureUrl.AbsoluteUri + ",";
                        cayMau.ImageNuoiCay = string.Join("", listOldImageNC);
                    }
                }
                else if (cayMau.ExistImageNC.Length < listOldImageNC.Count)
                {
                    cayMau.ImageNuoiCay = null;
                    listOldImageNC = new List<string>();
                    for (int item = 0; item < cayMau.ExistImageNC.Length; item++)
                    {
                        cayMau.ImageNuoiCay += cayMau.ExistImageNC[item] + ",";
                        listOldImageNC.Add(cayMau.ExistImageNC[item] + ",");
                    }
                }
                else
                {
                    continue;
                }
            }
            for (int j = 0; j < cayMau.ImageDinhDanhFiles.Length; j++)
            {
                if (cayMau.ImageDinhDanhFiles[j] != null)
                {
                    var extensionDD = Path.GetExtension(cayMau.ImageDinhDanhFiles[j].FileName.ToLowerInvariant());
                    var fileNameDD = Guid.NewGuid() + extensionDD;
                    var directDD = Path.Combine(Server.MapPath("~/UploadImage"), fileNameDD);
                    cayMau.ImageDinhDanhFiles[j].SaveAs(directDD);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(directDD),
                        Folder = "ViSinh",
                    };
                    var uploadResult = _cloudinary.Upload(uploadParams);
                    if ((j + 1) < listOldImageDD.Count)
                    {
                        listOldImageDD[j] = uploadResult.SecureUrl.AbsoluteUri + ",";
                        cayMau.ImageDinhDanh = string.Join("", listOldImageDD) + ",";
                    }
                    else if (cayMau.ExistImageDD != null && cayMau.ExistImageDD.Length < cayMau.ImageDinhDanhFiles.Length && (j + 1) > listOldImageDD.Count)
                    {
                        cayMau.ImageDinhDanh += uploadResult.SecureUrl.AbsoluteUri + ",";
                    }
                    else
                    {
                        listOldImageDD[j] = uploadResult.SecureUrl.AbsoluteUri + ",";
                        cayMau.ImageDinhDanh = string.Join("", listOldImageDD);
                    }
                }
                else if (cayMau.ExistImageDD.Length < listOldImageDD.Count)
                {
                    cayMau.ImageDinhDanh = null;
                    listOldImageDD = new List<string>();
                    for (int itemDD = 0; itemDD < cayMau.ExistImageDD.Length; itemDD++)
                    {
                        cayMau.ImageDinhDanh += cayMau.ExistImageDD[itemDD] + ",";
                        listOldImageDD.Add(cayMau.ExistImageDD[itemDD] + ",");
                    }
                }
                else
                {
                    continue;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(cayMau).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "MultipleModels");
            }
            ViewBag.InformationExamination_ID = new SelectList(db.InformationExaminations, "ID", "ID", cayMau.InformationExamination_ID);
            return RedirectToAction("Edit", "MultipleModels");
        }

        // GET: Admin/CayMaus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CayMau cayMau = db.CayMaus.Find(id);
            if (cayMau == null)
            {
                return HttpNotFound();
            }
            return View(cayMau);
        }

        // POST: Admin/CayMaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CayMau cayMau = db.CayMaus.Find(id);
            db.CayMaus.Remove(cayMau);
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
