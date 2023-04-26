﻿namespace DemoApp
{
	using System.IO;
	using System.Windows;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using System.Windows.Media.Imaging;
	using System.Threading;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			await DownloadImage(this.Image1, "https://th-thumbnailer.cdn-si-edu.com/bZAar59Bdm95b057iESytYmmAjI=/1400x1050/filters:focal(594x274:595x275)/https://tf-cmsv2-smithsonianmag-media.s3.amazonaws.com/filer/95/db/95db799b-fddf-4fde-91f3-77024442b92d/egypt_kitty_social.jpg");
			await DownloadImage(this.Image2, "https://cdn.shopify.com/s/files/1/1832/0821/files/catshark.jpg?v=1649869148");
		}

		private async Task DownloadImage(Image image, string url)
		{
			var client = new HttpClient();
			var request = await client.GetAsync(url);
			await Task.Run(() => Thread.Sleep(2000));
			var byteData = await request.Content.ReadAsByteArrayAsync();
			image.Source = this.LoadImage(byteData);
		}

		private BitmapImage LoadImage(byte[] imageData)
		{
			if (imageData == null || imageData.Length == 0) return null;
			var image = new BitmapImage();
			using (var mem = new MemoryStream(imageData))
			{
				mem.Position = 0;
				image.BeginInit();
				image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.UriSource = null;
				image.StreamSource = mem;
				image.EndInit();
			}
			image.Freeze();
			return image;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{

		}
	}
}