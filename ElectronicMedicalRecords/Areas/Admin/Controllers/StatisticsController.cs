using ElectronicMedicalRecords.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
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
                Session["DoctorAndCondition"] = statisticModels1;
            }
            return View(statisticModels1);
        }
        public ActionResult PrintStatisByConditionPost()
        {
            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintStatisticByCondition()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["Condition"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisByDocandCon()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["DoctorAndCondition"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisticDiagnostic()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["Diagnostic"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisticPrice()
        {
            db.Configuration.LazyLoadingEnabled = false;
            StatisticModel statisticModels = new StatisticModel();
            statisticModels = (StatisticModel)Session["Money"];
            return View(statisticModels);
        }

        public ActionResult SearchBSAndTT(DateTime? dateStart, DateTime? dateEnd)
        {
            var users = db.Users.ToList();
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            Session["DateStartBSAndTT"] = dateStart;
            Session["DateEndBSAndTT"] = dateEnd;
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
                    Session["DoctorAndCondition"] = statisticModels1;
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
                Session["Condition"] = statisticModels1;
            }
            return View(statisticModels1);
        }

        public ActionResult SearchTT(DateTime? dateStart, DateTime? dateEnd)
        {
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            List<InformationExamination> info = new List<InformationExamination>();
            Session["DateStartTT"] = dateStart;
            Session["DateEndTT"] = dateEnd;
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
                Session["Condition"] = statisticModels1;
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
            Session["Diagnostic"] = statisticModels1;
            return View(statisticModels1);
        }

        public ActionResult SearchDiagnostic(DateTime? dateStart, DateTime? dateEnd)
        {
            var Infomation = db.InformationExaminations.Where(p => p.DiagnosticCategory_ID != null).ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            List<DiagnosticsCategory> diagnosticsCategories1 = new List<DiagnosticsCategory>();
            Session["DateStartDiagnostic"] = dateStart;
            Session["DateEndDiagnostic"] = dateEnd;
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
            Session["Diagnostic"] = statisticModels1;
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
            statisticModels.priceExamination = priceInfo.ToString("N0");
            statisticModels.pricePrescription = pricePres.ToString("N0");
            statisticModels.priceSubclinical = priceTotalTest.ToString("N0");
            Session["Money"] = statisticModels;
            return View(statisticModels);
        }

        public async Task<ActionResult> SearchMoney(DateTime? dateStart, DateTime? dateEnd)
        {
            StatisticModel statisticModels = new StatisticModel();
            int priceInfo = 0;
            int pricePres = 0;
            int priceTotalTest = 0;
            List<InformationExamination> informationExaminations = new List<InformationExamination>();
            Session["DateStartMoney"] = dateStart;
            Session["DateEndMoney"] = dateEnd;
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
            statisticModels.priceExamination = priceInfo.ToString("N0");
            statisticModels.pricePrescription = pricePres.ToString("N0");
            statisticModels.priceSubclinical = priceTotalTest.ToString("N0");
            Session["Money"] = statisticModels;
            return View("StatisByMoney",statisticModels);
        }

        public ActionResult ExportExcelCondition()
        {
            List<StatisticModel> statisticModels = (List<StatisticModel>)Session["Condition"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Arial";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                workSheet.Cells[8 + i, 1].Value = item.patientStatu.ID;
                workSheet.Cells[8 + i, 2].Value = item.patientStatu.Name;
                workSheet.Cells[8 + i, 4].Value = item.countPatient;
                workSheet.Cells[8 + i, 2, 8 + i, 3].Merge = true;
                workSheet.Column(2).Width = 16;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 30;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            }

            workSheet.Cells[statisticModels.Count + 9, 3].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Merge = true;

            workSheet.Cells[statisticModels.Count + 10, 3].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 10, 3, statisticModels.Count + 10, 4].Merge = true;

            workSheet.Cells["A1"].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            workSheet.Cells["A1:B1"].Merge = true;
            workSheet.Cells["A2"].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            workSheet.Cells["A2:B2"].Merge = true;
            workSheet.Cells["A1:A2"].Style.Font.Bold = true;

            workSheet.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            workSheet.Cells["C1:D1"].Merge = true;
            workSheet.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
            workSheet.Cells["C2:D2"].Merge = true;
            workSheet.Cells["C1:C2"].Style.Font.Bold = true;

            workSheet.Cells["A4"].Value = "THỐNG KÊ THEO TÌNH TRẠNG";
            workSheet.Cells["A4:D4"].Merge = true;
            workSheet.Cells["A4:B4:C4:D4"].Style.Font.Bold = true;

            if(Session["DateStartTT"] != null && Session["DateEndTT"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartTT"];
                workSheet.Cells["A5"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if(Session["DateStartTT"] == null && Session["DateEndTT"] != null) {
                DateTime dateEnd = (DateTime)Session["DateEndTT"];
                workSheet.Cells["A5"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if(Session["DateStartTT"] != null && Session["DateEndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndTT"];
                DateTime dateStart = (DateTime)Session["DateStartTT"];
                workSheet.Cells["A5"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A5"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A5:D5"].Merge = true;

            workSheet.Cells["A7"].Value = "Mã Tình Trạng";
            workSheet.Cells["B7"].Value = "Tên Tình Trạng";
            workSheet.Cells["B7:C7"].Merge = true;
            workSheet.Cells["D7"].Value = "Số Lượng Bệnh Nhân";
            workSheet.Cells["A7:B7:D7"].Style.Font.Bold = true;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as StatisticCondition.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=StatisticCondition.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }

        public ActionResult ExportExcelDiagnostic()
        {
            List<StatisticModel> statisticModels = (List<StatisticModel>)Session["Diagnostic"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Arial";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                workSheet.Cells[8 + i, 1].Value = item.diagnosticsCategory.Code;
                workSheet.Cells[8 + i, 2].Value = item.diagnosticsCategory.Name;
                workSheet.Cells[8 + i, 4].Value = item.countPatient;
                workSheet.Cells[8 + i, 2, 8 + i, 3].Merge = true;
                workSheet.Column(2).Width = 16;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 30;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            }

            workSheet.Cells[statisticModels.Count + 9, 3].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Merge = true;

            workSheet.Cells[statisticModels.Count + 10, 3].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 10, 3, statisticModels.Count + 10, 4].Merge = true;

            workSheet.Cells["A1"].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            workSheet.Cells["A1:B1"].Merge = true;
            workSheet.Cells["A2"].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            workSheet.Cells["A2:B2"].Merge = true;
            workSheet.Cells["A1:A2"].Style.Font.Bold = true;

            workSheet.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            workSheet.Cells["C1:D1"].Merge = true;
            workSheet.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
            workSheet.Cells["C2:D2"].Merge = true;
            workSheet.Cells["C1:C2"].Style.Font.Bold = true;

            workSheet.Cells["A4"].Value = "THỐNG KÊ THEO NHÓM BỆNH";
            workSheet.Cells["A4:D4"].Merge = true;
            workSheet.Cells["A4:B4:C4:D4"].Style.Font.Bold = true;

            if (Session["DateStartDiagnostic"] != null && Session["DateEndDiagnostic"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartDiagnostic"];
                workSheet.Cells["A5"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartDiagnostic"] == null && Session["DateEndDiagnostic"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndDiagnostic"];
                workSheet.Cells["A5"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartDiagnostic"] != null && Session["DateEndDiagnostic"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndDiagnostic"];
                DateTime dateStart = (DateTime)Session["DateStartDiagnostic"];
                workSheet.Cells["A5"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A5"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A5:D5"].Merge = true;

            workSheet.Cells["A7"].Value = "Mã Chẩn Đoán";
            workSheet.Cells["B7"].Value = "Tên Chẩn Đoán";
            workSheet.Cells["B7:C7"].Merge = true;
            workSheet.Cells["D7"].Value = "Số Lượng Bệnh Nhân";
            workSheet.Cells["A7:B7:D7"].Style.Font.Bold = true;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as StatisticCondition.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=StatisticDiagnostic.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }

        public ActionResult ExportExcelMoney()
        {
            StatisticModel statisticModels = (StatisticModel)Session["Money"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Arial";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[8, 1].Value = statisticModels.priceExamination;
            workSheet.Cells[8, 2].Value = statisticModels.pricePrescription;
            workSheet.Cells[8, 4].Value = statisticModels.priceSubclinical;
            workSheet.Cells[8, 2, 8, 3].Merge = true;
            workSheet.Column(2).Width = 16;
            workSheet.Column(3).Width = 16;
            workSheet.Column(4).Width = 30;
            workSheet.Cells[8, 1, 8, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[8, 1, 8, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[8, 1, 8, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[8, 1, 8, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[10, 3].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[10, 3, 10, 4].Style.Font.Size = 10;
            workSheet.Cells[10, 3, 10, 4].Merge = true;

            workSheet.Cells[11, 3].Value = "Thư Ký";
            workSheet.Cells[11, 3, 11, 4].Merge = true;

            workSheet.Cells["A1"].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            workSheet.Cells["A1:B1"].Merge = true;
            workSheet.Cells["A2"].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            workSheet.Cells["A2:B2"].Merge = true;
            workSheet.Cells["A1:A2"].Style.Font.Bold = true;

            workSheet.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            workSheet.Cells["C1:D1"].Merge = true;
            workSheet.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
            workSheet.Cells["C2:D2"].Merge = true;
            workSheet.Cells["C1:C2"].Style.Font.Bold = true;

            workSheet.Cells["A4"].Value = "THỐNG KÊ DOANH SỐ";
            workSheet.Cells["A4:D4"].Merge = true;
            workSheet.Cells["A4:B4:C4:D4"].Style.Font.Bold = true;

            if (Session["DateStartMoney"] != null && Session["DateEndMoney"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartMoney"];
                workSheet.Cells["A5"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartMoney"] == null && Session["DateEndMoney"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndMoney"];
                workSheet.Cells["A5"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartMoney"] != null && Session["DateEndMoney"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndMoney"];
                DateTime dateStart = (DateTime)Session["DateStartMoney"];
                workSheet.Cells["A5"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A5"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A5:D5"].Merge = true;

            workSheet.Cells["A7"].Value = "Khám";
            workSheet.Cells["B7"].Value = "Thuốc";
            workSheet.Cells["B7:C7"].Merge = true;
            workSheet.Cells["D7"].Value = "Xét Nghiệm";
            workSheet.Cells["A7:B7:D7"].Style.Font.Bold = true;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as StatisticCondition.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=StatisticMoney.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }

        public ActionResult ExportExcelBSAndTT()
        {
            List<StatisticModel> statisticModels = (List<StatisticModel>)Session["DoctorAndCondition"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Arial";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                var charName = item.usersStatis.AspNetUser.Email.IndexOf(".", 0);
                var charName1 = item.usersStatis.AspNetUser.Email.IndexOf("@", 0);
                workSheet.Cells[8 + i, 1].Value = item.usersStatis.AspNetUser.Email.Substring(charName + 1, charName1 - charName - 1);
                workSheet.Cells[8 + i, 2].Value = item.usersStatis.Name;
                workSheet.Cells[8 + i, 3].Value = item.patientStatu.Name;
                workSheet.Cells[8 + i, 4].Value = item.usersStatis.InformationExaminations.Count;
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 25;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 30;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[8 + i, 1, 8 + i, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Row(8 + i).Height = 35;
            }

            workSheet.Cells[statisticModels.Count + 9, 3].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 9, 3, statisticModels.Count + 9, 4].Merge = true;

            workSheet.Cells[statisticModels.Count + 10, 3].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 10, 3, statisticModels.Count + 10, 4].Merge = true;

            workSheet.Cells["A1"].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            workSheet.Cells["A1:B1"].Merge = true;
            workSheet.Cells["A2"].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            workSheet.Cells["A2:B2"].Merge = true;
            workSheet.Cells["A1:A2"].Style.Font.Bold = true;

            workSheet.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            workSheet.Cells["C1:D1"].Merge = true;
            workSheet.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
            workSheet.Cells["C2:D2"].Merge = true;
            workSheet.Cells["C1:C2"].Style.Font.Bold = true;

            workSheet.Cells["A4"].Value = "THỐNG KÊ THEO BÁC SĨ & TÌNH TRẠNG";
            workSheet.Cells["A4:D4"].Merge = true;
            workSheet.Cells["A4:D4"].Style.Font.Bold = true;

            if (Session["DateStartBSAndTT"] != null && Session["DateEndBSAndTT"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartBSAndTT"];
                workSheet.Cells["A5"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartBSAndTT"] == null && Session["DateEndBSAndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndBSAndTT"];
                workSheet.Cells["A5"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartBSAndTT"] != null && Session["DateEndBSAndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndBSAndTT"];
                DateTime dateStart = (DateTime)Session["DateStartBSAndTT"];
                workSheet.Cells["A5"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A5"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A5:D5"].Merge = true;

            workSheet.Cells["A7"].Value = "Mã Bác Sĩ";
            workSheet.Cells["B7"].Value = "Bác Sĩ";
            workSheet.Cells["C7"].Value = "Tình Trạng";
            workSheet.Cells["D7"].Value = "Số Lượng Bệnh Nhân";
            workSheet.Cells["A7:B7:C7:D7"].Style.Font.Bold = true;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[7, 1, 7, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as StatisticCondition.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=StatisticBSAndTT.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }
    }
}