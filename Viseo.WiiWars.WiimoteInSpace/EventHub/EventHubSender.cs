//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Viseo.WiiWars.EventHub
//{
//    using Microsoft.ServiceBus;
//    using Microsoft.ServiceBus.Messaging;
//    using Newtonsoft.Json;
//    using System.Configuration;

//    public class EventHubSender
//    {
//        private const string EVENT_HUB_NAME = "viseo-wii-wars-dev-noeu-eventhub";

//        private readonly EventHubClient Client;
//        private List<Task> SendingTasks;

//        public EventHubSender(string eventHubName)
//        {
//            Client = EventHubClient.Create(eventHubName);
//            SendingTasks = new List<Task>();
//        }

//		public void Send(object value)
//		{
//			Send(value, null);
//		}

//		public void Send(object value, string partitionKey)
//        {
//            var serializedString = JsonConvert.SerializeObject(value);
//			using (var data = new EventData(Encoding.UTF8.GetBytes(serializedString)))
//			{
//				if (partitionKey != null)
//					data.PartitionKey = partitionKey;

//				// Set user properties if needed
//				data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());

//				// Send the metric to Event Hub
//				SendingTasks.Add(Client.SendAsync(data));
//			}
//        }

//        internal static void SendEventsToEventHub(int deviceId, int x, int y, int z, int rotationA, int rotationB)
//        {
//            string connectionString = GetServiceBusConnectionString();
//            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
//            EventHubDescription eventHubDescription = new EventHubDescription(EVENT_HUB_NAME);
//            eventHubDescription.PartitionCount = 8;

//            // {"The remote server returned an error: (401) Unauthorized. Manage claim is required for this operation. 
//            // TrackingId:ab2452dd-b79b-4e92-91f0-5171d4a5b5bf_G50,TimeStamp:3/24/2015 9:09:23 AM"}

//            namespaceManager.CreateEventHubIfNotExistsAsync(eventHubDescription).Wait();

//            // Create EventHubClient
//            EventHubClient client = EventHubClient.Create(EVENT_HUB_NAME);

//            const int PARTITION_KEY_CONSIDERED = 5;
//            const int NUMBER_OF_MESSAGES = 50;

//            try
//            {
//                List<Task> tasks = new List<Task>();

//                // Send messages to Event Hub
//                Console.WriteLine("Sending messages to Event Hub {0}", client.Path);
//                Random random = new Random();
//                for (int i = 0; i < NUMBER_OF_MESSAGES; ++i)
//                {
//                    string myStringData = string.Format("Into Eventhub {0}, {1}, {2}, {3}, {4}, {5}",
//                            deviceId, x, y * random.Next(6565161), z * random.Next(6161), rotationA, rotationB);

//                    // Create the device/temperature metric
//                    var serializedString = JsonConvert.SerializeObject(myStringData);
//                    EventData data = new EventData(System.Text.Encoding.UTF8.GetBytes(serializedString))
//                    {
//                        PartitionKey = PARTITION_KEY_CONSIDERED.ToString()
//                    };

//                    // Set user properties if needed
//                    data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());

//                    // Send the metric to Event Hub
//                    tasks.Add(client.SendAsync(data));
//                }

//                Task.WaitAll(tasks.ToArray());
//            }
//            catch (Exception exp)
//            {
//                Console.WriteLine("Error on send: " + exp.Message);
//            }

//            client.CloseAsync().Wait();
//        }

//        public void Close()
//        {
//            Task.WaitAll(SendingTasks.ToArray());
//            Client.CloseAsync().Wait();
//        }

//        private static string GetServiceBusConnectionString()
//        {
//            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
//            if (string.IsNullOrEmpty(connectionString))
//            {
//                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
//                return string.Empty;
//            }
//            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
//            builder.TransportType = TransportType.Amqp;
//            return builder.ToString();
//        }
//    }
//}
