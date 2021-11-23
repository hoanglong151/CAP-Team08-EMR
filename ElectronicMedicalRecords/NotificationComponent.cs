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
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [ID],[Patient_ID] from [dbo].[InformationExamination] where DateExamine > @DateExamine";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@DateExamine", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                RegisterNotification(DateTime.Now);
            }
        }

        public List<InformationExamination> GetInformationExamination()
        {
            using (CP24Team08Entities db = new CP24Team08Entities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.InformationExaminations.Where(a => a.TestCD == false).OrderByDescending(a => a.DateExamine).ToList();
            }
        }
    }
}