using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WiimoteLib;
using OpenCvSharp.CPlusPlus;
using Viseo.WiiWars.WiimoteInSpace.Helper;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using System.Threading;
using AForge.Video;
using System.Drawing;
using Viseo.WiiWars.WiimoteInSpace.WebApi.Dal;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public sealed class MainWindowViewModel : NotifierBase, IMainWindowViewModel, IDisposable
    {
        #region Properties
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private readonly SynchronizationContext _synchronizationContext;
        private List<FilterInfo> _devices;
        private FilterInfo _selectedDevice;
        private BitmapImage _currentImage;
        private BitmapImage _videoButtonImage;
        private ICommand _startStopVideoFeedCommand;
        private bool _videoEnabled;

        public BitmapImage VideoButtonImage
        {
            get { return _videoButtonImage; }
            set
            {
                _videoButtonImage = value;
                OnPropertyChanged();
            }
        }

        public ICollection<FilterInfo> Devices
        {
            get { return _devices; }
            private set
            {
                _devices = (List<FilterInfo>)value;
                OnPropertyChanged();
            }
        }

        public FilterInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
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
            
            _videoSource = new VideoCaptureDevice(SelectedDevice.MonikerString);
            VideoButtonImage = Application.Current.Resources["StopImage"] as BitmapImage;
            _videoSource.NewFrame += OnNewFrameReceived;
            _videoSource.Start();
            _videoEnabled = true;
            SetModelAR(_saberRepository.Get(1));
        }

        private void OnNewFrameReceived(object sender, NewFrameEventArgs eventArgs)
        {
            var img = (Bitmap)eventArgs.Frame.Clone();
            //var width = img.Width;
            _synchronizationContext.Post(o =>
            {
                CurrentImage = BitmapConverter.ToBitmapImage(img);
            }, null);


            //var arImage = GetAugmentedImage(width);
            //_synchronizationContext.Post(o =>
            //{
            //    ARImage = arImage;
            //}, null);
        }

        private void Stop()
        {
            CloseVideoSource();
            VideoButtonImage = Application.Current.Resources["StartImage"] as BitmapImage;
            CurrentImage = null;
            _videoEnabled = false;
            SetModelAR(_saberRepository.Get(1));
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

        private void InitializeWebCamList()
        {
            SelectedDevice = null;
            Devices = null;

            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0)
                return;

            Devices = _videoDevices.Cast<FilterInfo>().ToList();
            SelectedDevice = Devices.First();
        }

        private BitmapSource _arImage;

        public BitmapSource ARImage
        {
            get { return _arImage; }
            set
            {
                _arImage = value;
                OnPropertyChanged();
            }
        }


        //Use for compositing
        //public event EventHandler<NotificationEventArgs<int, BitmapSource>> GetViewportImage;

        //private BitmapSource GetAugmentedImage(int width)
        //{
        //    if (GetViewportImage != null)
        //        GetViewportImage(this, new NotificationEventArgs<int, BitmapSource>(String.Empty, width, SetViewPortImage));

        //    return _viewPortImage;
        //}

        //BitmapSource _viewPortImage;

        //private void SetViewPortImage(BitmapSource image)
        //{
        //    _viewPortImage = image;
        //}


        private readonly Dispatcher dispatcher;

        private Model3D _wiimote;
        private Model3D _lightSaber;
        private Model3D _lightSaberOff;
        private Model3D _model;
        private Model3D _modelAR;

        public Model3D Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        public Model3D ModelAR
        {
            get { return _modelAR; }
            set
            {
                _modelAR = value;
                OnPropertyChanged();
            }
        }

        private Model3D _iRBeacon;

        public Model3D IRBeacon
        {
            get { return _iRBeacon; }
            set
            {
                _iRBeacon = value;
                OnPropertyChanged();
            }
        }

        private double _translateX;

        public double TranslateX
        {
            get { return _translateX; }
            set
            {
                _translateX = value;
                OnPropertyChanged();
            }
        }

        private double _translateY;

        public double TranslateY
        {
            get { return _translateY; }
            set
            {
                _translateY = value;
                OnPropertyChanged();
            }
        }

        private double _translateZ;

        public double TranslateZ
        {
            get { return _translateZ; }
            set
            {
                _translateZ = value;
                OnPropertyChanged();
            }
        }

        private double _rotX;

        public double RotX
        {
            get { return _rotX; }
            set
            {
                _rotX = value;
                OnPropertyChanged();
            }
        }

        private double _rotY;

        public double RotY
        {
            get { return _rotY; }
            set
            {
                _rotY = value;
                OnPropertyChanged();
            }
        }

        private double _rotZ;

        public double RotZ
        {
            get { return _rotZ; }
            set
            {
                _rotZ = value;
                OnPropertyChanged();
            }
        }

        private Point3D _rotCenter;

        public Point3D RotCenter
        {
            get { return _rotCenter; }
            set
            {
                _rotCenter = value;
                OnPropertyChanged();
            }
        }


        public WiimoteCollection WiimoteCollection { get; private set; }

        private List<IRPlotViewModel> _irPlots = new List<IRPlotViewModel>()
        {
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Blue), Point = new Point3D(-3.3, 0.5, -1.6) },
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Yellow), Point = new Point3D(-3.3, 0.5, 0.5) },
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Red), Point = new Point3D(2.8, 0.5, 1.6) },
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Green), Point = new Point3D(2.8, 0.5, -1.6) },
        };


        public ICollection<IRPlotViewModel> IRPlots
        {
            get { return _irPlots; }
        }

        private string _ir1;

        public string IR1
        {
            get { return _ir1; }
            set
            {
                _ir1 = value;
                OnPropertyChanged();
            }
        }

        private string _ir2;

        public string IR2
        {
            get { return _ir2; }
            set
            {
                _ir2 = value;
                OnPropertyChanged();
            }
        }

        private string _ir3;

        public string IR3
        {
            get { return _ir3; }
            set
            {
                _ir3 = value;
                OnPropertyChanged();
            }
        }

        private string _ir4;

        public string IR4
        {
            get { return _ir4; }
            set
            {
                _ir4 = value;
                OnPropertyChanged();
            }
        }

        public enum E3DModel
        {
            Wiimote,
            LightSaber
        }

        public static IEnumerable<E3DModel> Models
        {
            get { return Enum.GetValues(typeof(E3DModel)).Cast<E3DModel>(); }
        }

        private E3DModel _selectedModel;

        public E3DModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                OnPropertyChanged();

                switch (SelectedModel)
                {
                    case E3DModel.Wiimote:
                        Model = _wiimote;
                        RotCenter = new Point3D(0, 0, 0);
                        break;
                    case E3DModel.LightSaber:
                        Model = _lightSaber;
                        break;
                    default:
                        break;
                }
            }
        }



        #endregion

        private WebApi.WebApiServer _server;
        private SaberRepository _saberRepository;

        public MainWindowViewModel()
        {
            _server = new WebApi.WebApiServer();
            _server.Start();

            _saberRepository = SaberRepository.Instance;

            dispatcher = Dispatcher.CurrentDispatcher;
            _synchronizationContext = SynchronizationContext.Current;
            VideoButtonImage = Application.Current.Resources["StartImage"] as BitmapImage;

            InitializeWiimote();
            InitializeWiimoteModel();
            InitializeLightSaberModel();
            InitializeIRBeaconModel();
            InitializeWebCamList();

            SelectedModel = E3DModel.Wiimote;
        }

        private void InitializeWiimote()
        {
            WiimoteCollection = new WiimoteCollection();
            int index = 1;

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
                wm.Connect();
                wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);
                wm.SetLEDs(index++);

                wm.WiimoteChanged += Wm_WiimoteChanged;

                var saber = new Models.Saber() { Color = WiimoteInSpace.Models.Saber.SaberColor.Blue, IsOn = false };
                saber.PropertyChanged += Saber_PropertyChanged;
                _saberRepository.Add(saber);

            }
        }

        Dictionary<Models.Saber.SaberColor, Material> SaberMaterials = new Dictionary<WiimoteInSpace.Models.Saber.SaberColor, Material>()
        {
            { WiimoteInSpace.Models.Saber.SaberColor.Blue, MaterialHelper.CreateMaterial(Colors.Blue) },
            { WiimoteInSpace.Models.Saber.SaberColor.Green, MaterialHelper.CreateMaterial(Colors.Green) },
            { WiimoteInSpace.Models.Saber.SaberColor.Red, MaterialHelper.CreateMaterial(Colors.Red) },
        };

        private void Saber_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOn")
            {
                SetModelAR((Models.Saber)sender);
            }
            if (e.PropertyName == "Color")
            {
                _synchronizationContext.Post(o =>
                {
                    ((GeometryModel3D)((Model3DGroup)_lightSaber).Children[0]).Material = SaberMaterials[((Models.Saber)sender).Color];
                }, null);
            }
        }

        private void SetModelAR(Models.Saber saber)
        {
            if (!_videoEnabled)
                ModelAR = null;
            else
            {
                ModelAR = saber.IsOn ? _lightSaber : _lightSaberOff;
            }
        }


        #region Wiimote IR camera
        // Dimensions of the image taken by the IR camera
        private const int _imageWidth = 1024;
        private const int _imageHeight = 768;
        // Camera intrinsic parameters. These weren't obtained from calibration but works well enough.
        private const double _fx = 1700;
        private const double _fy = 1700;
        private const double _cx = _imageWidth / 2;
        private const double _cy = _imageHeight / 2;

        private Mat _intrinsic = new Mat(3, 3, MatType.CV_64F, new double[] { _fx, 0, _cx, 0, _fy, _cy, 0, 0, 1 });
        private bool _lastA = false;
        private bool _lastHome = false;

        private const int avgDepth = 10;
        private DoubleAverager _translateXAvg = new DoubleAverager(avgDepth);
        private DoubleAverager _translateYAvg = new DoubleAverager(avgDepth);
        private DoubleAverager _translateZAvg = new DoubleAverager(avgDepth);
        private DoubleAverager _rotXAvg = new DoubleAverager(avgDepth);
        private DoubleAverager _rotYAvg = new DoubleAverager(avgDepth);
        private DoubleAverager _rotZAvg = new DoubleAverager(avgDepth);

        #endregion

        private void Wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            var sensors = e.WiimoteState.IRState.IRSensors;

            IR1 = "IR1: " + sensors[0].RawPosition.ToString();
            IR2 = "IR2: " + sensors[1].RawPosition.ToString();
            IR3 = "IR3: " + sensors[2].RawPosition.ToString();
            IR4 = "IR4: " + sensors[3].RawPosition.ToString();

            //Order image points counter clockwise to match object points
            var imagePoints = Cv2.ConvexHull(sensors.Select(s => s.RawPosition).ToPoint2f().ToArray()).ToList();

            //Get the origin point with the shortest distance
            int shortestIdx = GetShortestDistanceIdx(imagePoints);

            //the origin point with the shortest distance to the next one is the first (idx 0)
            //auto rotate the idx found back to 0
            while (shortestIdx > 0)
            {
                imagePoints.RotateLeft();
                shortestIdx--;
            }

            //update ir plots position
            for (int i = 0; i < imagePoints.Count; i++)
                _irPlots[i].SetFromIRSensor(sensors.First(s => s.RawPosition.X == imagePoints[i].X &&
                                                               s.RawPosition.Y == imagePoints[i].Y));
            //if all sensors are found, update wiimote
            if (sensors.All(s => s.Found == true))
            {
                var objectPoints = _irPlots.Select(i => i.Point.ToPoint3f()).ToArray();

                using (var rvec = new MatOfDouble(3, 1))
                using (var tvec = new MatOfDouble(3, 1))
                using (var objectPointsArray = InputArray.Create(objectPoints))
                using (var imagePointsArray = InputArray.Create(imagePoints))
                using (var distCoeff = new Mat())
                using (var distCoeffArray = InputArray.Create(distCoeff))
                {
                    try
                    {
                        Cv2.SolvePnP(objectPointsArray, //led absolute position
                                     imagePointsArray, //positions from camera
                                     _intrinsic,
                                     distCoeffArray, rvec, tvec);
                    }
                    catch (OpenCvSharp.OpenCVException)
                    {
                        //log
                        return;
                    }

                    var rvecIdxer = rvec.GetIndexer();
                    var tvecIdxer = tvec.GetIndexer();

                    TranslateX = _translateXAvg.Update(tvecIdxer[0]);
                    TranslateY = _translateYAvg.Update(tvecIdxer[1]);
                    TranslateZ = _translateZAvg.Update(tvecIdxer[2]);

                    RotX = _rotXAvg.Update(rvecIdxer[0] * (180 / Math.PI));
                    RotY = _rotYAvg.Update(rvecIdxer[1] * (180 / Math.PI));
                    RotZ = _rotZAvg.Update(rvecIdxer[2] * (180 / Math.PI));
                }
            }

            if (_lastA == false && e.WiimoteState.ButtonState.A == true)
            {
                _saberRepository.Get(1).IsOn = !_saberRepository.Get(1).IsOn;
            }
            _lastA = e.WiimoteState.ButtonState.A;


            if (_lastHome == false && e.WiimoteState.ButtonState.Home == true)
            {
                //var transform = new Transform3DGroup();

                //transform.Children.Add(new TranslateTransform3D(TranslateX, TranslateY, TranslateZ));
                //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), RotX)));
                //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), RotY)));
                //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), RotZ)));

                //dispatcher.Invoke(() =>
                //{
                //    _lightSaber.Transform = transform;
                //    _lightSaberOff.Transform = transform;
                //});
            }
            _lastHome = e.WiimoteState.ButtonState.Home;
        }

        private static int GetShortestDistanceIdx(List<Point2f> imagePoints)
        {
            double shortestDist = double.MaxValue;
            int shortestIdx = -1;

            for (int idx = 0; idx < imagePoints.Count; idx++)
            {
                int nextIdx = (idx + 1 == imagePoints.Count) ? 0 : idx + 1;
                var dist = GetDistanceBetweenPoints(imagePoints[idx], imagePoints[nextIdx]);

                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    shortestIdx = idx;
                }
            }

            return shortestIdx;
        }

        public static double GetDistanceBetweenPoints(Point2f p, Point2f q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        private void InitializeIRBeaconModel()
        {
            var modelGroup = new Model3DGroup();
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Point3D(0, 0, 0), 8, 0.5, 6);

            var mesh = meshBuilder.ToMesh(true);

            var cardMaterial = MaterialHelper.CreateMaterial(Colors.WhiteSmoke);
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = cardMaterial, BackMaterial = cardMaterial });

            meshBuilder = new MeshBuilder();
            foreach (var ir in _irPlots)
                meshBuilder.AddSphere(ir.Point, 0.2);
            mesh = meshBuilder.ToMesh(true);
            var ledMaterial = MaterialHelper.CreateMaterial(Colors.SteelBlue);
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = ledMaterial, BackMaterial = ledMaterial });

            IRBeacon = modelGroup;
        }

        private async void InitializeLightSaberModel()
        {
            _lightSaber = await LoadAsync("Assets/LightSaber/LightSaber.obj");
            _lightSaberOff = await LoadAsync("Assets/LightSaber/LightSaberOff.obj");
            Material sabreMat = MaterialHelper.CreateImageMaterial("Assets/LightSaber/sabre.png", 1);
            ((GeometryModel3D)((Model3DGroup)_lightSaber).Children[1]).Material = sabreMat;
            ((GeometryModel3D)((Model3DGroup)_lightSaberOff).Children[0]).Material = sabreMat;

            var transform = new Transform3DGroup();
            //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));

            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));

            _lightSaber.Transform = transform;
            _lightSaberOff.Transform = transform;
        }


        private async Task<Model3DGroup> LoadAsync(string model3DPath)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new ModelImporter().Load(model3DPath, this.dispatcher);
            });
        }

        private void InitializeWiimoteModel()
        {
            var modelGroup = new Model3DGroup();
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Point3D(0, 0, 0), 3.5, 14.5, 3);

            var mesh = meshBuilder.ToMesh(true);

            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));

            var whiteMaterial = MaterialHelper.CreateMaterial(Colors.White);
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = whiteMaterial, BackMaterial = whiteMaterial, Transform = transform });
            
            _wiimote = modelGroup;
        }

        public void Dispose()
        {
            _intrinsic.Dispose();
            foreach (Wiimote wm in WiimoteCollection)
                wm.Disconnect();
            CloseVideoSource();
            _server.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
