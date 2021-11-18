using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.HomeTown).Include(u => u.Nation).Include(u => u.Religion).Include(g => g.Gender).ToList();
            return View(users);
        }

        [ChildActionOnly]
        public ActionResult RenderUser()
        {
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(id => id.UserID == UserID);
            return PartialView("_AdminUser", userID);
        }

        public ActionResult HomePage()
        {
            return View();
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", user.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", user.Nation_ID);
            ViewBag.Religion_ID = new SelectList(db.Religions, "ID", "Name", user.Religion_ID);
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", user.Gender_ID);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Edit(HttpPostedFileBase Image1, User user)
        {
            var check = db.Users.Find(user.ID);
            if (ModelState.IsValid)
            {
                if (Image1 != null)
                {
                    var myAccount = new Account
                    {
                        ApiKey = ConfigurationManager.AppSettings["CloudinaryAPIKey"]
                    ,
                        ApiSecret = ConfigurationManager.AppSettings["CloudinarySecret"]
                    ,
                        Cloud = ConfigurationManager.AppSettings["CloudinaryName"]
                    };
                    Cloudinary _cloudinary = new Cloudinary(myAccount);
                    var extension = Path.GetExtension(Image1.FileName.ToLowerInvariant());
                    var fileName = Guid.NewGuid() + extension;
                    var direct = Path.Combine(Server.MapPath("~/UploadImage"), fileName);
                    Image1.SaveAs(direct);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(direct),
                        Folder = "AdminEMR",
                    };
                    var uploadResult = _cloudinary.Upload(uploadParams);
                    user.Image = uploadResult.SecureUri.AbsoluteUri;
                }
                var existData = db.Users.Find(user.ID);
                db.Entry(existData).CurrentValues.SetValues(user);
                db.SaveChanges();
                if(check.Privacy == true && check.ActiveAccount == true)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("HomePage");
            }
            ViewBag.HomeTown_ID = new SelectList(db.HomeTowns, "ID", "HomeTown1", user.HomeTown_ID);
            ViewBag.Nation_ID = new SelectList(db.Nations, "ID", "Name", user.Nation_ID);
            ViewBag.Religion_ID = new SelectList(db.Religions, "ID", "Name", user.Religion_ID);
            ViewBag.Gender_ID = new SelectList(db.Genders, "ID", "Gender1", user.Gender_ID);
            return View(user);
        }

        public ActionResult DenyAccount()
        {
            return View();
        }
        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            AspNetUser aspNetUser = db.AspNetUsers.Find(user.UserID);
            var aspNetRole = db.AspNetRoles.ToList();
            for(int i = 0; i < aspNetRole.Count; i++)
            {
                AspNetRole roleid = db.AspNetRoles.Find(aspNetRole[i].Id);
                roleid.AspNetUsers.Remove(aspNetUser);
            }
            user.ActiveAccount = false;
            db.Entry(user).State = EntityState.Modified;
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
