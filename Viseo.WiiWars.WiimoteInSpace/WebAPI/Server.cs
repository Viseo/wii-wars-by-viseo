using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.WiimoteInSpace.WebAPI
{
    public class Server : IDisposable
    {
        private IDisposable _instance;

        public void Dispose()
        {
            _instance.Dispose();
        }

        public void Start()
        {
            _instance = WebApp.Start<Startup>(url: Properties.Resources.BaseAddress);
        }
    }
}
