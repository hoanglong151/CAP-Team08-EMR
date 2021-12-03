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
        //string conbs = ConfigurationManager.ConnectionStrings["sqlConStringBS"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(conString);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationComponentKTV NCktv = new NotificationComponentKTV();
            NotificationComponentBS NCbs = new NotificationComponentBS();
            NotificationComponentBS1 NCyta = new NotificationComponentBS1();
            //var currentTimebs = DateTime.Now;
            //var currentTimektv = DateTime.Now;
            //HttpContext.Current.Session["LastUpdated"] = currentTimektv;
            NCktv.RegisterNotificationKTV();
            NCbs.RegisterNotificationBS();
            NCyta.RegisterNotificationBS1();
        }

        //protected void Session_Startbs(object sender, EventArgs e)
        //{
        //    NotificationComponentBS NCbs = new NotificationComponentBS();
        //    var currentTimebs = DateTime.Now;
        //    HttpContext.Current.Session["LastUpdated"] = currentTimebs;
        //    NCbs.RegisterNotificationBS(currentTimebs);
        //}

        protected void Application_End()
        {
            SqlDependency.Stop(conString);
        }
    }
}
