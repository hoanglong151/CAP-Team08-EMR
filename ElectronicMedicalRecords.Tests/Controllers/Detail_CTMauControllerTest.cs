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
    public class Detail_CTMauControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        Detail_CTMauController controller = new Detail_CTMauController();
        [TestMethod]
        public void CreateP()
        {
            var cTMaus = db.CTMaus.ToList();
            cTMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_CTMau detail_CTMau = new Detail_CTMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(detail_CTMau, cTMaus, info.ID, multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void DetailIE()
        {
            using (var scope = new TransactionScope())
            {
                MultiplesModel multiplesModel = new MultiplesModel();
                var result = controller.DetailIE(multiplesModel) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_DetailIE", result.ViewName);
            }
        }

        [TestMethod]
        public void BillCheck()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            using (var scope = new TransactionScope())
            {
                var result = controller.BillCheck(multiplesModel) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_BillCheck", result.ViewName);
            }
        }

        [TestMethod]
        public void EditG()
        {
            MultiplesModel multiplesModel = new MultiplesModel();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(multiplesModel) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_Edit", result.ViewName);
            }
        }

        [TestMethod]
        public void CreateOldPatientP()
        {
            var cTMaus = db.CTMaus.ToList();
            cTMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_CTMau detail_CTMau = new Detail_CTMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(detail_CTMau, cTMaus, info.ID, multiplesModel) as ActionResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void EditP()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            var CTMauList = db.Detail_CTMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Detail_CTMaus = CTMauList;
            using (var scope = new TransactionScope())
            {
                var result = controller.EditPost(multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }
    }
}
