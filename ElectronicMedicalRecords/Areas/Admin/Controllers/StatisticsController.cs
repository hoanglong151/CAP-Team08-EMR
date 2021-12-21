using ElectronicMedicalRecords.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/Statistics
        public ActionResult StatisByDoctorAndCondition()
        {
            var users = db.Users.ToList();
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            foreach (var user in users)
            {
                var listInfo = user.InformationExaminations;
                foreach(var status in statusP)
                {
                    User usersInfo = new User();
                    StatisticModel statisticModels = new StatisticModel();
                    db.Users.Add(usersInfo);
                    usersInfo.AspNetUser = user.AspNetUser;
                    var originalEntityValues = db.Entry(user).CurrentValues;
                    db.Entry(usersInfo).CurrentValues.SetValues(originalEntityValues);
                    usersInfo.InformationExaminations = listInfo.Where(p => p.PatientStatus_ID == status.ID).ToList();
                    statisticModels.usersStatis = usersInfo;
                    statisticModels.patientStatu = status;
                    statisticModels1.Add(statisticModels);
                }
            }
            return View(statisticModels1);
        }

        public ActionResult SearchBSAndTT(DateTime? dateStart, DateTime? dateEnd)
        {
            var users = db.Users.ToList();
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            foreach (var user in users)
            {
                var listInfo = user.InformationExaminations;
                foreach (var status in statusP)
                {
                    User usersInfo = new User();
                    StatisticModel statisticModels = new StatisticModel();
                    db.Users.Add(usersInfo);
                    usersInfo.AspNetUser = user.AspNetUser;
                    var originalEntityValues = db.Entry(user).CurrentValues;
                    db.Entry(usersInfo).CurrentValues.SetValues(originalEntityValues);
                    if (dateStart.HasValue && dateEnd.HasValue)
                    {
                        TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                        dateEnd = dateEnd + timeEnd;
                        usersInfo.InformationExaminations = listInfo.Where(p => p.PatientStatus_ID == status.ID && p.DateExamine >= dateStart.Value && p.DateEnd <= dateEnd.Value).ToList();
                    }
                    else if (dateStart.HasValue)
                    {
                        usersInfo.InformationExaminations = listInfo.Where(p => p.PatientStatus_ID == status.ID && p.DateExamine >= dateStart.Value).ToList();
                    }
                    else if (dateEnd.HasValue)
                    {
                        TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                        dateEnd = dateEnd + timeEnd;
                        usersInfo.InformationExaminations = listInfo.Where(p => p.PatientStatus_ID == status.ID && p.DateEnd <= dateEnd.Value).ToList();
                    }
                    else
                    {
                        usersInfo.InformationExaminations = listInfo.Where(p => p.PatientStatus_ID == status.ID).ToList();
                    }
                    statisticModels.usersStatis = usersInfo;
                    statisticModels.patientStatu = status;
                    statisticModels1.Add(statisticModels);
                }
            }
            return View("StatisByDoctorAndCondition", statisticModels1);
        }

        public ActionResult StatisByCondition()
        {
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            foreach (var status in statusP)
            {
                StatisticModel statisticModels = new StatisticModel();
                var info = db.InformationExaminations.Where(p => p.PatientStatus_ID == status.ID).ToList();
                statisticModels.countPatient = info.Count;
                statisticModels.patientStatu = status;
                statisticModels1.Add(statisticModels);
            }
            return View(statisticModels1);
        }

        public ActionResult SearchTT(DateTime? dateStart, DateTime? dateEnd)
        {
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            List<InformationExamination> info = new List<InformationExamination>();
            foreach (var status in statusP)
            {
                StatisticModel statisticModels = new StatisticModel();
                if (dateStart.HasValue && dateEnd.HasValue)
                {
                    TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                    dateEnd = dateEnd + timeEnd;
                    info = db.InformationExaminations.Where(p => p.PatientStatus_ID == status.ID && p.DateExamine >= dateStart.Value && p.DateEnd <= dateEnd.Value).ToList();
                }
                else if (dateStart.HasValue)
                {
                    info = db.InformationExaminations.Where(p => p.PatientStatus_ID == status.ID && p.DateExamine >= dateStart.Value).ToList();
                }
                else if (dateEnd.HasValue)
                {
                    TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                    dateEnd = dateEnd + timeEnd;
                    info = db.InformationExaminations.Where(p => p.PatientStatus_ID == status.ID && p.DateEnd <= dateEnd.Value).ToList();
                }
                else
                {
                    info = db.InformationExaminations.Where(p => p.PatientStatus_ID == status.ID).ToList();
                }
                statisticModels.countPatient = info.Count;
                statisticModels.patientStatu = status;
                statisticModels1.Add(statisticModels);
            }
            return View("StatisByCondition", statisticModels1);
        }

        public ActionResult StatisByDiagnostic()
        {
            var Infomation = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID != null).ToList();
            List<DiagnosticsCategory> diagnosticsCategories1 = new List<DiagnosticsCategory>();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            foreach (var informationExamination in Infomation)
            {
                StatisticModel statisticModels = new StatisticModel();
                var getDiagnostic = db.DiagnosticsCategories.FirstOrDefault(p => p.ID == informationExamination.DiagnosticCategory_ID);
                var checkDiagnostic = diagnosticsCategories1.FirstOrDefault(p => p.ID == getDiagnostic.ID);
                if(checkDiagnostic == null)
                {
                    diagnosticsCategories1.Add(getDiagnostic);
                }
            }
            foreach (var item in diagnosticsCategories1)
            {
                StatisticModel statisticModels = new StatisticModel();
                var info = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID == item.ID).ToList();
                statisticModels.countPatient = info.Count;
                statisticModels.diagnosticsCategory = item;
                statisticModels1.Add(statisticModels);
            }
            return View(statisticModels1);
        }

        public ActionResult SearchDiagnostic(DateTime? dateStart, DateTime? dateEnd)
        {
            var Infomation = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID != null).ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            List<DiagnosticsCategory> diagnosticsCategories1 = new List<DiagnosticsCategory>();
            foreach (var informationExamination in Infomation)
            {
                StatisticModel statisticModels = new StatisticModel();
                var getDiagnostic = db.DiagnosticsCategories.FirstOrDefault(p => p.ID == informationExamination.DiagnosticCategory_ID);
                var checkDiagnostic = diagnosticsCategories1.FirstOrDefault(p => p.ID == getDiagnostic.ID);
                if (checkDiagnostic == null)
                {
                    diagnosticsCategories1.Add(getDiagnostic);
                }
            }
            foreach (var item in diagnosticsCategories1)
            {
                StatisticModel statisticModels = new StatisticModel();
                List<InformationExamination> info = new List<InformationExamination>();
                if (dateStart.HasValue && dateEnd.HasValue)
                {
                    TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                    dateEnd = dateEnd + timeEnd;
                    info = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID == item.ID && p.DateExamine >= dateStart.Value && p.DateEnd <= dateEnd.Value).ToList();
                }
                else if (dateStart.HasValue)
                {
                    info = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID == item.ID && p.DateExamine >= dateStart.Value).ToList();
                }
                else if (dateEnd.HasValue)
                {
                    TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                    dateEnd = dateEnd + timeEnd;
                    info = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID == item.ID && p.DateEnd <= dateEnd.Value).ToList();
                }
                else
                {
                    info = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID == item.ID).ToList();
                }
                statisticModels.countPatient = info.Count;
                statisticModels.diagnosticsCategory = item;
                statisticModels1.Add(statisticModels);
            }
            return View("StatisByDiagnostic", statisticModels1);
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

        public async Task<ActionResult> StatisByMoney()
        {
            StatisticModel statisticModels = new StatisticModel();
            int priceInfo = 0;
            int pricePres = 0;
            int priceTotalTest = 0;
            var priceExam = db.InformationExaminations.ToList();
            foreach (var item in priceExam)
            {
                if (item.PriceExamination != null)
                {
                    priceInfo += (int)item.PriceExamination;
                }
                var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == item.ID).ToList();
                foreach (var price in listPrescription)
                {
                    if (price.TotalPrice != null)
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
                foreach (var price in result)
                {
                    priceTotalTest += price;
                }
            }
            statisticModels.priceExamination = priceInfo;
            statisticModels.pricePrescription = pricePres;
            statisticModels.priceSubclinical = priceTotalTest;
            return View(statisticModels);
        }

        public async Task<ActionResult> SearchMoney(DateTime? dateStart, DateTime? dateEnd)
        {
            StatisticModel statisticModels = new StatisticModel();
            int priceInfo = 0;
            int pricePres = 0;
            int priceTotalTest = 0;
            List<InformationExamination> informationExaminations = new List<InformationExamination>();
            if (dateStart.HasValue && dateEnd.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                dateEnd = dateEnd + timeEnd;
                informationExaminations = db.InformationExaminations.Where(p => p.DateExamine >= dateStart.Value && p.DateEnd <= dateEnd.Value).ToList();
            }
            else if (dateStart.HasValue)
            {
                informationExaminations = db.InformationExaminations.Where(p => p.DateExamine >= dateStart.Value).ToList();
            }
            else if (dateEnd.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                dateEnd = dateEnd + timeEnd;
                informationExaminations = db.InformationExaminations.Where(p => p.DateEnd <= dateEnd.Value).ToList();
            }
            else
            {
                informationExaminations = db.InformationExaminations.ToList();
            }

            foreach (var item in informationExaminations)
            {
                if (item.PriceExamination != null)
                {
                    priceInfo += (int)item.PriceExamination;
                }
                var listPrescription = db.Prescription_Detail.Where(p => p.InformationExamination_ID == item.ID).ToList();
                foreach (var price in listPrescription)
                {
                    if (price.TotalPrice != null)
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
                foreach (var price in result)
                {
                    priceTotalTest += price;
                }
            }
            statisticModels.priceExamination = priceInfo;
            statisticModels.pricePrescription = pricePres;
            statisticModels.priceSubclinical = priceTotalTest;
            return View("StatisByMoney",statisticModels);
        }
    }
}