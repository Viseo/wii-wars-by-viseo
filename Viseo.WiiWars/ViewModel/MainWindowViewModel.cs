using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WiimoteLib;

namespace Viseo.WiiWars.ViewModel
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public sealed class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private const string EVENT_HUB_NAME = "viseo-wii-wars-dev-noeu-eventhub";
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private readonly SynchronizationContext _synchronizationContext;
        private List<FilterInfo> _devices;
        private FilterInfo _selectedDevice;
        private BitmapImage _currentImage;
        private BitmapImage _videoButtonImage;
        private ICommand _startStopSendingToAzureCommand;
        private ICommand _startStopVideoFeedCommand;

        private ObservableCollection<WiimoteControlViewModel> _wiimoteTabs;

        public ObservableCollection<WiimoteControlViewModel> WiimoteTabs
        {
            get { return _wiimoteTabs; }
            private set
            {
                _wiimoteTabs = value;
                OnPropertyChanged();
            }
        }

        private WiimoteControlViewModel _selectedWiimoteTab;

        public WiimoteControlViewModel SelectedWiimoteTab
        {
            get { return _selectedWiimoteTab; }
            set
            {
                _selectedWiimoteTab = value;
                OnPropertyChanged();
            }
        }


        public WiimoteCollection WiimoteCollection { get; private set; }

        /// <summary>
        /// Installed camera
        /// </summary>
        public ICollection<FilterInfo> Devices
        {
            get { return _devices; }
            private set
            {
                _devices = (List<FilterInfo>)value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected camera
        /// </summary>
        public FilterInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Current Image
        /// </summary>
        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage VideoButtonImage
        {
            get { return _videoButtonImage; }
            set
            {
                _videoButtonImage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ARObjectViewModel> _augmentedRealityObjects;

        public ObservableCollection<ARObjectViewModel> AugmentedRealityObjects
        {
            get { return _augmentedRealityObjects; }
            private set
            {
                _augmentedRealityObjects = value;
                OnPropertyChanged();
            }
        }


        public ICommand StartStopSendingToAzureCommand
        {
            get { return _startStopSendingToAzureCommand ?? (_startStopSendingToAzureCommand = new RelayCommand(StartStopSendingToAzure)); }
        }

        public ICommand StartStopVideoFeedCommand
        {
            get { return _startStopVideoFeedCommand ?? (_startStopVideoFeedCommand = new RelayCommand(StartStopVideoFeed)); }
        }

        private void StartStopVideoFeed()
        {
            if (SelectedDevice == null)
                return;

            if (_videoSource != null && _videoSource.IsRunning)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        private void Start()
        {
            CloseVideoSource();
            VideoButtonImage = Application.Current.Resources["StopImage"] as BitmapImage;

            _videoSource = new VideoCaptureDevice(SelectedDevice.MonikerString);
            _videoSource.NewFrame += OnNewFrameReceived;
            _videoSource.Start();
        }

        private void OnNewFrameReceived(object sender, NewFrameEventArgs eventArgs)
        {
            var img = (Bitmap)eventArgs.Frame.Clone();

            foreach (var obj in AugmentedRealityObjects)
            {
                if (obj.SpaceWidth != img.Width)
                    obj.SpaceWidth = img.Width;
                if (obj.SpaceHeight != img.Height)
                    obj.SpaceHeight = img.Height;
            }

            _synchronizationContext.Post(o =>
            {
                CurrentImage = BitmapConverter.ToBitmapImage(img);
            }, null);
        }

        private void Stop()
        {
            CloseVideoSource();
            VideoButtonImage = Application.Current.Resources["StartImage"] as BitmapImage;
            CurrentImage = null;
        }

        private void CloseVideoSource()
        {
            if (_videoSource == null) return;
            if (!_videoSource.IsRunning) return;
            _videoSource.SignalToStop();
            _synchronizationContext.Post(o =>
            {
                CurrentImage = null;
            }, null);
        }

        private void StartStopSendingToAzure()
        {

        }

        public MainWindowViewModel()
        {
            _synchronizationContext = SynchronizationContext.Current;
            AugmentedRealityObjects = new ObservableCollection<ARObjectViewModel>();
            VideoButtonImage = Application.Current.Resources["StartImage"] as BitmapImage;

            InitializeWebCamList();
            InitializeWiimoteList();

        }


        private void InitializeWebCamList()
        {
            SelectedDevice = null;
            Devices = null;

            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0)
                return;

            Devices = _videoDevices.Cast<FilterInfo>().ToList();
            SelectedDevice = Devices.First();
            //StartStopVideoFeed();
        }

        /// <summary>
        /// Determines the number of wiimotes and initializes them, if any. Throughs an exception if no devices.
        /// </summary>
		private void InitializeWiimoteList()
        {
            WiimoteTabs = null;
            WiimoteCollection = new WiimoteCollection();
            WiimoteTabs = new ObservableCollection<WiimoteControlViewModel>();
            int index = 1;

            if (WiimoteCollection.Count == 0)
            {
                MessageBox.Show("DEBUG_VTH: Wiimote not found.",
                    Properties.Resources.WiimoteNotFound, MessageBoxButton.OK, MessageBoxImage.Error);

                // No wiimote => As a PoC, sends dummy elements into the Azure Eventhub.
                Random random = new Random();
                const int DEVICE_A = 1;

                SendEventsToEventHub(
                    DEVICE_A,
                    random.Next(6514), // X
                    random.Next(512), // Y
                    random.Next(458), // Z
                    random.Next(8236), // Rotation A
                    random.Next(751)); // Rotation B
            }
            else
            {
                try
                {
                    WiimoteCollection.FindAllWiimotes();
                }
                catch (WiimoteNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.WiimoteNotFound, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (WiimoteException ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.WiimoteError, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                foreach (Wiimote wm in WiimoteCollection)
                {
                    var tab = new WiimoteControlViewModel(wm) { Header = "Wiimote " + index };
                    WiimoteTabs.Add(tab);

                    wm.Connect();
                    wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);
                    wm.SetLEDs(index++);

                    var saber = new LightSaber(wm);

                    AugmentedRealityObjects.Add(saber);

                    //wm.WiimoteChanged += Wm_WiimoteChanged;
                }
            }
        }

        private void SendEventsToEventHub(int deviceId, int x, int y, int z, int rotationA, int rotationB)
        {
            string connectionString = GetServiceBusConnectionString();
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            EventHubDescription eventHubDescription = new EventHubDescription(EVENT_HUB_NAME);
            eventHubDescription.PartitionCount = 8;
            namespaceManager.CreateEventHubIfNotExistsAsync(eventHubDescription).Wait();

            // Create EventHubClient
            EventHubClient client = EventHubClient.Create(EVENT_HUB_NAME);

            const int DEVICE_CONSIDERED = 5;
            const int NUMBER_OF_MESSAGES = 50;

            try
            {
                List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>();

                // Send messages to Event Hub
                Console.WriteLine("Sending messages to Event Hub {0}", client.Path);
                Random random = new Random();
                for (int i = 0; i < NUMBER_OF_MESSAGES; ++i)
                {
                    string myStringData = string.Format("Into Eventhub {0}, {1}, {2}, {3}, {4}, {5}",
                            deviceId, x, y*random.Next(6565161), z*random.Next(6161), rotationA, rotationB);

                    // Create the device/temperature metric
                    var serializedString = JsonConvert.SerializeObject(myStringData);
                    EventData data = new EventData(System.Text.Encoding.UTF8.GetBytes(serializedString))
                    {
                        PartitionKey = DEVICE_CONSIDERED.ToString()
                    };

                    // Set user properties if needed
                    data.Properties.Add("Type", "Telemetry_" + DateTime.Now.ToLongTimeString());

                    // Send the metric to Event Hub
                    tasks.Add(client.SendAsync(data));
                }

                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error on send: " + exp.Message);
            }

            client.CloseAsync().Wait();
        }

        private EventHub.EventHubSender Sender { get; set; }

        private void Wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            if (Sender == null)
                Sender = new EventHub.EventHubSender(ConfigurationManager.AppSettings["EventHub.Namespace"]);

            Sender.Send(e.WiimoteState, ((Wiimote)sender).ID.ToString());
        }

        public void Dispose()
        {
            foreach (Wiimote wm in WiimoteCollection)
                wm.Disconnect();
            GC.SuppressFinalize(this);
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
