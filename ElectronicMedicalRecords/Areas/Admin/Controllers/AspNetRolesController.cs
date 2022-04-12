using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Giám Đốc,QTV")]
    public class AspNetRolesController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();

        // GET: Admin/AspNetRoles
        public ActionResult Index()
        {
            var getUser = db.Users.Where(p => p.Image != null && p.Name != null).ToList();
            ViewBag.User = getUser;
            ViewBag.CountUserExist = TempData["CountUserExist"];
            var roles = db.AspNetRoles.ToList();
            return View(roles);
        }

        public ActionResult AddUserRole(List<string> Users, AspNetRole role)
        {
            var countExist = 0;
            for(int i = 0; i < Users.Count; i++)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var userID = Users[i];
                var roleID = db.AspNetRoles.Find(role.Id);
                var user = UserManager.FindById(userID);
                var activeAccount = db.Users.FirstOrDefault(c => c.UserID == userID);
                if(UserManager.IsInRole(user.Id, roleID.Name))
                {
                    countExist += 1;
                }
                else
                {
                    UserManager.AddToRole(user.Id, roleID.Name);
                    activeAccount.ActiveAccount = true;
                    db.Entry(activeAccount).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            TempData["CountUserExist"] = countExist;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteUserRole(string roleID, string userID)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(userID);
            AspNetRole aspNetRole = db.AspNetRoles.Find(roleID);
            var aspNetRoleList = db.AspNetRoles.ToList();
            User user = db.Users.FirstOrDefault(p => p.UserID == userID);
            aspNetRole.AspNetUsers.Remove(aspNetUser);
            var count = 0;
            for (int i = 0; i < aspNetRoleList.Count; i++)
            {
                AspNetRole roleid = db.AspNetRoles.Find(aspNetRoleList[i].Id);
                var checkexistUser = roleid.AspNetUsers.FirstOrDefault(c => c.Id == aspNetUser.Id);
                if(checkexistUser != null)
                {
                    count += 1;
                }
            }
            if(count == 0)
            {
                user.ActiveAccount = false;
                db.Entry(user).State = EntityState.Modified;
            }
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
