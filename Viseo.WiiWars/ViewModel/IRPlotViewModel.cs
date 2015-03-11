using System.Windows.Media;
using WiimoteLib;

namespace Viseo.WiiWars.ViewModel
{
	public class IRPlotViewModel : ViewModelBase
	{
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

		private double _size;

		public double Size
		{
			get { return _size; }
			set
			{
				_size = value;
				OnPropertyChanged();
			}
		}

		public SolidColorBrush Color { get; set; }

		internal void SetFromIRSensor(IRSensor sensor)
		{
			X = sensor.RawPosition.X / 4;
			Y = sensor.RawPosition.Y / 4;
			Size = sensor.Found ? 5 : 0;
		}
	}
}
