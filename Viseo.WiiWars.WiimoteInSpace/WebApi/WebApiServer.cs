using Microsoft.Owin.Hosting;
using System;
using System.Reflection;
using System.Windows;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi
{
    public class WebApiServer : IDisposable
    {
        private IDisposable _instance;

        public void Dispose()
        {
            if (_instance != null)
                _instance.Dispose();
        }

        public bool Start(string baseAddress)
        {
            // If this temporary code crashed due to 'Access Denied', you need to Run VS as Administrator.
            try
            {
                _instance = WebApp.Start<Startup>(url: baseAddress);
            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("To run the local web api, you must run the application as Administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
