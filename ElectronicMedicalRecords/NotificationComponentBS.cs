﻿using ElectronicMedicalRecords.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicMedicalRecords
{
    public class NotificationComponentBS
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConStringBS"].ConnectionString;
            string sqlCommand = @"SELECT [ID],[Patient_ID] from [dbo].[InformationExamination] where DateEnd > @DateEnd";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@DateEnd", currentTime);
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

                var notificationHubBS = GlobalHost.ConnectionManager.GetHubContext<NotificationHubBS>();
                notificationHubBS.Clients.All.notify("added1");

                RegisterNotification(DateTime.Now);
            }
        }

        public List<InformationExamination> ReturnResultTest()
        {
            using (CP24Team08Entities db = new CP24Team08Entities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.InformationExaminations.Where(a => a.ResultCTMau == true || a.ResultSHM == true || a.ResultDMau == true || a.ResultNhomMau == true || a.ResultNuocTieu == true || a.ResultMienDich == true || a.ResultDichChocDo == true || a.ResultViSinh == true).OrderByDescending(a => a.DateExamine).ToList();
            }
        }
    }
}