using System;
using System.Windows.Media.Imaging;
using WiimoteLib;

namespace Viseo.WiiWars.ViewModel
{
	public class ARObjectViewModel : ViewModelBase
	{
		private BitmapImage _image;

		public BitmapImage Image
		{
			get { return _image; }
			set
			{
				_image = value;
				OnPropertyChanged();
			}
		}

		private double _x;

		public double X
		{
			get { return _x; }
			set
			{
				_x = value;
				OnPropertyChanged();
			}
		}

		private double _y;

		public double Y
		{
			get { return _y; }
			set
			{
				_y = value;
				OnPropertyChanged();
			}
		}

		private double _width;

		public double Width
		{
			get { return _width; }
			set
			{
				_width = value;
				OnPropertyChanged();
			}
		}

		private double _height;

		public double Height
		{
			get { return _height; }
			set
			{
				_height = value;
				OnPropertyChanged();
			}
		}

		public double SpaceWidth { get; set; }
		public double SpaceHeight { get; set; }

		public ARObjectViewModel(Wiimote wm)
		{
			if (wm == null)
				throw new ArgumentNullException("wm");
			wm.WiimoteChanged += WiimoteChanged;
		}

		protected virtual void WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
            var ws = e.WiimoteState;
			int? sensorId = ws.IRState.IRSensors[0].Found ? 0 :
							ws.IRState.IRSensors[1].Found ? 1 :
							ws.IRState.IRSensors[2].Found ? 2 :
							ws.IRState.IRSensors[3].Found ? 3 : (int?)null;

			if (sensorId.HasValue)
			{
				var sensorX = ws.IRState.IRSensors[sensorId.Value].RawPosition.X;
				var sensorY = ws.IRState.IRSensors[sensorId.Value].RawPosition.Y;

				X = SpaceWidth - (sensorX * SpaceWidth) / 1024 - Image.Width / 2;
				Y = (sensorY * SpaceHeight) / 768 - Image.Height / 2;
			}
		}
	}
}
