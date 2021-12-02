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
    public class NotificationComponentBS
    {
        public void RegisterNotificationBS()
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
                //cmd.Parameters.AddWithValue("@DateEnd", currentTime);
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChangeBS;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChangeBS(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChangeBS;

                var notificationHubBS = GlobalHost.ConnectionManager.GetHubContext<NotificationHubBS>();
                notificationHubBS.Clients.All.notifyResult("added");

                RegisterNotificationBS();
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