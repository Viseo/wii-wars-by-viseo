using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using WiimoteLib;

namespace Viseo.WiiWars.ViewModel
{
	public class LightSaber : ARObjectViewModel
	{
		private static readonly List<string> _images =
			new List<string>() { "SaberBlue", "SaberGreen", "SaberRed", "SaberViolet" };
		private const string _saberOff = "SaberOff";
		private static readonly Random _rand = new Random();

		private bool _lastButtonAState;
		public bool Powered { get; set; }
		public string SaberImageName { get; set; }

		public LightSaber(Wiimote wm) : base(wm)
		{
			SaberImageName = _images[_rand.Next(_images.Count)];
            Image = Application.Current.Resources[_saberOff] as BitmapImage;
			X = 0;
			Y = 0;
			Width = Image.Width / 2;
			Height = Image.Height / 2;
			Powered = false;
            wm.WiimoteChanged += WiimoteChanged;
        }

		protected override void WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
			base.WiimoteChanged(sender, e);

			if (_lastButtonAState == true && e.WiimoteState.ButtonState.A == false)
			{
				Powered = !Powered;
				Image = Powered ?	Application.Current.Resources[SaberImageName] as BitmapImage :
									Application.Current.Resources[_saberOff] as BitmapImage;
			}
			_lastButtonAState = e.WiimoteState.ButtonState.A;
		}
	}
}
