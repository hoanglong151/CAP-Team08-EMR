using ElectronicMedicalRecords;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class Detail_NhomMauControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        Detail_NhomMauController controller = new Detail_NhomMauController();
        [TestMethod]
        public void CreateP()
        {
            var nhomMaus = db.NhomMaus.ToList();
            nhomMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(detail_NhomMau, nhomMaus, info.ID, multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void DetailIE()
        {
            using (var scope = new TransactionScope())
            {
                var info = db.InformationExaminations.First();
                var result = controller.DetailIE(info.ID) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_DetailIE", result.ViewName);
            }
        }

        [TestMethod]
        public void BillCheck()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.BillCheck(info.ID) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_BillCheck", result.ViewName);
            }
        }

        [TestMethod]
        public void EditG()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(info.ID) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_Edit", result.ViewName);
            }
        }

        [TestMethod]
        public void CreateOldPatientP()
        {
            var nhomMaus = db.NhomMaus.ToList();
            nhomMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_NhomMau detail_NhomMau = new Detail_NhomMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(detail_NhomMau, nhomMaus, info.ID, multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void EditP()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            var NhomMauList = db.Detail_NhomMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Detail_NhomMaus = NhomMauList;
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }
    }
}
