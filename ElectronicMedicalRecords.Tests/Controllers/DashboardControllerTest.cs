using ElectronicMedicalRecords;
using ElectronicMedicalRecords.Areas.Admin.Controllers;
using ElectronicMedicalRecords.Controllers;
using ElectronicMedicalRecords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using static ElectronicMedicalRecords.Tests.Controllers.InformationExaminationsControllerTest;

namespace ElectronicMedicalRecords.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        CP24Team08Entities db = new CP24Team08Entities();
        DashboardController controller = new DashboardController();
        [TestMethod]
        public void GetData()
        {
            var result = controller.GetData() as JsonResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Dashboard()
        {
            var patient = db.Patients.FirstOrDefault();
            var user = db.AspNetUsers.First();
            var loggedInUsers = new Dictionary<string, DateTime>();
            //add this user to the list
            loggedInUsers.Add(user.Id, DateTime.Now);
            //add the list into the cache
            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { "Giám Đốc" });

            List<string> list = new List<string>();
            list.Add(user.Id);

            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["ListUsersOnline"]).Returns(list); //somevalue
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            mockControllerContext.Setup(t => t.HttpContext.User).Returns(genericPrincipal);
            controller.ControllerContext = mockControllerContext.Object;

            var result = controller.Dashboard() as ViewResult;
            Assert.IsNotNull(result);
        }
    }

}
