using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Controllers
{
    public class HomeController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        public ActionResult Index()
        {
            var listPatient = db.Patients.Count();
            var role = db.AspNetRoles.FirstOrDefault(x => x.Name == "Giám Đốc");
            var director = role.AspNetUsers.FirstOrDefault();
            var listDoctor = db.Users.Count();
            ViewBag.ListPatient = listPatient;
            ViewBag.ListDoctor = listDoctor;
            ViewBag.Director = director.Users.FirstOrDefault();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Doctors()
        {
            var listDoctor = db.Users.Where(p => p.IsShow == true).ToList();
            return View(listDoctor);

        } public ActionResult News()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}