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
    public class Detail_DongMauControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        Detail_DongMauController controller = new Detail_DongMauController();
        [TestMethod]
        public void CreateP()
        {
            var dongMaus = db.DongMaus.ToList();
            dongMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_DongMau detail_DongMau = new Detail_DongMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(detail_DongMau, dongMaus, info.ID, multiplesModel) as Task<ActionResult>;
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
            var dongMaus = db.DongMaus.ToList();
            dongMaus[1].ChiDinh = true;
            var info = db.InformationExaminations.AsNoTracking().First();
            MultiplesModel multiplesModel = new MultiplesModel();
            Detail_DongMau detail_DongMau = new Detail_DongMau();
            using (var scope = new TransactionScope())
            {
                var result = controller.CreateOldPatient(detail_DongMau, dongMaus, info.ID, multiplesModel) as ActionResult;
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void EditP()
        {
            var info = db.InformationExaminations.AsNoTracking().First();
            var dongMausList = db.Detail_DongMau.Where(p => p.InformationExamination_ID == info.ID).ToList();
            MultiplesModel multiplesModel = new MultiplesModel();
            multiplesModel.Detail_DongMaus = dongMausList;
            using (var scope = new TransactionScope())
            {
                var result = controller.EditPost(multiplesModel) as Task<ActionResult>;
                Assert.IsNotNull(result);
            }
        }
    }
}
