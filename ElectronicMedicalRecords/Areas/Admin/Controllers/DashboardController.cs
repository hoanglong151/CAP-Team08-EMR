using ElectronicMedicalRecords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Giám Đốc,QTV")]
    public class DashboardController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            var patients = db.Patients.ToList();
            var allUsers = db.AspNetUsers.ToList();
            var userList = db.Users.Where(p => p.ActiveAccount == true).ToList();
            var userBS = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Bác Sĩ")).ToList();
            var userGD = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Giám Đốc")).ToList();
            var userKTV = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Kỹ Thuật Viên")).ToList();
            var userQTV = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("QTV")).ToList();
            var userTN = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Thu Ngân")).ToList();
            var userYTa = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Y tá/Điều dưỡng")).ToList();
            var userOnline = db.Users.ToList();
            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.userBS = userBS;
            dashboardModel.userGD = userGD;
            dashboardModel.userKTV = userKTV;
            dashboardModel.userQTV = userQTV;
            dashboardModel.userTN = userTN;
            dashboardModel.userYTa = userYTa;
            dashboardModel.patients = patients;
            dashboardModel.users = userOnline;
            dashboardModel.userList = userList;
            dashboardModel.listUsersOnline = (List<string>)Session["ListUsersOnline"];
            return View(dashboardModel);
        }

        public ActionResult GetData()
        {
            List<string> date = new List<string>();
            List<int> patient = new List<int>();
            List<double> priceExamination = new List<double>();
            List<double> pricePrescription = new List<double>();
            List<double> priceSubclinical = new List<double>();
            for(int i = 7; i > 0; i--)
            {
                int priceInfo = 0;
                int pricePres = 0;
                int priceTotalTest = 0;
                DateTime Datetime = DateTime.Now;
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                Datetime = Datetime.AddDays(DateTime.Today.Day - (DateTime.Today.Day + i - 1));
                Datetime = Datetime.AddHours(Datetime.Hour - (Datetime.Hour * 2)).AddMinutes(Datetime.Minute - (Datetime.Minute * 2)).AddSeconds(Datetime.Second - (Datetime.Second * 2));
                DateTime DateEnd = Datetime + timeEnd;
                var numUsers = db.InformationExaminations.Where(p => p.DateExamine >= Datetime && p.DateExamine < DateEnd).Count();
                var priceExam = db.InformationExaminations.Where(p => p.DateExamine >= Datetime && p.DateExamine < DateEnd).ToList();
                foreach(var item in priceExam)
                {
                    if(item.PriceExamination != null)
                    {
                        priceInfo += (int)item.PriceExamination;
                    }
                    if (item.PricePrescription != null)
                    {
                        pricePres += (int)item.PricePrescription;
                    }
                    if (item.PriceTest != null)
                    {
                        priceTotalTest += (int)item.PriceTest;
                    }
                }
                priceExamination.Add(priceInfo);
                pricePrescription.Add(pricePres);
                priceSubclinical.Add(priceTotalTest);
                date.Add(Datetime.ToString("dd/MM"));
                patient.Add(numUsers);
            }
            return Json(new { datetime = date, numUser = patient, priceExaminationInfo = priceExamination, pricePrescriptionDetail = pricePrescription, priceTestSubclinical = priceSubclinical }, JsonRequestBehavior.AllowGet);
        }
    }
}