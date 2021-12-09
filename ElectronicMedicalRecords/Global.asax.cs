using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ElectronicMedicalRecords
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string conString = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlDependency.Start(conString);
            var loggedInUsers = new Dictionary<string, DateTime>();
            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationComponentKTV NCktv = new NotificationComponentKTV();
            NotificationComponentBS NCbs = new NotificationComponentBS();
            NotificationComponentBS1 NCyta = new NotificationComponentBS1();
            NCktv.RegisterNotificationKTV();
            NCbs.RegisterNotificationBS();
            NCyta.RegisterNotificationBS1();
        }
        protected void Application_End()
        {
            //var loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];

            //if (User.Identity.IsAuthenticated)
            //{
            //    var userName = User.Identity.GetUserId();
            //    if (loggedInUsers != null)
            //    {
            //        loggedInUsers[userName] = DateTime.Now;
            //        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
            //    }
            //}

            //if (loggedInUsers != null)
            //{
            //    foreach (var item in loggedInUsers.ToList())
            //    {
            //        if (item.Value < DateTime.Now.AddMinutes(-1))
            //        {
            //            loggedInUsers.Remove(item.Key);
            //        }
            //    }
            //    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
            //}
            SqlDependency.Stop(conString);
        }
    }
}
