using ElectronicMedicalRecords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            var patients = db.Patients.ToList();
            var allUsers = db.AspNetUsers.ToList();
            var userBS = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Bác Sĩ")).ToList();
            var userGD = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Giám Đốc")).ToList();
            var userKTV = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Kỹ Thuật Viên")).ToList();
            var userQTV = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("QTV")).ToList();
            var userTN = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Thu Ngân")).ToList();
            var userYTa = allUsers.Where(p => p.AspNetRoles.Select(role => role.Name).Contains("Y tá/Điều dưỡng")).ToList();
            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.userBS = userBS;
            dashboardModel.userGD = userGD;
            dashboardModel.userKTV = userKTV;
            dashboardModel.userQTV = userQTV;
            dashboardModel.userTN = userTN;
            dashboardModel.userYTa = userYTa;
            dashboardModel.patients = patients;
            return View(dashboardModel);
        }

        public ActionResult GetData()
        {
            List<string> date = new List<string>();
            List<int> patient = new List<int>();
            List<int> priceExamination = new List<int>();
            List<int> pricePrescription = new List<int>();
            List<int> priceSubclinical = new List<int>();
            for(int i = 0; i < 7; i++)
            {
                int priceInfo = 0;
                int pricePres = 0;
                int priceTest = 0;
                DateTime Datetime = DateTime.Now;
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                Datetime = Datetime.AddDays(DateTime.Today.Day - (DateTime.Today.Day + i));
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
                    var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    foreach(var price in listPrescription)
                    {
                        pricePres += (price.NumMedication * price.Medication.Price);
                    }
                    var priceCTMau = db.Detail_CTMau.FirstOrDefault(p => p.InformationExamination_ID == item.ID);
                    if(item.PriceCTMaus != null && priceCTMau != null)
                    {
                        priceTest += (int)item.PriceCTMaus;
                    }
                    var priceAmniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if(priceAmniocente != null)
                    {
                        foreach (var priceAmnio in priceAmniocente)
                        {
                            priceTest += priceAmnio.Amniocente.Price;
                        }
                    }
                    var priceDongMau = db.Detail_DongMau.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if (priceDongMau != null)
                    {
                        foreach (var priceDM in priceDongMau)
                        {
                            priceTest += priceDM.DongMau.Price;
                        }
                    }
                    var priceImmune = db.Detail_Immune.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if (priceImmune != null)
                    {
                        foreach (var priceIm in priceImmune)
                        {
                            priceTest += priceIm.Immune.Price;
                        }
                    }
                    var priceNhomMau = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if (priceNhomMau != null)
                    {
                        foreach (var priceNM in priceNhomMau)
                        {
                            priceTest += priceNM.NhomMau.Price;
                        }
                    }
                    var priceSHMau = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if (priceSHMau != null)
                    {
                        foreach (var priceSHM in priceSHMau)
                        {
                            priceTest += priceSHM.SinhHoaMau.Price;
                        }
                    }
                    var priceUrine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == item.ID).ToList();
                    if (priceUrine != null)
                    {
                        foreach (var priceUr in priceUrine)
                        {
                            priceTest += priceUr.Urine.Price;
                        }
                    }
                    var priceVSinh = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == item.ID).ToList();
                    if (priceVSinh != null)
                    {
                        foreach (var priceVS in priceVSinh)
                        {
                            priceTest += priceVS.ViSinh.Price;
                        }
                    }
                }
                priceExamination.Add(priceInfo);
                pricePrescription.Add(pricePres);
                priceSubclinical.Add(priceTest);
                date.Add(Datetime.ToString("dd/MM"));
                patient.Add(numUsers);
            }
            return Json(new { datetime = date, numUser = patient, priceExaminationInfo = priceExamination, pricePrescriptionDetail = pricePrescription, priceTestSubclinical = priceSubclinical }, JsonRequestBehavior.AllowGet);
        }
    }
}