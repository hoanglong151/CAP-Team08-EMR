using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng,Thu Ngân")]
    public class InformationExaminationsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        MultiplesModel multiplesModel = new MultiplesModel();
        // GET: Admin/InformationExaminations
        public ActionResult Index()
        {
            var informationExaminations = db.InformationExaminations.Include(i => i.Patient).Include(i => i.PatientStatu).Include(i => i.User);
            return View(informationExaminations.ToList());
        }

        public async Task<ActionResult> GetNotification()
        {
            NotificationComponentKTV NCNoti = new NotificationComponentKTV();
            var listNotify = NCNoti.GetInformationExamination();
            var countresult = 0;
            List<object> patient = new List<object>();
            foreach(var item in listNotify)
            {
                var detail_TuanHoan = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_HoHap = db.Detail_HoHap.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_TieuHoa = db.Detail_TieuHoa.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_ThanTietNieu = db.Detail_ThanTietNieu.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_CoXuongKhop = db.Detail_CoXuongKhop.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_ThanKinh = db.Detail_ThanKinh.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_TamThan = db.Detail_TamThan.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_Mat = db.Detail_Mat.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_TaiMuiHong = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_RangHamMat = db.Detail_RangHamMat.Where(p => p.InformationExamination_ID == item.ID).ToList();
                var detail_DaLieu = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == item.ID).ToList();

                var checkDangerousTuanHoan = detail_TuanHoan.Any(p => p.TuanHoan.Dangerous == true);
                var checkDangerousHoHap = detail_HoHap.Any(p => p.HoHap.Dangerous == true);
                var checkDangerousTieuHoa = detail_TieuHoa.Any(p => p.TieuHoa.Dangerous == true);
                var checkDangerousThanTietNieu = detail_ThanTietNieu.Any(p => p.ThanTietNieu.Dangerous == true);
                var checkDangerousCoXuongKhop = detail_CoXuongKhop.Any(p => p.CoXuongKhop.Dangerous == true);
                var checkDangerousThanKinh = detail_ThanKinh.Any(p => p.ThanKinh.Dangerous == true);
                var checkDangerousTamThan = detail_TamThan.Any(p => p.TamThan.Dangerous == true);
                var checkDangerousNgoaiKhoa = detail_NgoaiKhoa.Any(p => p.NgoaiKhoa.Dangerous == true);
                var checkDangerousSanPhuKhoa = detail_SanPhuKhoa.Any(p => p.SanPhuKhoa.Dangerous == true);
                var checkDangerousMat = detail_Mat.Any(p => p.Mat.Dangerous == true);
                var checkDangerousTai = detail_TaiMuiHong.Any(p => p.TaiMuiHong.Dangerous == true);
                var checkDangerousRangHamMat = detail_RangHamMat.Any(p => p.RangHamMat.Dangerous == true);
                var checkDangerousDaLieu = detail_DaLieu.Any(p => p.DaLieu.Dangerous == true);

                var checkResult = false;
                if(checkDangerousTuanHoan == true || checkDangerousHoHap == true || checkDangerousTieuHoa == true || checkDangerousThanTietNieu == true
                    || checkDangerousCoXuongKhop == true || checkDangerousThanKinh == true || checkDangerousTamThan == true || checkDangerousNgoaiKhoa == true
                    || checkDangerousSanPhuKhoa == true || checkDangerousMat == true || checkDangerousTai == true || checkDangerousRangHamMat == true || checkDangerousDaLieu == true)
                {
                    checkResult = true;
                }

                if (item.PatientStatus_ID == 44)
                {
                    checkResult = true;
                }

                var patientUserFalse = db.Patients.FirstOrDefault(p => p.ID == item.Patient_ID);
                if (item.ResultCTMau == false)
                {
                    countresult += 1;
                }
                if (item.ResultSHM == false)
                {
                    countresult += 1;
                }
                if (item.ResultDMau == false)
                {
                    countresult += 1;
                }
                if (item.ResultNhomMau == false)
                {
                    countresult += 1;
                }
                if (item.ResultNuocTieu == false)
                {
                    countresult += 1;
                }
                if (item.ResultMienDich == false)
                {
                    countresult += 1;
                }
                if (item.ResultDichChocDo == false)
                {
                    countresult += 1;
                }
                if (item.ResultViSinh == false)
                {
                    countresult += 1;
                }
                var NotiResultFalse = new { patientUserFalse.Name, countresult, item.ID, checkResult };
                patient.Add(NotiResultFalse);
                countresult = 0;
            }
            return await Task.Run(() => Json(new { data = patient }, JsonRequestBehavior.AllowGet));
         }

        public async Task<ActionResult> getPatientDangerous()
        {
            var Noti = Session["Noti"];
            return await Task.Run(() => Json(new { data = Noti }, JsonRequestBehavior.AllowGet));
        }

        public async Task<ActionResult> GetNotificationBS()
        {
            NotificationComponentBS NC = new NotificationComponentBS();
            var list = NC.ReturnResultTest();
            List<object> Noti = new List<object>();
            var count = 0;
            foreach (var item1 in list)
            {
                var detail_TuanHoan = db.Detail_TuanHoan.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_HoHap = db.Detail_HoHap.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_TieuHoa = db.Detail_TieuHoa.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_ThanTietNieu = db.Detail_ThanTietNieu.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_CoXuongKhop = db.Detail_CoXuongKhop.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_ThanKinh = db.Detail_ThanKinh.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_TamThan = db.Detail_TamThan.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_NgoaiKhoa = db.Detail_NgoaiKhoa.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_SanPhuKhoa = db.Detail_SanPhuKhoa.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_Mat = db.Detail_Mat.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_TaiMuiHong = db.Detail_TaiMuiHong.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_RangHamMat = db.Detail_RangHamMat.Where(p => p.InformationExamination_ID == item1.ID).ToList();
                var detail_DaLieu = db.Detail_DaLieu.Where(p => p.InformationExamination_ID == item1.ID).ToList();

                var checkDangerousTuanHoan = detail_TuanHoan.Any(p => p.TuanHoan.Dangerous == true);
                var checkDangerousHoHap = detail_HoHap.Any(p => p.HoHap.Dangerous == true);
                var checkDangerousTieuHoa = detail_TieuHoa.Any(p => p.TieuHoa.Dangerous == true);
                var checkDangerousThanTietNieu = detail_ThanTietNieu.Any(p => p.ThanTietNieu.Dangerous == true);
                var checkDangerousCoXuongKhop = detail_CoXuongKhop.Any(p => p.CoXuongKhop.Dangerous == true);
                var checkDangerousThanKinh = detail_ThanKinh.Any(p => p.ThanKinh.Dangerous == true);
                var checkDangerousTamThan = detail_TamThan.Any(p => p.TamThan.Dangerous == true);
                var checkDangerousNgoaiKhoa = detail_NgoaiKhoa.Any(p => p.NgoaiKhoa.Dangerous == true);
                var checkDangerousSanPhuKhoa = detail_SanPhuKhoa.Any(p => p.SanPhuKhoa.Dangerous == true);
                var checkDangerousMat = detail_Mat.Any(p => p.Mat.Dangerous == true);
                var checkDangerousTai = detail_TaiMuiHong.Any(p => p.TaiMuiHong.Dangerous == true);
                var checkDangerousRangHamMat = detail_RangHamMat.Any(p => p.RangHamMat.Dangerous == true);
                var checkDangerousDaLieu = detail_DaLieu.Any(p => p.DaLieu.Dangerous == true);

                var checkResult = false;
                if (checkDangerousTuanHoan == true || checkDangerousHoHap == true || checkDangerousTieuHoa == true || checkDangerousThanTietNieu == true
                    || checkDangerousCoXuongKhop == true || checkDangerousThanKinh == true || checkDangerousTamThan == true || checkDangerousNgoaiKhoa == true
                    || checkDangerousSanPhuKhoa == true || checkDangerousMat == true || checkDangerousTai == true || checkDangerousRangHamMat == true || checkDangerousDaLieu == true)
                {
                    checkResult = true;
                }

                if (item1.PatientStatus_ID == 44)
                {
                    checkResult = true;
                }

                var patientUser = db.Patients.FirstOrDefault(p => p.ID == item1.Patient_ID);
                if (item1.ResultCTMau == true)
                {
                    count += 1;
                }
                if (item1.ResultSHM == true)
                {
                    count += 1;
                }
                if (item1.ResultDMau == true)
                {
                    count += 1;
                }
                if (item1.ResultNhomMau == true)
                {
                    count += 1;
                }
                if (item1.ResultNuocTieu == true)
                {
                    count += 1;
                }
                if (item1.ResultMienDich == true)
                {
                    count += 1;
                }
                if (item1.ResultDichChocDo == true)
                {
                    count += 1;
                }
                if (item1.ResultViSinh == true)
                {
                    count += 1;
                }
                var NotiResult = new { patientUser.Name, count, item1.ID, checkResult };
                Noti.Add(NotiResult);
                count = 0;
            }
            return await Task.Run(() => Json(new { data = Noti }, JsonRequestBehavior.AllowGet));
        }

        public async Task<ActionResult> GetNotificationBS1()
        {
            NotificationComponentBS1 NCBS = new NotificationComponentBS1();
            var list = NCBS.ReturnResultTestBS1();
            List<object> NotiBS = new List<object>();
            foreach (var item1 in list)
            {
                var dangerPatient = false;
                var patientUser = db.Patients.FirstOrDefault(p => p.ID == item1.Patient_ID);
                var date = item1.DateExamine.Value.ToString("dd/MM/yyyy hh:mm:ss");
                if(item1.PatientStatus_ID == 44)
                {
                    dangerPatient = true;
                }
                var NotiResult = new { patientUser.Name, date, item1.ID, dangerPatient };
                NotiBS.Add(NotiResult);
            }
            return await Task.Run(() => Json(new { data = NotiBS }, JsonRequestBehavior.AllowGet));
        }

        // GET: Admin/InformationExaminations/Details/5
        public ActionResult Details(int id)
        {
            var informationExaminations = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            ViewBag.id = id;
            return View(informationExaminations);
        }

        [HttpPost]
        public ActionResult SearchPatientDetail(DateTime? DateStartDetail, DateTime? DateEndDetail, int? id)
        {
            PatientsController patientsController = new PatientsController();
            var checkID = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            if (DateStartDetail == null && DateEndDetail == null)
            {
                ViewBag.ErrorInfo = "Vui lòng nhập ít nhất 1 trường";
                return RedirectToAction("Details","InformationExaminations", new { id = id});
            }
            if(DateStartDetail.HasValue)
            {
                TimeSpan timeStart = new TimeSpan(1, 0, 01);
                DateStartDetail = DateStartDetail + timeStart;
                checkID = checkID.Where(p => p.DateExamine >= DateStartDetail.Value).ToList();
            }
            if (DateEndDetail.HasValue)
            {
                TimeSpan timeEnd = new TimeSpan(23, 59, 59);
                DateEndDetail = DateEndDetail + timeEnd;
                checkID = checkID.Where(p => p.DateEnd <= DateEndDetail.Value).ToList();
            }
            ViewBag.id = id;
            return View("Details", checkID);
        }

        // GET: Admin/InformationExaminations/DetailIE/5
        public ActionResult DetailIE(MultiplesModel multiplesModel)
        {
            var UserName = db.Users.FirstOrDefault(p => p.ID == multiplesModel.InformationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
            if(UserName != null)
            {
                ViewBag.UserName = UserName.Name;
            }
            return PartialView("_DetailsIE", multiplesModel);
        }

        public ActionResult DetailIERead(MultiplesModel multiplesModel)
        {
            var UserName = db.Users.FirstOrDefault(p => p.ID == multiplesModel.InformationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
            if (UserName != null)
            {
                ViewBag.UserName = UserName.Name;
            }
            return PartialView("_DetailIERead", multiplesModel);
        }

        // GET: Admin/InformationExaminations/Create1
        public ActionResult Create1()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(ids => ids.UserID == UserID);
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewBag.DateExamination = DateTime.Now;
            return PartialView("_Create1");
        }

        // GET: Admin/InformationExaminations/BillExamination
        public ActionResult BillExamination(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(ids => ids.UserID == UserID);
            var info = db.InformationExaminations.Find(id);
            multiplesModel.InformationExamination = info;
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewBag.DateExamination = DateTime.Now;
            return PartialView("_BillExamination", multiplesModel);
        }


        // GET: Admin/InformationExaminations/Create
        public ActionResult Create(int? id)
        {
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(ids => ids.UserID == UserID);
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewBag.DateExamination = DateTime.Now;
            if (id != null)
            {
                var listInfo = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
                var informationExamination = listInfo.LastOrDefault();
                ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            }
            else
            {
                ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name");
            }
            return PartialView("_Create");
        }

        // POST: Admin/InformationExaminations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int patientID, MultiplesModel multiplesModel)
        {
            InformationExamination informationExamination = new InformationExamination();
            if (ModelState.IsValid)
            {
                informationExamination.Patient_ID = patientID;
                informationExamination.DateExamine = DateTime.Now;
                informationExamination.DateEnd = DateTime.Now;
                informationExamination.PatientStatus_ID = multiplesModel.InformationExamination.PatientStatus_ID;
                informationExamination.BloodPressure = multiplesModel.InformationExamination.BloodPressure;
                informationExamination.Breathing = multiplesModel.InformationExamination.Breathing;
                informationExamination.HeartBeat = multiplesModel.InformationExamination.HeartBeat;
                informationExamination.Weight = multiplesModel.InformationExamination.Weight;
                informationExamination.Height = multiplesModel.InformationExamination.Height;
                db.InformationExaminations.Add(informationExamination);
                db.SaveChanges();
                return RedirectToAction("Create", "MultipleModels");
            }

            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // GET: Admin/InformationExaminations/BillCheck/5
        public ActionResult BillCheck(MultiplesModel multiplesModel)
        {
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
            return PartialView("_BillCheck", multiplesModel);
        }

        // GET: Admin/InformationExaminations/CreateOldPatient/5
        [HttpGet]
        public ActionResult CreateOldPatient(MultiplesModel multiplesModel)
        {
            //InformationExamination informationExamination = db.InformationExaminations.Find(multiplesModel.InformationExamination.ID);
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(ids => ids.UserID == UserID);
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            //if (informationExamination == null)
            //{
            //    return HttpNotFound();
            //}
            //var UserName = db.Users.FirstOrDefault(p => p.ID == informationExamination.User_ID);
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
            //multiplesModel.InformationExamination = informationExamination;
            return PartialView("_CreateOldPatient", multiplesModel);
        }

        [HttpPost, ActionName("CreateOldPatient")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOldPatientPost(MultiplesModel multiplesModel)
        {
            InformationExamination informationExamination = new InformationExamination();
            if (ModelState.IsValid)
            {
                multiplesModel.InformationExamination.Patient_ID = multiplesModel.Patient.ID;
                multiplesModel.InformationExamination.DateExamine = DateTime.Now;
                multiplesModel.InformationExamination.DateEnd = DateTime.Now;
                db.InformationExaminations.Add(multiplesModel.InformationExamination);
                db.SaveChanges();
                return RedirectToAction("CreateOldPatient", "MultipleModels");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest(InformationExamination informationExamination, Detail_DiagnosticsCategory detail_DiagnosticsCategory)
        {
            if (ModelState.IsValid)
            {
                informationExamination.DateEnd = DateTime.Now;
                db.Entry(informationExamination).State = EntityState.Modified;
                db.SaveChanges();

                if(detail_DiagnosticsCategory.DiagnosticsCategory_ID != null)
                {
                    detail_DiagnosticsCategory.InformationExamination_ID = informationExamination.ID;
                    db.Detail_DiagnosticsCategory.Add(detail_DiagnosticsCategory);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "MultipleModels");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // GET: Admin/InformationExaminations/Edit/5
        public ActionResult Edit(MultiplesModel multiplesModel)
        {
            Clinical clinical = db.Clinicals.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
            var UserID = User.Identity.GetUserId();
            var userID = db.Users.FirstOrDefault(ids => ids.UserID == UserID);
            ViewBag.UserByID = userID.ID;
            ViewBag.UserName = userID.Name;
            ViewData["InformationExamination.PatientStatus_ID"] = new SelectList(db.PatientStatus, "ID", "Name", multiplesModel.InformationExamination.PatientStatus_ID);
            multiplesModel.Clinical = clinical;
            return PartialView("_Edit",multiplesModel);
        }

        // POST: Admin/InformationExaminations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InformationExamination informationExamination, Detail_DiagnosticsCategory detail_DiagnosticsCategory)
        {
            if (ModelState.IsValid)
            {
                informationExamination.DateEnd = DateTime.Now;
                db.Entry(informationExamination).State = EntityState.Modified;
                db.SaveChanges();
                if(detail_DiagnosticsCategory != null)
                {
                    var checkExist = db.Detail_DiagnosticsCategory.AsNoTracking().FirstOrDefault(p => p.InformationExamination_ID == detail_DiagnosticsCategory.InformationExamination_ID);
                    if (detail_DiagnosticsCategory.DiagnosticsCategory_ID != null && checkExist == null)
                    {
                        detail_DiagnosticsCategory.InformationExamination_ID = informationExamination.ID;
                        db.Detail_DiagnosticsCategory.Add(detail_DiagnosticsCategory);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(detail_DiagnosticsCategory).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }               
                return RedirectToAction("Index", "MultipleModels");
            }
            ViewBag.Patient_ID = new SelectList(db.Patients, "ID", "Name", informationExamination.Patient_ID);
            ViewBag.PatientStatus_ID = new SelectList(db.PatientStatus, "ID", "Name", informationExamination.PatientStatus_ID);
            ViewBag.User_ID = new SelectList(db.Users, "ID", "Name", informationExamination.User_ID);
            return View(informationExamination);
        }

        // POST: Admin/InformationExaminations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InformationExamination checkInfoExam = db.InformationExaminations.Find(id);
            if (checkInfoExam.ResultCTMau != null
                    || checkInfoExam.ResultSHM != null || checkInfoExam.ResultDMau != null
                    || checkInfoExam.ResultNhomMau != null || checkInfoExam.ResultNuocTieu != null
                    || checkInfoExam.ResultMienDich != null || checkInfoExam.ResultDichChocDo != null
                    || checkInfoExam.ResultViSinh != null || checkInfoExam.PricePrescription != null || checkInfoExam.PriceExamination != null
                    || checkInfoExam.PriceTest != null)
            {
                return Json(new { success = false });
            }
            else
            {
                var patientCheck = db.Patients.FirstOrDefault(p => p.ID == checkInfoExam.Patient_ID);
                db.InformationExaminations.Remove(checkInfoExam);
                db.SaveChanges();
                return Json(new { success = true });
            }
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
