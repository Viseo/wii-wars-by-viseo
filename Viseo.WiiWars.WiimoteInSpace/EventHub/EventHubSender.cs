using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Viseo.WiiWars.WiimoteInSpace;

namespace Viseo.WiiWars.EventHub
{
    public class EventHubSender : NotifierBase
    {
        private readonly EventHubClient Client;
        private List<Task> SendingTasks;
        private int _totalMsg;
        private int _msgSent;

        public string MsgStatus
        {
            get { return String.Format("{0} / {1}", _msgSent, _totalMsg); }
        }

        public EventHubSender(string eventHubName)
        {
            string connectionString = GetServiceBusConnectionString();
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            EventHubDescription eventHubDescription = new EventHubDescription(eventHubName);
            eventHubDescription.PartitionCount = 8;
            
            namespaceManager.CreateEventHubIfNotExistsAsync(eventHubDescription).Wait();

            Client = EventHubClient.Create(eventHubName);
            SendingTasks = new List<Task>();
        }

        public void Send(object value)
        {
            Send(value, null);
        }

        public void Send(object value, string partitionKey)
        {
            var serializedString = JsonConvert.SerializeObject(value);
            using (var data = new EventData(Encoding.UTF8.GetBytes(serializedString)))
            {
                if (partitionKey != null)
                    data.PartitionKey = partitionKey;

                // Set user properties if needed
                data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());

                // Send the metric to Event Hub
                _totalMsg++;
                SendingTasks.Add(Client.SendAsync(data).ContinueWith((prevTask) =>
                {
                    _msgSent++;
                    OnPropertyChanged("MsgStatus");
                }));
            }
        }
        
        public void Close()
        {
            //ain't nobody got time for that
            //Task.WaitAll(SendingTasks.ToArray());
            Client.CloseAsync().Wait();
        }

        private static string GetServiceBusConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
                return string.Empty;
            }
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
            builder.TransportType = TransportType.Amqp;
            return builder.ToString();
        }
    }
}
