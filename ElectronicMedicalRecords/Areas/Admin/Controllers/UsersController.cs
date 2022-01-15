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
    public class UsersController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        [Authorize]
        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.HomeTown).Include(u => u.Nation).Include(u => u.Religion).Include(g => g.Gender).ToList();
            return View(users);
        }
        [AllowAnonymous]
        public ActionResult Status()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<string> listUser = new List<string>();
            var usersOnline = HttpRuntime.Cache["LoggedInUsers"] as Dictionary<string, DateTime>;
            if (usersOnline != null)
            {
                HttpRuntime.Cache["LoggedInUsers"] = usersOnline;
                if (User.Identity.IsAuthenticated && usersOnline.ContainsKey(System.Web.HttpContext.Current.User.Identity.GetUserId()) == false)
                {
                    usersOnline.Add(System.Web.HttpContext.Current.User.Identity.GetUserId(), DateTime.Now);
                }
                foreach (var user in usersOnline)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        listUser.Add(user.Key);
                    }
                    var time = user.Value.AddMinutes(10);
                    if (time <= DateTime.Now)
                    {
                        listUser.Remove(user.Key);
                    }
                }
                foreach (var item in usersOnline.ToList())
                {
                    if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        usersOnline.Remove(item.Key);
                    }
                }
                Session["ListUsersOnline"] = listUser;
                return Json(new { success = true, data = listUser }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login","Account", new { Area = "" });
            }
        }

        public ActionResult updateUser()
        {
            var usersOnline = HttpRuntime.Cache["LoggedInUsers"] as Dictionary<string, DateTime>;
            var userUpdate = User.Identity.GetUserId();
            usersOnline.Remove(userUpdate);
            usersOnline.Add(userUpdate, DateTime.Now);
            return View("Status");
        }

        [ChildActionOnly]
        public ActionResult RenderUser()
        {
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(id => id.UserID == UserID);
            return PartialView("_AdminUser", userID);
        }
        [Authorize]
        public ActionResult HomePage()
        {
            Random randomNum = new Random();
            string[] random = {"Khó khăn không trường tồn, chỉ có con người cứng rắn trường tồn.",
                "Khó khăn nào rồi cũng qua. Cũng giống như sau cơn mưa, trời lại sáng.",
                "Không bao giờ cúi đầu, không bao giờ bỏ cuộc hay chỉ ngồi than thở. Hãy tìm một cách khác. Và đừng cầu nguyện khi trời mưa nếu bạn không cầu nguyện khi trời nắng",
                "Sẽ không bao giờ có bế tắc thật sự khi trong bạn còn niềm tin. Chỉ cần có niềm tin, bạn sẽ có hi vọng, sẽ tìm thấy con đường để bước tiếp.",
                "Nếu bạn không thích điều gì đó, hãy thay đổi nó. Nếu bạn không thể thay đổi nó, hãy thay đổi cách suy nghĩ của bạn về nó." };
            var indexRandom = randomNum.Next(0, 4);
            var status = random[indexRandom];
            ViewBag.StatusAccess = status;
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
                return RedirectToAction("DenyAccount");
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
        [Authorize(Roles = "Giám Đốc,QTV")]
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
