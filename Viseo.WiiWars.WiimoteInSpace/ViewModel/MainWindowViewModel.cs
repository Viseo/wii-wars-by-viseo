﻿using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WiimoteLib;
using OpenCvSharp.CPlusPlus;
using Viseo.WiiWars.WiimoteInSpace.Helper;
using System.Reflection;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        #region Properties
        private readonly Dispatcher dispatcher;

        private Model3D _wiimote;

        public Model3D Wiimote
        {
            get { return _wiimote; }
            set
            {
                _wiimote = value;
                OnPropertyChanged();
            }
        }

        private Model3D _lightSaber;

        public Model3D LightSaber
        {
            get { return _lightSaber; }
            set
            {
                _lightSaber = value;
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



        #endregion

        public MainWindowViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            InitializeWiimote();
            InitializeWiimoteModel();
            //InitializeLightSaberModel();
            InitializeIRBeaconModel();
        }

        private void InitializeWiimote()
        {
            var WiimoteCollection = new WiimoteCollection();
            int index = 1;

            try
            {
                WiimoteCollection.FindAllWiimotes();
            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "WiimoteNotFound", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "WiimoteError", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach (Wiimote wm in WiimoteCollection)
            {
                wm.Connect();
                wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);
                wm.SetLEDs(index++);

                wm.WiimoteChanged += Wm_WiimoteChanged;
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
                var rvec = new MatOfDouble(3, 1);
                var tvec = new MatOfDouble(3, 1);

                var objectPoints = _irPlots.Select(i => i.Point.ToPoint3f()).ToArray();

                var intrinsic = new Mat(3, 3, MatType.CV_64F, new double[] { _fx, 0, _cx, 0, _fy, _cy, 0, 0, 1 });

                try
                {
                    Cv2.SolvePnP(InputArray.Create(objectPoints), //led absolute position
                                 InputArray.Create(imagePoints), //positions from camera
                                 intrinsic,
                                 InputArray.Create(new Mat()), rvec, tvec);
                }
                catch (OpenCvSharp.OpenCVException)
                {
                    //log
                    return;
                }


                var rvecIdxer = rvec.GetIndexer();
                var tvecIdxer = tvec.GetIndexer();

                TranslateX = tvecIdxer[0];
                TranslateY = tvecIdxer[1];
                TranslateZ = tvecIdxer[2];

                RotX = rvecIdxer[0] * (180 / Math.PI);
                RotY = rvecIdxer[1] * (180 / Math.PI);
                RotZ = rvecIdxer[2] * (180 / Math.PI);

            }
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
            LightSaber = await LoadAsync("Assets/LightSaber/LightSaber.obj");
            Material sabreMat = MaterialHelper.CreateImageMaterial("Assets/LightSaber/sabre.png", 1);
            ((GeometryModel3D)((Model3DGroup)LightSaber).Children[1]).Material = sabreMat;
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

            var whiteMaterial = MaterialHelper.CreateMaterial(Colors.White);
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = whiteMaterial, BackMaterial = whiteMaterial });

            Wiimote = modelGroup;
        }

        public static string GetKnownColorName(Color clr)
        {
            Color clrKnownColor;

            //Use reflection to get all known colors
            Type ColorType = typeof(System.Windows.Media.Colors);
            PropertyInfo[] arrPiColors = ColorType.GetProperties(BindingFlags.Public | BindingFlags.Static);

            //Iterate over all known colors, convert each to a <Color> and then compare
            //that color to the passed color.
            foreach (PropertyInfo pi in arrPiColors)
            {
                clrKnownColor = (Color)pi.GetValue(null, null);
                if (clrKnownColor == clr) return pi.Name;
            }

            return string.Empty;
        }

    }
}
