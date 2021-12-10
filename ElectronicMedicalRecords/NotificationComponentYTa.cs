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
    public class NotificationComponentYTa
    {
        public void RegisterNotificationYTa()
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [ID] from [dbo].[InformationExamination]";
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
                sqlDep.OnChange += sqlDep_OnChangeYTa;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChangeYTa(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChangeYTa;

                var notificationHubYTa = GlobalHost.ConnectionManager.GetHubContext<NotificationHubYTa>();
                notificationHubYTa.Clients.All.notifyPatient("added");

                RegisterNotificationYTa();
            }
        }
    }
}