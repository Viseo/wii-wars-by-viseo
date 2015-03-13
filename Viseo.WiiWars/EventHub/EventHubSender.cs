using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.EventHub
{
    public class EventHubSender
    {
        private readonly EventHubClient Client;
        private List<Task> SendingTasks;

        public EventHubSender(string eventHubName)
        {
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
				SendingTasks.Add(Client.SendAsync(data));
			}
        }

        public void Close()
        {
            Task.WaitAll(SendingTasks.ToArray());
            Client.CloseAsync().Wait();
        }

    }
}
