using Microsoft.AspNet.SignalR.Client;
using System;
using System.Net.Http;
using System.Windows;
using Viseo.WiiWars.WiimoteInSpace.WebApi.Controllers;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi
{
    public class SignalRClient : IDisposable
    {
        private const string ServerURI = "http://localhost:39638/signalr";
        private const string HubName = "SaberHub";
        public HubConnection Connection { get; set; }
        public IHubProxy HubProxy { get; set; }

        private SaberController _saberController = new SaberController();

        public async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy(HubName);

            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<int>("TurnOn", (id) => _saberController.TurnOn(id));
            HubProxy.On<int>("TurnOff", (id) => _saberController.TurnOff(id));
            HubProxy.On<int>("ChangeColorGreen", (id) => _saberController.ChangeColorGreen(id));
            HubProxy.On<int>("ChangeColorRed", (id) => _saberController.ChangeColorRed(id));
            HubProxy.On<int>("ChangeColorBlue", (id) => _saberController.ChangeColorBlue(id));

            HubProxy.On<string>("hello", msg =>
              {
                  MessageBox.Show(msg, "Hello", MessageBoxButton.OK, MessageBoxImage.Information);
              });

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                //No connection
                return;
            }
        }

        private void Connection_Closed()
        {
            //event disconnected
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }
    }
}
