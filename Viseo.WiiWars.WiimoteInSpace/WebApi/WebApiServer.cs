using Microsoft.Owin.Hosting;
using System;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi
{
    public class WebApiServer : IDisposable
    {
        private IDisposable _instance;

        public void Dispose()
        {
            _instance.Dispose();
        }

        public void Start()
        {
            // If this temporary code crashed due to 'Access Denied', you need to Run VS as Administrator.
            _instance = WebApp.Start<Startup>(url: Properties.Resources.BaseAddress);
        }
    }
}
