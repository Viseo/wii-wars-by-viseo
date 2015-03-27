using System;
using System.Windows;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Viseo.WiiWars.WiimoteInSpace.ViewModel;

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
            Application.Current.Exit += Current_Exit;
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Dispose();
        }
    }
}
