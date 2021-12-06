using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicMedicalRecords
{
    public class NotificationHubOnlineUser : Hub
    {
        public void Status()
        {
            Clients.All.statusUser();
        }
    }
}