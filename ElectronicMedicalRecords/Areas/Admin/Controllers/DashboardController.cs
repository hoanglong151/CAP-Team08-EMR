using ElectronicMedicalRecords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View(dashboardModel);
        }

        public async Task<int> PriceCTMau(InformationExamination informationExamination)
        {
            int priceCTMaus = 0;
            var priceCTMau = db.Detail_CTMau.FirstOrDefault(p => p.InformationExamination_ID == informationExamination.ID);
            if (informationExamination.PriceCTMaus != null && priceCTMau != null)
            {
                priceCTMaus += (int)informationExamination.PriceCTMaus;
            }
            return priceCTMaus;
        }

        public async Task<int> PriceAmniocente(InformationExamination informationExamination)
        {
            int priceAmniocentes = 0;
            var priceAmniocente = db.Detail_Amniocente.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceAmniocente != null)
            {
                foreach (var priceAmnio in priceAmniocente)
                {
                    priceAmniocentes += priceAmnio.Amniocente.Price;
                }
            }
            return priceAmniocentes;
        }

        public async Task<int> PriceDongMau(InformationExamination informationExamination)
        {
            int priceDongMaus = 0;
            var priceDongMau = db.Detail_DongMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceDongMau != null)
            {
                foreach (var priceDM in priceDongMau)
                {
                    priceDongMaus += priceDM.DongMau.Price;
                }
            }
            return priceDongMaus;
        }

        public async Task<int> PriceImmune(InformationExamination informationExamination)
        {
            int priceImmunes = 0;
            var priceImmune = db.Detail_Immune.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceImmune != null)
            {
                foreach (var priceIm in priceImmune)
                {
                    priceImmunes += priceIm.Immune.Price;
                }
            }
            return priceImmunes;
        }

        public async Task<int> PriceNhomMau(InformationExamination informationExamination)
        {
            int priceNhomMaus = 0;
            var priceNhomMau = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceNhomMau != null)
            {
                foreach (var priceNM in priceNhomMau)
                {
                    priceNhomMaus += priceNM.NhomMau.Price;
                }
            }
            return priceNhomMaus;
        }

        public async Task<int> PriceSinhHoaMau(InformationExamination informationExamination)
        {
            int priceSHMaus = 0;
            var priceSHMau = db.Detail_SinhHoaMau.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceSHMau != null)
            {
                foreach (var priceSHM in priceSHMau)
                {
                    priceSHMaus += priceSHM.SinhHoaMau.Price;
                }
            }
            return priceSHMaus;
        }

        public async Task<int> PriceUrine(InformationExamination informationExamination)
        {
            int priceUrines = 0;
            var priceUrine = db.Detail_Urine.Where(p => p.InfomationExamination_ID == informationExamination.ID).ToList();
            if (priceUrine != null)
            {
                foreach (var priceUr in priceUrine)
                {
                    priceUrines += priceUr.Urine.Price;
                }
            }
            return priceUrines;
        }

        public async Task<int> PriceViSinh(InformationExamination informationExamination)
        {
            int priceViSinhs = 0;
            var priceVSinh = db.Detail_ViSinh.Where(p => p.InformationExamination_ID == informationExamination.ID).ToList();
            if (priceVSinh != null)
            {
                foreach (var priceVS in priceVSinh)
                {
                    priceViSinhs += priceVS.ViSinh.Price;
                }
            }
            return priceViSinhs;
        }

        public async Task<ActionResult> GetData()
        {
            List<string> date = new List<string>();
            List<int> patient = new List<int>();
            List<string> priceExamination = new List<string>();
            List<string> pricePrescription = new List<string>();
            List<string> priceSubclinical = new List<string>();
            for(int i = 0; i < 7; i++)
            {
                int priceInfo = 0;
                int pricePres = 0;
                int priceTotalTest = 0;
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
                        if(price.TotalPrice != null)
                        {
                            pricePres += (int)price.TotalPrice;
                        }
                    }
                    var priceCTMau = PriceCTMau(item);
                    var priceAmniocente = PriceAmniocente(item);
                    var priceDongMau = PriceDongMau(item);
                    var priceImmune = PriceImmune(item);
                    var priceNhomMau = PriceNhomMau(item);
                    var priceSinhHoaMau = PriceSinhHoaMau(item);
                    var priceUrine = PriceUrine(item);
                    var priceViSinh = PriceViSinh(item);
                    var result = await Task.WhenAll(priceCTMau, priceAmniocente
                        , priceDongMau, priceImmune, priceNhomMau, priceSinhHoaMau
                        , priceUrine, priceViSinh);
                    foreach(var price in result)
                    {
                        priceTotalTest += price;
                    }
                }
                priceExamination.Add(priceInfo.ToString());
                pricePrescription.Add(pricePres.ToString());
                priceSubclinical.Add(priceTotalTest.ToString());
                date.Add(Datetime.ToString("dd/MM"));
                patient.Add(numUsers);
            }
            return Json(new { datetime = date, numUser = patient, priceExaminationInfo = priceExamination, pricePrescriptionDetail = pricePrescription, priceTestSubclinical = priceSubclinical }, JsonRequestBehavior.AllowGet);
        }
    }
}