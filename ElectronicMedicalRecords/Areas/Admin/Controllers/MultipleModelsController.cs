using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Areas.Admin.Controllers
{
    [Authorize(Roles = "Bác Sĩ,Giám Đốc,QTV,Kỹ Thuật Viên,Y tá/Điều dưỡng")]
    public class MultipleModelsController : Controller
    {
        private CP24Team08Entities db = new CP24Team08Entities();
        // GET: Admin/MultipleModels
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print()
        {
            return View();
        }

        // GET: Admin/MultipleModels/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult LoadDetailBloods(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<CTMau> listNewBloods = new List<CTMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var blood = db.CTMaus.FirstOrDefault(p => p.ID == id);
                    blood.ChiDinh = true;
                    listNewBloods.Add(blood);
                }
                return Json(new { data = listNewBloods }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailUrines(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Urine> listNewUrines = new List<Urine>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var urine = db.Urines.FirstOrDefault(p => p.ID == id);
                    urine.ChiDinh = true;
                    listNewUrines.Add(urine);
                }
                return Json(new { data = listNewUrines }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailImmune(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Immune> listNewImmunes = new List<Immune>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var immune = db.Immunes.FirstOrDefault(p => p.ID == id);
                    immune.ChiDinh = true;
                    listNewImmunes.Add(immune);
                }
                return Json(new { data = listNewImmunes }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailAmniocente(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Amniocente> aMniocentes = new List<Amniocente>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var amniocente = db.Amniocentes.FirstOrDefault(p => p.ID == id);
                    amniocente.ChiDinh = true;
                    aMniocentes.Add(amniocente);
                }
                return Json(new { data = aMniocentes }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailSHM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<SinhHoaMau> sinhHoaMaus = new List<SinhHoaMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var SHM = db.SinhHoaMaus.FirstOrDefault(p => p.ID == id);
                    SHM.ChiDinh = true;
                    sinhHoaMaus.Add(SHM);
                }
                return Json(new { data = sinhHoaMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailDM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<DongMau> dongMaus = new List<DongMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var DM = db.DongMaus.FirstOrDefault(p => p.ID == id);
                    DM.ChiDinh = true;
                    dongMaus.Add(DM);
                }
                return Json(new { data = dongMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailNM(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<NhomMau> nhomMaus = new List<NhomMau>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var NM = db.NhomMaus.FirstOrDefault(p => p.ID == id);
                    NM.ChiDinh = true;
                    nhomMaus.Add(NM);
                }
                return Json(new { data = nhomMaus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDetailVS(int[] arr)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ViSinh> viSinhs = new List<ViSinh>();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = arr[i];
                    var VS = db.ViSinhs.FirstOrDefault(p => p.ID == id);
                    VS.ChiDinh = true;
                    viSinhs.Add(VS);
                }
                return Json(new { data = viSinhs }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkExistPatient(Patient patient)
        {
            var checkExist = db.Patients.FirstOrDefault(p => p.Name == patient.Name && p.Address == patient.Address && p.BirthDate == patient.BirthDate);
            if (checkExist != null)
            {
                return Json(new { success = false, responseText = "Bệnh Nhân Đã Tồn Tại. Vui Lòng Kiểm Tra Lại" });
            }
            else
            {
                return Json(new { success = true });
            }
        }

        [HttpGet]
        // GET: Admin/MultipleModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MultipleModels/Create
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> Create(MultiplesModel multiplesModel)
         {
            PatientsController patientsController = new PatientsController();
            InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
            try
            {
                // TODO: Add insert logic here
                patientsController.Create(multiplesModel.Patient);
                var PatientID = multiplesModel.Patient.ID;
                informationExaminationsController.Create(PatientID);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ex)
            {
                //return View();
                var error = ex;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/CreateOldPatient/5
        public ActionResult CreateOldPatient(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExaminationList = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var inforamtionExamination = inforamtionExaminationList.LastOrDefault();
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/CreateOldPatient/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> CreateOldPatient(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateOldPatient(multiplesModel);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ex1)
            {
                var error = ex1;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/CreateOldPatient/5
        public ActionResult CreateTest(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == inforamtionExamination.Patient_ID);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/CreateOldPatient/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> CreateTest(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
                Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
                Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
                Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
                Detail_UrineController detail_UrineController = new Detail_UrineController();
                Detail_ImmuneController detail_ImmuneController = new Detail_ImmuneController();
                Detail_AmniocenteController detail_AmniocenteController = new Detail_AmniocenteController();
                Detail_ViSinhController detail_ViSinhController = new Detail_ViSinhController();
                ClinicalsController clinicalsController = new ClinicalsController();
                //CayMausController cayMausController = new CayMausController();

                Detail_CTMau detail_CTMau = new Detail_CTMau();
                Detail_SinhHoaMau detail_SinhHoaMau = new Detail_SinhHoaMau();
                Detail_DongMau detail_DongMau = new Detail_DongMau();
                Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
                Detail_Urine detail_Urine = new Detail_Urine();
                Detail_Immune detail_Immune = new Detail_Immune();
                Detail_Amniocente detail_Amniocente = new Detail_Amniocente();
                Detail_ViSinh detail_ViSinh = new Detail_ViSinh();
                patientsController.CreateOldPatient(multiplesModel.Patient);
                informationExaminationsController.CreateTest(multiplesModel.InformationExamination);
                var CongThucMauCD = Task.Run(() => detail_CTMauController.CreateOldPatient(detail_CTMau, multiplesModel.CTMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var SinhHoaMauCD = Task.Run(() => detail_SinhHoaMauController.CreateOldPatient(detail_SinhHoaMau, multiplesModel.SinhHoaMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var DongMauCD = Task.Run(() => detail_DongMauController.CreateOldPatient(detail_DongMau, multiplesModel.DongMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var NhomMauCD = Task.Run(() => detail_NhomMauController.CreateOldPatient(detail_NhomMau, multiplesModel.NhomMau, multiplesModel.InformationExamination.ID, multiplesModel));
                var UrineCD = Task.Run(() => detail_UrineController.CreateOldPatient(detail_Urine, multiplesModel.Urine, multiplesModel.InformationExamination.ID, multiplesModel));
                var ImmuneCD = Task.Run(() => detail_ImmuneController.CreateOldPatient(detail_Immune, multiplesModel.Immune, multiplesModel.InformationExamination.ID, multiplesModel));
                var AmniocenteCD = Task.Run(() => detail_AmniocenteController.CreateOldPatient(detail_Amniocente, multiplesModel.Amniocente, multiplesModel.InformationExamination.ID, multiplesModel));
                var ViSinhCD = Task.Run(() => detail_ViSinhController.CreateOldPatient(detail_ViSinh, multiplesModel.ViSinh, multiplesModel.InformationExamination.ID, multiplesModel));
                var ResultNew = await Task.WhenAll(CongThucMauCD, SinhHoaMauCD, DongMauCD, NhomMauCD, UrineCD, ImmuneCD, AmniocenteCD, ViSinhCD);
                clinicalsController.CreateOldPatient(multiplesModel);
                //cayMausController.CreateOldPatient(multiplesModel);

                var checkCongThucMauCD = db.Detail_CTMau.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkCongThucMauCD != null)
                {
                    multiplesModel.InformationExamination.ResultCTMau = false;
                }
                var checkSinhHoaMauCD = db.Detail_SinhHoaMau.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID); ;
                if (checkSinhHoaMauCD != null)
                {
                    multiplesModel.InformationExamination.ResultSHM = false;
                }
                var checkDongMauCD = db.Detail_DongMau.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkDongMauCD != null)
                {
                    multiplesModel.InformationExamination.ResultDMau = false;
                }
                var checkNhomMauCD = db.Detail_NhomMau.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkNhomMauCD != null)
                {
                    multiplesModel.InformationExamination.ResultNhomMau = false;
                }
                var checkUrineCD = db.Detail_Urine.FirstOrDefault(p => p.InfomationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkUrineCD != null)
                {
                    multiplesModel.InformationExamination.ResultNuocTieu = false;
                }
                var checkImmuneCD = db.Detail_Immune.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkImmuneCD != null)
                {
                    multiplesModel.InformationExamination.ResultMienDich = false;
                }
                var checkAmniocenteCD = db.Detail_Amniocente.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkAmniocenteCD != null)
                {
                    multiplesModel.InformationExamination.ResultDichChocDo = false;
                }
                var checkViSinhCD = db.Detail_ViSinh.FirstOrDefault(p => p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                if (checkViSinhCD != null)
                {
                    multiplesModel.InformationExamination.ResultViSinh = false;
                }
                if (multiplesModel.InformationExamination.Breathing != null || multiplesModel.InformationExamination.HeartBeat != null
                    || multiplesModel.InformationExamination.BloodPressure != null || multiplesModel.InformationExamination.Weight != null 
                    || multiplesModel.InformationExamination.Height != null || checkCongThucMauCD != null || checkSinhHoaMauCD != null
                    || checkDongMauCD != null || checkNhomMauCD != null || checkUrineCD != null || checkImmuneCD != null || checkAmniocenteCD != null
                    || checkViSinhCD != null)
                {
                    multiplesModel.InformationExamination.New = true;
                }
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                db.SaveChanges();
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch (Exception ex1)
            {
                var error = ex1;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/Edit/5
        public ActionResult Edit(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var inforamtionExaminationList = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            var inforamtionExamination = inforamtionExaminationList.LastOrDefault();
            var patient = db.Patients.Find(id);
            multiplesModel.InformationExamination = inforamtionExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult EditByID(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        public ActionResult DetailsIE(int id)
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            var InformationExamination = db.InformationExaminations.Find(id);
            var patient = db.Patients.FirstOrDefault(p => p.ID == InformationExamination.Patient_ID);
            multiplesModel.InformationExamination = InformationExamination;
            multiplesModel.Patient = patient;
            return View(multiplesModel);
        }

        // POST: Admin/MultipleModels/Edit/5
        [HttpPost, ValidateInput(false)]
        public async Task<RedirectToRouteResult> Edit(MultiplesModel multiplesModel)
        {
            try
            {
                // TODO: Add update logic here
                PatientsController patientsController = new PatientsController();
                InformationExaminationsController informationExaminationsController = new InformationExaminationsController();
                Detail_CTMauController detail_CTMauController = new Detail_CTMauController();
                Detail_SinhHoaMauController detail_SinhHoaMauController = new Detail_SinhHoaMauController();
                Detail_DongMauController detail_DongMauController = new Detail_DongMauController();
                Detail_NhomMauController detail_NhomMauController = new Detail_NhomMauController();
                Detail_UrineController detail_UrineController = new Detail_UrineController();
                Detail_ImmuneController detail_ImmuneController = new Detail_ImmuneController();
                Detail_AmniocenteController detail_AmniocenteController = new Detail_AmniocenteController();
                Detail_ViSinhController detail_ViSinhController = new Detail_ViSinhController();
                ClinicalsController clinicalsController = new ClinicalsController();
                //CayMausController cayMausController = new CayMausController();
                patientsController.Edit(multiplesModel.Patient);
                informationExaminationsController.Edit(multiplesModel.InformationExamination);
                var CongThucMauEdit = Task.Run(() => detail_CTMauController.Edit(multiplesModel));
                var SinhHoaMauEdit = Task.Run(() => detail_SinhHoaMauController.Edit(multiplesModel));
                var DongMauEdit = Task.Run(() => detail_DongMauController.Edit(multiplesModel));
                var NhomMauEdit = Task.Run(() => detail_NhomMauController.Edit(multiplesModel));
                var UrineEdit = Task.Run(() => detail_UrineController.Edit(multiplesModel));
                var ImmuneEdit = Task.Run(() => detail_ImmuneController.Edit(multiplesModel));
                var AmniocenteEdit = Task.Run(() => detail_AmniocenteController.Edit(multiplesModel));
                var ViSinhEdit = Task.Run(() => detail_ViSinhController.Edit(multiplesModel));
                var ResultEdit = await Task.WhenAll(CongThucMauEdit, SinhHoaMauEdit, DongMauEdit, NhomMauEdit, UrineEdit, ImmuneEdit, AmniocenteEdit, ViSinhEdit);
                clinicalsController.Edit(multiplesModel);
                if(multiplesModel.Detail_CTMaus != null)
                {
                    var checkResultCTMau = multiplesModel.Detail_CTMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultCTMau == true)
                    {
                        multiplesModel.InformationExamination.ResultCTMau = true;
                    }
                }
                if(multiplesModel.Detail_SinhHoaMaus != null)
                {
                    var checkResultSHM = multiplesModel.Detail_SinhHoaMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultSHM == true)
                    {
                        multiplesModel.InformationExamination.ResultSHM = true;
                    }
                }
                if(multiplesModel.Detail_DongMaus != null)
                {
                    var checkResultDMau = multiplesModel.Detail_DongMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultDMau == true)
                    {
                        multiplesModel.InformationExamination.ResultDMau = true;
                    }
                }
                if(multiplesModel.Detail_NhomMaus != null)
                {
                    var checkResultNMau = multiplesModel.Detail_NhomMaus.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultNMau == true)
                    {
                        multiplesModel.InformationExamination.ResultNhomMau = true;
                    }
                }
                if(multiplesModel.Detail_Urines != null)
                {
                    var checkResultNuocTieu = multiplesModel.Detail_Urines.All(p => p.Result != null && p.InfomationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultNuocTieu == true)
                    {
                        multiplesModel.InformationExamination.ResultNuocTieu = true;
                    }
                }
                if(multiplesModel.Detail_Immunes != null)
                {
                    var checkResultImmune = multiplesModel.Detail_Immunes.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultImmune == true)
                    {
                        multiplesModel.InformationExamination.ResultMienDich = true;
                    }
                }
                if(multiplesModel.Detail_Amniocentes != null)
                {
                    var checkResultAmniocente = multiplesModel.Detail_Amniocentes.All(p => p.Result != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultAmniocente == true)
                    {
                        multiplesModel.InformationExamination.ResultDichChocDo = true;
                    }
                }
                if (multiplesModel.Detail_ViSinhs != null)
                {
                    var checkResultViSinhs = multiplesModel.Detail_ViSinhs.All(p => p.Result != null || p.ResultNC != null || p.ResultDD != null || p.MatDo != null && p.InformationExamination_ID == multiplesModel.InformationExamination.ID);
                    if (checkResultViSinhs == true)
                    {
                        multiplesModel.InformationExamination.ResultViSinh = true;
                    }
                }
                db.Entry(multiplesModel.InformationExamination).State = EntityState.Modified;
                db.SaveChanges();
                //cayMausController.Edit(multiplesModel.CayMau, Server);
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
            catch(Exception ED)
            {
                var errorEdit = ED;
                return await Task.Run(() => RedirectToAction("Index", "Patients"));
            }
        }

        // GET: Admin/MultipleModels/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/MultipleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var InfoExamination = db.InformationExaminations.Where(p => p.Patient_ID == id).ToList();
            if(InfoExamination.Count == 1)
            {
                var checkInfoExam = db.InformationExaminations.FirstOrDefault(p => p.Patient_ID == id);
                if (checkInfoExam.HeartBeat != null || checkInfoExam.Breathing != null
                    || checkInfoExam.BloodPressure != null || checkInfoExam.Weight != null
                    || checkInfoExam.Height != null || checkInfoExam.ResultCTMau != null
                    || checkInfoExam.ResultSHM != null || checkInfoExam.ResultDMau != null
                    || checkInfoExam.ResultNhomMau != null || checkInfoExam.ResultNuocTieu != null
                    || checkInfoExam.ResultMienDich != null || checkInfoExam.ResultDichChocDo != null
                    || checkInfoExam.ResultViSinh != null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    var patient = db.Patients.Find(id);
                    db.InformationExaminations.Remove(checkInfoExam);
                    db.Patients.Remove(patient);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new { success = false });
            }            
        }
    }
}
