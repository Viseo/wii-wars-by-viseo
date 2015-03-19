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

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
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


		private Point3D[] _ledPositions = new Point3D[4]
		{
			new Point3D(2.5, 0.5, 1.5),
			new Point3D(2.5, 0.5, -1),
			new Point3D(3.5, 0.5, -2.5),
			new Point3D(-3.5, 0.5, -2.5)
		};

		public MainWindowViewModel()
		{
			testSolvePnP();

			return;

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

			if (sensors.All(s => s.Found == true))
			{
				double[] rvec, tvec;
				var objectPoints = _ledPositions.ToPoint3f().ToArray();
				var imagePoints = sensors.Select(s => s.RawPosition).ToPoint2f().ToArray();
				var intrinsic = new double[3, 3] { { _fx, 0, _cx }, { 0, _fy, _cy }, { 0, 0, 1 } };

				Cv2.SolvePnP(objectPoints, //led absolute position
							 imagePoints, //positions from camera
							 intrinsic,
							 null, out rvec, out tvec);
			}

		}

		private void testSolvePnP()
		{
			double[] rvec, tvec;

			//var objectPoints = _ledPositions.ToPoint3f().ToArray();
			//var imagePoints = sensors.Select(s => s.RawPosition).ToPoint2f().ToArray();
			var intrinsic = new double[3, 3] { { _fx, 0, _cx }, { 0, _fy, _cy }, { 0, 0, 1 } };
			//var rvec = new double[3];
			//var tvec = new double[3];

			var objectPoints = new OpenCvSharp.CPlusPlus.Point3f[4]
			{
					new Point3f(2.5f, 0.5f, 1.5f),
					new Point3f(2.5f, 0.5f, -1f),
					new Point3f(3.5f, 0.5f, -2.5f),
					new Point3f(-3.5f, 0.5f, -2.5f)
			};

			var imagePoints = new OpenCvSharp.CPlusPlus.Point2f[4]
			{
					new Point2f(297, 369),
					new Point2f(294, 198),
					new Point2f(195, 106),
					new Point2f(746, 44)
			};

			//var iaObjectPoints = InputArray.Create<Point3f>(objectPoints);
			//var iaImagePoints = InputArray.Create<Point2f>(imagePoints);
			//var iaIntrinsic = InputArray.Create<double>(intrinsic);
			//var oaRvec = OutputArray.Create<double>(new List<double>());
			//var oaTvec = OutputArray.Create<double>(new List<double>());
			//var iaDistCoeff = InputArray.Create(new Mat());

			Cv2.SolvePnP(objectPoints, //led absolute position
						 imagePoints, //positions from camera
						 intrinsic,
						 null, out rvec, out tvec, false, SolvePnPFlag.EPNP);

			//Cv2.SolvePnP(iaObjectPoints, iaImagePoints, iaIntrinsic, iaDistCoeff, oaRvec, oaTvec);
			//NativeMethods.calib3d_solvePnP_vector(objectPoints.ToArray(),
			//									  objectPoints.Length,
			//									  imagePoints,
			//									  imagePoints.Length,
			//									  intrinsic,
			//									  new double[0],
			//									  0,
			//									  rvec,
			//									  tvec,
			//									  0,
			//									  (int)SolvePnPFlag.Iterative);
		}

		private void InitializeIRBeaconModel()
		{
			var transform = new TranslateTransform3D(0, 0, 4);

			var modelGroup = new Model3DGroup();
			var meshBuilder = new MeshBuilder();
			meshBuilder.AddBox(new Point3D(0, 0, 0), 8, 0.5, 6);

			var mesh = meshBuilder.ToMesh(true);

			var cardMaterial = MaterialHelper.CreateMaterial(Colors.WhiteSmoke);
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = cardMaterial, BackMaterial = cardMaterial , Transform = transform});

			meshBuilder = new MeshBuilder();
			foreach (var led in _ledPositions)
				meshBuilder.AddSphere(led, 0.2);
			mesh = meshBuilder.ToMesh(true);
			var ledMaterial = MaterialHelper.CreateMaterial(Colors.SteelBlue);
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = ledMaterial, BackMaterial = ledMaterial, Transform = transform });

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
			meshBuilder.AddBox(new Point3D(0, 0, 5), 3.5, 14.5, 3);

			var mesh = meshBuilder.ToMesh(true);

			var whiteMaterial = MaterialHelper.CreateMaterial(Colors.White);
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = whiteMaterial, BackMaterial = whiteMaterial });

			Wiimote = modelGroup;
		}
	}
}
