using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viseo.WiiWars.ViewModel;

namespace Viseo.WiiWars
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Application.Current.Exit += Current_Exit;
		}

		private void Current_Exit(object sender, ExitEventArgs e)
		{
			((MainWindowViewModel)DataContext).Dispose();
		}
	}
}
