using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Viseo.WiiWars.ViewModel
{
	public static class BitmapConverter
	{
		public static BitmapImage ToBitmapImage(Image bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");
			var bitmapImage = new BitmapImage();
			using (var mem = new MemoryStream())
			{
				bitmap.Save(mem, ImageFormat.Jpeg);
				mem.Position = 0;

				bitmapImage.BeginInit();
				bitmapImage.StreamSource = mem;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
			}
			return bitmapImage;
		}
		/// <summary>
		/// Converti un BitmapImage en Bitmap Jpeg. Le qualityLevel permet d'ajuster le taux de compression Jpeg
		/// </summary>
		public static Bitmap ToJpegBitmap(BitmapImage bitmapImage)
		{
			return ToJpegBitmap(bitmapImage, 70);
        }

		public static Bitmap ToJpegBitmap(BitmapImage bitmapImage, int qualityLevel)
		{
			if (bitmapImage == null)
				throw new ArgumentNullException("bitmapImage");
			using (var mem = new MemoryStream())
			{
				var encoder = new JpegBitmapEncoder { QualityLevel = qualityLevel };
				encoder.Frames.Add(BitmapFrame.Create(bitmapImage.Clone()));
				encoder.Save(mem);
				var bmp = new Bitmap(mem);
				return new Bitmap(bmp);
			}
		}
	}
}
