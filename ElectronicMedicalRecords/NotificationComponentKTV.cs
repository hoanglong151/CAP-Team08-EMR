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
    public class NotificationComponentKTV
    {
        public void RegisterNotificationKTV()
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [ResultCTMau],[ResultSHM],[ResultDMau],[ResultNhomMau],[ResultNuocTieu],[ResultMienDich],[ResultDichChocDo],[ResultViSinh] from [dbo].[InformationExamination]";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                //cmd.Parameters.AddWithValue("@DateExamine", currentTime);
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChangeKTV;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChangeKTV(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChangeKTV;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                RegisterNotificationKTV();
            }
        }

        public List<InformationExamination> GetInformationExamination()
        {
            using (CP24Team08Entities db = new CP24Team08Entities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.InformationExaminations.Where(a => a.ResultCTMau == false || a.ResultSHM == false || a.ResultDMau == false || a.ResultNhomMau == false || a.ResultNuocTieu == false || a.ResultMienDich == false || a.ResultDichChocDo == false || a.ResultViSinh == false).OrderByDescending(a => a.DateEnd).ToList();
            }
        }
    }
}