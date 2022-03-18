using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicMedicalRecords
{
    public class NotificationComponentBS1
    {
        public void RegisterNotificationBS1()
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [New],[Examining] from [dbo].[InformationExamination]";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                //cmd.Parameters.AddWithValue("@DateEnd", currentTime);
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChangeBS1;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChangeBS1(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChangeBS1;

                var notificationHubBS1 = GlobalHost.ConnectionManager.GetHubContext<NotificationHubBS1>();
                notificationHubBS1.Clients.All.notifyResultBS1("added");

                RegisterNotificationBS1();
            }
        }

        public List<InformationExamination> ReturnResultTestBS1()
        {
            using (CP24Team08Entities db = new CP24Team08Entities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.InformationExaminations.Where(a => a.New == false && a.Examining != true).OrderByDescending(a => a.DateExamine).ToList();
            }
        }
    }
}