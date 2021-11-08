using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        UsersController controller = new UsersController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<User>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.Users.Count(), model.Count);
        }

        [TestMethod]
        public void TestEditG()
        {
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var user = db.Users.First();
            var result1 = controller.Edit(user.ID) as ViewResult;
            Assert.IsNotNull(result1);

            var model = result1.Model as User;
            Assert.AreEqual(user.Address, model.Address);
            Assert.AreEqual(user.BirthDate, model.BirthDate);
            Assert.AreEqual(user.Degree, model.Degree);
            Assert.AreEqual(user.Gender_ID, model.Gender_ID);
            Assert.AreEqual(user.HomeTown_ID, model.HomeTown_ID);
            Assert.AreEqual(user.Image, model.Image);
            Assert.AreEqual(user.Introduction, model.Introduction);
            Assert.AreEqual(user.IsShow, model.IsShow);
            Assert.AreEqual(user.Name, model.Name);
            Assert.AreEqual(user.Nation_ID, model.Nation_ID);
            Assert.AreEqual(user.Phone, model.Phone);
            Assert.AreEqual(user.Privacy, model.Privacy);
            Assert.AreEqual(user.Religion_ID, model.Religion_ID);
            Assert.AreEqual(user.SpecializedTreatment, model.SpecializedTreatment);
            Assert.AreEqual(user.TrainingProcess, model.TrainingProcess);
            Assert.AreEqual(user.UserID, model.UserID);
            Assert.AreEqual(user.WorkingProcess, model.WorkingProcess);
            Assert.AreEqual(user.ActiveAccount, model.ActiveAccount);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var user = db.Users.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(user.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);

                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
