using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using WiimoteLib;

namespace Viseo.WiiWars.ViewModel
{
	public class WiimoteControlViewModel : ViewModelBase
	{
		#region Properties
		private bool _buttonA;

		public bool ButtonA
		{
			get { return _buttonA; }
			set
			{
				if (_buttonA != value)
				{
					_buttonA = value;
					OnPropertyChanged();
				}
			}
		}


		private bool _buttonB;

		public bool ButtonB
		{
			get { return _buttonB; }
			set
			{
				if (_buttonB != value)
				{
					_buttonB = value;
					OnPropertyChanged();
				}
			}
		}


		private bool _buttonMinus;

		public bool ButtonMinus
		{
			get { return _buttonMinus; }
			set
			{
				if (_buttonMinus != value)
				{
					_buttonMinus = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonHome;

		public bool ButtonHome
		{
			get { return _buttonHome; }
			set
			{
				if (_buttonHome != value)
				{
					_buttonHome = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonPlus;

		public bool ButtonPlus
		{
			get { return _buttonPlus; }
			set
			{
				if (_buttonPlus != value)
				{
					_buttonPlus = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonOne;

		public bool ButtonOne
		{
			get { return _buttonOne; }
			set
			{
				if (_buttonOne != value)
				{
					_buttonOne = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonTwo;

		public bool ButtonTwo
		{
			get { return _buttonTwo; }
			set
			{
				if (_buttonTwo != value)
				{
					_buttonTwo = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonUp;

		public bool ButtonUp
		{
			get { return _buttonUp; }
			set
			{
				if (_buttonUp != value)
				{
					_buttonUp = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonDown;

		public bool ButtonDown
		{
			get { return _buttonDown; }
			set
			{
				if (_buttonDown != value)
				{
					_buttonDown = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonLeft;

		public bool ButtonLeft
		{
			get { return _buttonLeft; }
			set
			{
				if (_buttonLeft != value)
				{
					_buttonLeft = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _buttonRight;

		public bool ButtonRight
		{
			get { return _buttonRight; }
			set
			{
				if (_buttonRight != value)
				{
					_buttonRight = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _led1;

		public bool Led1
		{
			get { return _led1; }
			set
			{
				if (_led1 != value)
				{
					_led1 = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _led2;

		public bool Led2
		{
			get { return _led2; }
			set
			{
				if (_led2 != value)
				{
					_led2 = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _led3;

		public bool Led3
		{
			get { return _led3; }
			set
			{
				if (_led3 != value)
				{
					_led3 = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _led4;

		public bool Led4
		{
			get { return _led4; }
			set
			{
				if (_led4 != value)
				{
					_led4 = value;
					OnPropertyChanged();
				}
			}
		}


		private string _accelX;
		public string AccelX
		{
			get { return _accelX; }
			set
			{
				if (_accelX != value)
				{
					_accelX = value;
					OnPropertyChanged();
				}
			}
		}

		private string _accelY;
		public string AccelY
		{
			get { return _accelY; }
			set
			{
				if (_accelY != value)
				{
					_accelY = value;
					OnPropertyChanged();
				}
			}
		}

		private string _accelZ;
		public string AccelZ
		{
			get { return _accelZ; }
			set
			{
				if (_accelZ != value)
				{
					_accelZ = value;
					OnPropertyChanged();
				}
			}
		}

		private string _header;

		public string Header
		{
			get { return _header; }
			set
			{
				_header = value;
				OnPropertyChanged();
			}
		}

		private float _battery;

		public float Battery
		{
			get { return _battery; }
			set
			{
				if (_battery != value)
				{
					_battery = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _ir1Found;

		public bool IR1Found
		{
			get { return _ir1Found; }
			set
			{
				if (_ir1Found != value)
				{
					_ir1Found = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _ir2Found;

		public bool IR2Found
		{
			get { return _ir2Found; }
			set
			{
				if (_ir2Found != value)
				{
					_ir2Found = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _ir3Found;

		public bool IR3Found
		{
			get { return _ir3Found; }
			set
			{
				if (_ir3Found != value)
				{
					_ir3Found = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _ir4Found;

		public bool IR4Found
		{
			get { return _ir4Found; }
			set
			{
				if (_ir4Found != value)
				{
					_ir4Found = value;
					OnPropertyChanged();
				}
			}
		}

		private string _ir1;

		public string IR1
		{
			get { return _ir1; }
			set
			{
				if (_ir1 != value)
				{
					_ir1 = value;
					OnPropertyChanged();
				}
			}
		}

		private string _ir2;

		public string IR2
		{
			get { return _ir2; }
			set
			{
				if (_ir2 != value)
				{
					_ir2 = value;
					OnPropertyChanged();
				}
			}
		}

		private string _ir3;

		public string IR3
		{
			get { return _ir3; }
			set
			{
				if (_ir3 != value)
				{
					_ir3 = value;
					OnPropertyChanged();
				}
			}
		}

		private string _ir4;

		public string IR4
		{
			get { return _ir4; }
			set
			{
				if (_ir4 != value)
				{
					_ir4 = value;
					OnPropertyChanged();
				}
			}
		}

		private List<IRPlotViewModel> _irPlots = new List<IRPlotViewModel>()
		{
			new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Red) }, 
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Blue) }, 
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Yellow) },
            new IRPlotViewModel() { X = 0, Y = 0, Size = 0, Color = new SolidColorBrush(Colors.Orange) }
		};

		#endregion //Properties

		public WiimoteControlViewModel(Wiimote wm)
		{
			if (wm == null)
				throw new ArgumentNullException("wm");
			wm.WiimoteChanged += WiimoteChanged;
		}

		protected void WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
			var ws = e.WiimoteState;

			ButtonA = ws.ButtonState.A;
			ButtonB = ws.ButtonState.B;
			ButtonMinus = ws.ButtonState.Minus;
			ButtonHome = ws.ButtonState.Home;
			ButtonPlus = ws.ButtonState.Plus;
			ButtonOne = ws.ButtonState.One;
			ButtonTwo = ws.ButtonState.Two;
			ButtonUp = ws.ButtonState.Up;
			ButtonDown = ws.ButtonState.Down;
			ButtonLeft = ws.ButtonState.Left;
			ButtonRight = ws.ButtonState.Right;
			AccelX = String.Format(CultureInfo.CurrentCulture, "X={0:#.######}", ws.AccelState.Values.X);
			AccelY = String.Format(CultureInfo.CurrentCulture, "Y={0:#.######}", ws.AccelState.Values.Y);
			AccelZ = String.Format(CultureInfo.CurrentCulture, "Z={0:#.######}", ws.AccelState.Values.Z);
			Battery = ws.Battery;
			Led1 = ws.LEDState.LED1;
			Led2 = ws.LEDState.LED2;
			Led3 = ws.LEDState.LED3;
			Led4 = ws.LEDState.LED4;

			IR1Found = ws.IRState.IRSensors[0].Found;
			IR1 = String.Format(CultureInfo.CurrentCulture, "X={0:0000} Y={1:0000}", ws.IRState.IRSensors[0].RawPosition.X, ws.IRState.IRSensors[0].RawPosition.Y);
			_irPlots[0].SetFromIRSensor(ws.IRState.IRSensors[0]);

			IR2Found = ws.IRState.IRSensors[1].Found;
			IR2 = String.Format(CultureInfo.CurrentCulture, "X={0:0000} Y={1:0000}", ws.IRState.IRSensors[1].RawPosition.X, ws.IRState.IRSensors[1].RawPosition.Y);
			_irPlots[1].SetFromIRSensor(ws.IRState.IRSensors[1]);

			IR3Found = ws.IRState.IRSensors[2].Found;
			IR3 = String.Format(CultureInfo.CurrentCulture, "X={0:0000} Y={1:0000}", ws.IRState.IRSensors[2].RawPosition.X, ws.IRState.IRSensors[2].RawPosition.Y);
			_irPlots[2].SetFromIRSensor(ws.IRState.IRSensors[2]);

			IR4Found = ws.IRState.IRSensors[3].Found;
			IR4 = String.Format(CultureInfo.CurrentCulture, "X={0:0000} Y={1:0000}", ws.IRState.IRSensors[3].RawPosition.X, ws.IRState.IRSensors[3].RawPosition.Y);
			_irPlots[3].SetFromIRSensor(ws.IRState.IRSensors[3]);
        }
	}
	
}
