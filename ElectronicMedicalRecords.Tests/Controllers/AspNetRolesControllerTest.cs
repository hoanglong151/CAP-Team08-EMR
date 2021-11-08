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
    public class AspNetRolesControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        AspNetRolesController controller = new AspNetRolesController();

        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<AspNetRole>;
            Assert.IsNotNull(model);

            Assert.AreEqual(db.AspNetRoles.Count(), model.Count);
        }

        [TestMethod]
        public void TestDeleteP()
        {
            var user = db.AspNetUsers.AsNoTracking().First();
            var role = db.AspNetRoles.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteUserRole(role.Id, user.Id) as RedirectToRouteResult;
                Assert.IsNotNull(result);

                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
