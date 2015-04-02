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

            var vm = DataContext as MainWindowViewModel;
            if (vm == null)
                return;

            vm.GetViewPortImage += GetViewPortImage;
        }

        private void GetViewPortImage(object sender, NotificationEventArgs<int, BitmapSource> e)
        {
            int width = e.Data;
            double scale = width / ARViewport.ActualWidth;
            int height = (int)(ARViewport.ActualHeight * scale);

            BitmapSource bmp = null;
            Dispatcher.Invoke(() =>
            {
                bmp = ARViewport.Viewport.RenderBitmap(width, height, Brushes.Transparent);
            });
            e.Completed(bmp);
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Dispose();
        }

        private void ARViewport_CameraChanged(object sender, RoutedEventArgs e)
        {
            CameraHelper.Copy(ARViewport.Camera, DebugViewport.Camera);
        }

        private void DebugViewport_CameraChanged(object sender, RoutedEventArgs e)
        {
            CameraHelper.Copy(DebugViewport.Camera, ARViewport.Camera);
        }
    }
}
