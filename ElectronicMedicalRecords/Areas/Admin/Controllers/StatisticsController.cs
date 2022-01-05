using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Giám Đốc,QTV,Thu Ngân")]
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
            ViewBag.DateStart = Session["DateStartTT"];
            ViewBag.DateEnd = Session["DateEndTT"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisByDocandCon()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["DoctorAndCondition"];
            ViewBag.DateStart = Session["DateStartBSAndTT"];
            ViewBag.DateEnd = Session["DateEndBSAndTT"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisticDiagnostic()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["Diagnostic"];
            ViewBag.DateStart = Session["DateStartDiagnostic"];
            ViewBag.DateEnd = Session["DateEndDiagnostic"];
            return View(statisticModels);
        }

        public ActionResult PrintStatisticMoney()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            statisticModels = (List<StatisticModel>)Session["Money"];
            ViewBag.DateStart = Session["DateStartMoney"];
            ViewBag.DateEnd = Session["DateEndMoney"];
            return View(statisticModels);
        }

        public ActionResult SearchBSAndTT(DateTime? dateStart, DateTime? dateEnd)
        {
            var users = db.Users.ToList();
            var statusP = db.PatientStatus.ToList();
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            Session["DateStartBSAndTT"] = dateStart;
            Session["DateEndBSAndTT"] = dateEnd;
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
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
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
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
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
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

        public async Task<ActionResult> StatisByMoney()
        {
            List<StatisticModel> statisticModels1 = new List<StatisticModel>();
            int priceInfo = 0;
            int pricePres = 0;
            int priceTotalTest = 0;
            var priceExam = db.InformationExaminations.ToList();
            foreach (var item in priceExam)
            {
                StatisticModel statisticModel = new StatisticModel();
                if (item.PriceExamination != null)
                {
                    var patient = db.Patients.FirstOrDefault(p => p.ID == item.Patient_ID);
                    priceInfo += (int)item.PriceExamination;
                    if (item.PricePrescription != null)
                    {
                        pricePres += (int)item.PricePrescription;
                    }
                    if (item.PriceTest != null)
                    {
                        priceTotalTest = (int)item.PriceTest;
                    }
                    statisticModel.patient = patient;
                    statisticModel.informationExamination = item;
                    statisticModels1.Add(statisticModel);
                }
            }
            Session["Money"] = statisticModels1;
            return View(statisticModels1);
        }

        public async Task<ActionResult> SearchMoney(DateTime? dateStart, DateTime? dateEnd)
        {
            List<StatisticModel> statisticModels = new List<StatisticModel>();
            int priceInfo = 0;
            int pricePres = 0;
            int priceTotalTest = 0;
            List<InformationExamination> informationExaminations = new List<InformationExamination>();
            Session["DateStartMoney"] = dateStart;
            Session["DateEndMoney"] = dateEnd;
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
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
                    StatisticModel statisticModel = new StatisticModel();
                    var patient = db.Patients.FirstOrDefault(p => p.ID == item.Patient_ID);
                    priceInfo += (int)item.PriceExamination;
                    if (item.PricePrescription != null)
                    {
                        pricePres += (int)item.PricePrescription;
                    }
                    if (item.PriceTest != null)
                    {
                        priceTotalTest = (int)item.PriceTest;
                    }
                    statisticModel.patient = patient;
                    statisticModel.informationExamination = item;
                    statisticModels.Add(statisticModel);
                }
            }
            Session["Money"] = statisticModels;
            return View("StatisByMoney",statisticModels);
        }

        public ActionResult ExportExcelCondition()
        {
            List<StatisticModel> statisticModels = (List<StatisticModel>)Session["Condition"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            var totalCount = 0;
            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Times New Roman";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                totalCount += item.countPatient;
                workSheet.Cells[10 + i, 1].Value = i + 1;
                workSheet.Cells[10 + i, 2].Value = item.patientStatu.ID;
                workSheet.Cells[10 + i, 3].Value = item.patientStatu.Name;
                workSheet.Cells[10 + i, 5].Value = item.countPatient;
                workSheet.Cells[10 + i, 3, 10 + i, 4].Merge = true;
                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 16;
                workSheet.Column(3).Width = 20;
                workSheet.Column(4).Width = 23;
                workSheet.Column(5).Width = 25;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Row(10 + i).Height = 35;
            }

            workSheet.Cells[statisticModels.Count + 10, 1].Value ="Tổng cộng: ";
            workSheet.Cells[statisticModels.Count + 10, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 4].Merge = true;
            workSheet.Row(statisticModels.Count + 10).Height = 25;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[statisticModels.Count + 10, 5].Value = totalCount;

            workSheet.Cells[statisticModels.Count + 12, 4].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Merge = true;

            workSheet.Cells[statisticModels.Count + 13, 4].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 13, 4, statisticModels.Count + 13, 5].Merge = true;

            Image img = Image.FromFile(Server.MapPath("~/Areas/Admin/assets/images/logo/VLang.png"));
            var cfBm = new Bitmap(img, new Size(250, 85));
            var excelImage = workSheet.Drawings.AddPicture("My Logo", cfBm);
            excelImage.SetPosition(1, 0, 0, 0);

            workSheet.Cells["D2"].Value = "PHÒNG KHÁM VĂN LANG";
            workSheet.Cells["D2:E2"].Merge = true;
            workSheet.Cells["D2:E2"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Value = "Địa chỉ: Hẻm 69 Đặng Thùy Trâm," +
                "Phường 13, Bình Thạnh, TP HCM";
            workSheet.Cells["D3:E4"].Merge = true;

            workSheet.Cells["A6"].Value = "THỐNG KÊ THEO TÌNH TRẠNG";
            workSheet.Cells["A6:E6"].Merge = true;
            workSheet.Cells["A6:E6"].Style.Font.Bold = true;

            if(Session["DateStartTT"] != null && Session["DateEndTT"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartTT"];
                workSheet.Cells["A7"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if(Session["DateStartTT"] == null && Session["DateEndTT"] != null) {
                DateTime dateEnd = (DateTime)Session["DateEndTT"];
                workSheet.Cells["A7"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if(Session["DateStartTT"] != null && Session["DateEndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndTT"];
                DateTime dateStart = (DateTime)Session["DateStartTT"];
                workSheet.Cells["A7"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A7"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A7:E7"].Merge = true;

            workSheet.PrinterSettings.RepeatRows = new ExcelAddress("1:9");

            workSheet.Cells["A9"].Value = "#";
            workSheet.Cells["B9"].Value = "Mã Tình Trạng";
            workSheet.Cells["C9"].Value = "Tên Tình Trạng";
            workSheet.Cells["C9:D9"].Merge = true;
            workSheet.Cells["E9"].Value = "SLBN";
            workSheet.Cells["A9:E9"].Style.Font.Bold = true;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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
            var totalCount = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Times New Roman";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                totalCount += item.countPatient;
                workSheet.Cells[10 + i, 1].Value = i + 1;
                workSheet.Cells[10 + i, 2].Value = item.diagnosticsCategory.Code;
                workSheet.Cells[10 + i, 3].Value = item.diagnosticsCategory.Name;
                workSheet.Cells[10 + i, 5].Value = item.countPatient;
                workSheet.Cells[10 + i, 3, 10 + i, 4].Merge = true;
                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 16;
                workSheet.Column(3).Width = 20;
                workSheet.Column(4).Width = 23;
                workSheet.Column(5).Width = 25;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Row(10 + i).Height = 35;
            }

            workSheet.Cells[statisticModels.Count + 10, 1].Value = "Tổng cộng: ";
            workSheet.Cells[statisticModels.Count + 10, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 4].Merge = true;
            workSheet.Row(statisticModels.Count + 10).Height = 25;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[statisticModels.Count + 10, 5].Value = totalCount;

            workSheet.Cells[statisticModels.Count + 12, 4].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Merge = true;

            workSheet.Cells[statisticModels.Count + 13, 4].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 13, 4, statisticModels.Count + 13, 5].Merge = true;

            Image img = Image.FromFile(Server.MapPath("~/Areas/Admin/assets/images/logo/VLang.png"));
            var cfBm = new Bitmap(img, new Size(250, 85));
            var excelImage = workSheet.Drawings.AddPicture("My Logo", cfBm);
            excelImage.SetPosition(1, 0, 0, 0);

            workSheet.Cells["D2"].Value = "PHÒNG KHÁM VĂN LANG";
            workSheet.Cells["D2:E2"].Merge = true;
            workSheet.Cells["D2:E2"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Value = "Địa chỉ: Hẻm 69 Đặng Thùy Trâm," +
                "Phường 13, Bình Thạnh, TP HCM";
            workSheet.Cells["D3:E4"].Merge = true;

            workSheet.Cells["A6"].Value = "THỐNG KÊ THEO NHÓM BỆNH";
            workSheet.Cells["A6:E6"].Merge = true;
            workSheet.Cells["A6:E6"].Style.Font.Bold = true;

            if (Session["DateStartDiagnostic"] != null && Session["DateEndDiagnostic"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartDiagnostic"];
                workSheet.Cells["A7"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartDiagnostic"] == null && Session["DateEndDiagnostic"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndDiagnostic"];
                workSheet.Cells["A7"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartDiagnostic"] != null && Session["DateEndDiagnostic"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndDiagnostic"];
                DateTime dateStart = (DateTime)Session["DateStartDiagnostic"];
                workSheet.Cells["A7"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A7"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A7:E7"].Merge = true;

            workSheet.PrinterSettings.RepeatRows = new ExcelAddress("1:9");

            workSheet.Cells["A9"].Value = "#";
            workSheet.Cells["B9"].Value = "Mã Chẩn Đoán";
            workSheet.Cells["C9"].Value = "Tên Chẩn Đoán";
            workSheet.Cells["C9:D9"].Merge = true;
            workSheet.Cells["E9"].Value = "SLBN";
            workSheet.Cells["A9:E9"].Style.Font.Bold = true;

            workSheet.Cells[9, 1, 9, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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
            var totalExam = 0;
            var totalPres = 0;
            var totalTest = 0;
            List<StatisticModel> statisticModels = (List<StatisticModel>)Session["Money"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Times New Roman";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                item.informationExamination.PriceExamination = item.informationExamination.PriceExamination != null ? item.informationExamination.PriceExamination : 0;
                item.informationExamination.PricePrescription = item.informationExamination.PricePrescription != null ? item.informationExamination.PricePrescription : 0;
                item.informationExamination.PriceTest = item.informationExamination.PriceTest != null ? item.informationExamination.PriceTest : 0;
                totalExam += (int)item.informationExamination.PriceExamination;
                totalPres += (int)item.informationExamination.PricePrescription;
                totalTest += (int)item.informationExamination.PriceTest;
                workSheet.Cells[10 + i, 1].Value = i + 1;
                workSheet.Cells[10 + i, 2].Value = item.patient.MaBN;
                workSheet.Cells[10 + i, 3].Value = item.patient.Name;
                workSheet.Cells[10 + i, 4].Value = item.informationExamination.PriceExamination.Value.ToString("N0");
                workSheet.Cells[10 + i, 5].Value = item.informationExamination.PricePrescription.Value.ToString("N0");
                workSheet.Cells[10 + i, 6].Value = item.informationExamination.PriceTest.Value.ToString("N0");
                workSheet.Cells[10 + i, 7].Value = (item.informationExamination.PriceExamination + item.informationExamination.PricePrescription + item.informationExamination.PriceTest).Value.ToString("N0");
                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 18;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 12;
                workSheet.Column(6).Width = 13;
                workSheet.Column(7).Width = 14;
                workSheet.Cells[10 + i, 1, 10 + i, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Row(10 + i).Height = 35;
            }

            workSheet.Cells[statisticModels.Count + 10, 1].Value = "Tổng cộng: ";
            workSheet.Cells[statisticModels.Count + 10, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 3].Merge = true;
            workSheet.Row(statisticModels.Count + 10).Height = 25;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[statisticModels.Count + 10, 4].Value = totalExam.ToString("N0");
            workSheet.Cells[statisticModels.Count + 10, 5].Value = totalPres.ToString("N0");
            workSheet.Cells[statisticModels.Count + 10, 6].Value = totalTest.ToString("N0");
            workSheet.Cells[statisticModels.Count + 10, 7].Value = (totalExam + totalPres + totalTest).ToString("N0");

            workSheet.Cells[statisticModels.Count + 12, 4].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 7].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 7].Merge = true;

            workSheet.Cells[statisticModels.Count + 13, 4].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 13, 4, statisticModels.Count + 13, 7].Merge = true;

            Image img = Image.FromFile(Server.MapPath("~/Areas/Admin/assets/images/logo/VLang.png"));
            var cfBm = new Bitmap(img, new Size(250, 85));
            var excelImage = workSheet.Drawings.AddPicture("My Logo", cfBm);
            excelImage.SetPosition(1, 0, 0, 0);

            workSheet.Cells["E2"].Value = "PHÒNG KHÁM VĂN LANG";
            workSheet.Cells["E2:G2"].Merge = true;
            workSheet.Cells["E2:G2"].Style.Font.Bold = true;
            workSheet.Cells["E3"].Value = "Địa chỉ: Hẻm 69 Đặng Thùy Trâm, " +
                "Phường 13, Bình Thạnh, TP HCM";
            workSheet.Cells["E3:G4"].Merge = true;

            workSheet.Cells["A6"].Value = "THỐNG KÊ DOANH SỐ";
            workSheet.Cells["A6:G6"].Merge = true;
            workSheet.Cells["A6:G6"].Style.Font.Bold = true;

            if (Session["DateStartMoney"] != null && Session["DateEndMoney"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartMoney"];
                workSheet.Cells["A6"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartMoney"] == null && Session["DateEndMoney"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndMoney"];
                workSheet.Cells["A6"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartMoney"] != null && Session["DateEndMoney"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndMoney"];
                DateTime dateStart = (DateTime)Session["DateStartMoney"];
                workSheet.Cells["A6"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A6"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A6:G6"].Merge = true;

            workSheet.PrinterSettings.RepeatRows = new ExcelAddress("1:9");

            workSheet.Cells["A9"].Value = "#";
            workSheet.Cells["B9"].Value = "Mã BN";
            workSheet.Cells["C9"].Value = "Tên BN";
            workSheet.Cells["D9"].Value = "Khám";
            workSheet.Cells["E9"].Value = "Thuốc";
            workSheet.Cells["F9"].Value = "Xét Nghiệm";
            workSheet.Cells["G9"].Value = "Thành Tiền";
            workSheet.Cells["A9:G9"].Style.Font.Bold = true;
            workSheet.Cells[9, 1, 9, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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
            var totalCount = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.DefaultColWidth = 25;
            workSheet.DefaultRowHeight = 25;
            workSheet.Cells.Style.Font.Name = "Times New Roman";
            workSheet.Cells.Style.Font.Size = 12;
            workSheet.Cells.Style.WrapText = true;
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < statisticModels.Count; i++)
            {
                var item = statisticModels[i];
                totalCount += item.usersStatis.InformationExaminations.Count;
                var charName = item.usersStatis.AspNetUser.Email.IndexOf(".", 0);
                var charName1 = item.usersStatis.AspNetUser.Email.IndexOf("@", 0);
                workSheet.Cells[10 + i, 1].Value = i + 1;
                workSheet.Cells[10 + i, 2].Value = item.usersStatis.AspNetUser.Email.Substring(charName + 1, charName1 - charName - 1);
                workSheet.Cells[10 + i, 3].Value = item.usersStatis.Name;
                workSheet.Cells[10 + i, 4].Value = item.patientStatu.Name;
                workSheet.Cells[10 + i, 5].Value = item.usersStatis.InformationExaminations.Count;
                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 25;
                workSheet.Column(4).Width = 19;
                workSheet.Column(5).Width = 25;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[10 + i, 1, 10 + i, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Row(10 + i).Height = 35;
            }

            workSheet.Cells[statisticModels.Count + 10, 1].Value = "Tổng cộng: ";
            workSheet.Cells[statisticModels.Count + 10, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 4].Merge = true;
            workSheet.Row(statisticModels.Count + 10).Height = 25;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[statisticModels.Count + 10, 1, statisticModels.Count + 10, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[statisticModels.Count + 10, 5].Value = totalCount;

            workSheet.Cells[statisticModels.Count + 12, 4].Value = "Thành Phố Hồ Chí Minh, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Style.Font.Size = 10;
            workSheet.Cells[statisticModels.Count + 12, 4, statisticModels.Count + 12, 5].Merge = true;

            workSheet.Cells[statisticModels.Count + 13, 4].Value = "Thư Ký";
            workSheet.Cells[statisticModels.Count + 13, 4, statisticModels.Count + 13, 5].Merge = true;

            Image img = Image.FromFile(Server.MapPath("~/Areas/Admin/assets/images/logo/VLang.png"));
            var cfBm = new Bitmap(img, new Size(250, 85));
            var excelImage = workSheet.Drawings.AddPicture("My Logo", cfBm);
            excelImage.SetPosition(1, 0, 0, 0);

            workSheet.Cells["D2"].Value = "PHÒNG KHÁM VĂN LANG";
            workSheet.Cells["D2:E2"].Merge = true;
            workSheet.Cells["D2:E2"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Value = "Địa chỉ: Hẻm 69 Đặng Thùy Trâm," +
                "Phường 13, Bình Thạnh, TP HCM";
            workSheet.Cells["D3:E4"].Merge = true;

            workSheet.Cells["A6"].Value = "THỐNG KÊ THEO BÁC SĨ & TÌNH TRẠNG";
            workSheet.Cells["A6:E6"].Merge = true;
            workSheet.Cells["A6:E6"].Style.Font.Bold = true;

            if (Session["DateStartBSAndTT"] != null && Session["DateEndBSAndTT"] == null)
            {
                DateTime dateStart = (DateTime)Session["DateStartBSAndTT"];
                workSheet.Cells["A7"].Value = "Từ Ngày: " + dateStart.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartBSAndTT"] == null && Session["DateEndBSAndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndBSAndTT"];
                workSheet.Cells["A7"].Value = "Đến Ngày: " + dateEnd.ToString("dd-MM-yyyy");
            }
            else if (Session["DateStartBSAndTT"] != null && Session["DateEndBSAndTT"] != null)
            {
                DateTime dateEnd = (DateTime)Session["DateEndBSAndTT"];
                DateTime dateStart = (DateTime)Session["DateStartBSAndTT"];
                workSheet.Cells["A7"].Value = "Ngày: " + dateStart.ToString("dd-MM-yyyy") + " - " + dateEnd.ToString("dd-MM-yyyy");
            }
            else
            {
                workSheet.Cells["A7"].Value = "Đến Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
            }
            workSheet.Cells["A7:E7"].Merge = true;

            workSheet.PrinterSettings.RepeatRows = new ExcelAddress("1:9");

            workSheet.Cells["A9"].Value = "#";
            workSheet.Cells["B9"].Value = "Mã Bác Sĩ";
            workSheet.Cells["C9"].Value = "Bác Sĩ";
            workSheet.Cells["D9"].Value = "Tình Trạng";
            workSheet.Cells["E9"].Value = "SLBN";
            workSheet.Cells["A9:E9"].Style.Font.Bold = true;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[9, 1, 9, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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

        public ActionResult BillExam()
        {
            var userID = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(p => p.UserID == userID);
            var listInfoByUser = db.Bills.Where(p => p.UserPayment_ID == userID && p.TypePayment == "Khám").ToList();
            return View(listInfoByUser);
        }

        public ActionResult BillPre()
        {
            var userID = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(p => p.UserID == userID);
            var listInfoByUser = db.Bills.Where(p => p.UserPayment_ID == userID && p.TypePayment == "Thuốc").ToList();
            return View(listInfoByUser);
        }

        public ActionResult BillTest()
        {
            var userID = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(p => p.UserID == userID);
            var listInfoByUser = db.Bills.Where(p => p.UserPayment_ID == userID && p.TypePayment == "Xét Nghiệm").ToList();
            return View(listInfoByUser);
        }
    }
}