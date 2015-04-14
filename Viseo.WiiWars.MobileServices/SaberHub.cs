using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.MobileServices
{
    public class SaberHub : Hub
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public string Send(string message)
        {
            return "Hello from SignalR Chat Hub!";
        }
    }
}
