using ElectronicMedicalRecords.Models;
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
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Doctors()
        {
            var listDoctor = db.Users.ToList();
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