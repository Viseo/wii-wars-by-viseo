using System;
using System.Windows;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Viseo.WiiWars.WiimoteInSpace
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			/*
			//Viewport
			HelixViewport3D viewport = new HelixViewport3D();
			this.Container.Children.Add(viewport);

			//Camera
			viewport.DefaultCamera = new PerspectiveCamera();
			viewport.DefaultCamera.Position = new Point3D(100, 100, 100);
			viewport.DefaultCamera.LookDirection = new Vector3D(-100, -100, -100);
			viewport.DefaultCamera.UpDirection = new Vector3D(0, 0, 1);

			//Light
			DefaultLights light = new DefaultLights();
			viewport.Children.Add(light);

			//3D Grid
			viewport.Children.Add(new GridLinesVisual3D());

			//Sphere
			//SphereVisual3D sphere = new SphereVisual3D();
			//sphere.Radius = 5;
			//sphere.Center = new Point3D(5, 0, 0);
			//sphere.Fill = Brushes.Red;
			//viewport.Children.Add(sphere);

			//model
			//ModelVisual3D loadedModel = new ModelVisual3D();
			//viewport.Children.Add(loadedModel);

			//var mi = new ModelImporter();
			//Model3D currentModel = mi.Load("Assets/LightSaber/LightSaber.3ds");

			////Material m = MaterialHelper.CreateImageMaterial("Assets/LightSaber/sabre.png", 1);
			////((GeometryModel3D)currentModel).Material = m;
			////mi.Load("PATH\\TO\\MODEL.3ds", null, true);
			////((GeometryModel3D)model.Children[0]).Material = m;

			//loadedModel.Content = currentModel;

			//Wiimote
			var modelGroup = new Model3DGroup();
			var meshBuilder = new MeshBuilder();
			meshBuilder.AddBox(new Point3D(0, 0, 5), 3.5, 14.5, 3);

			var mesh = meshBuilder.ToMesh(true);

			var whiteMaterial = MaterialHelper.CreateMaterial(Colors.White);
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = whiteMaterial, BackMaterial = whiteMaterial });

			ModelVisual3D mv = new ModelVisual3D() { Content = modelGroup };
			
			viewport.Children.Add(mv);
			*/
		}
	}
}
