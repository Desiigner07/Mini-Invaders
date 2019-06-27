using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Invaders.View
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class InvadersView : Page
	{
		public InvadersView()
		{
			this.InitializeComponent();
		}

		private void spielfeld_Loaded(object sender, RoutedEventArgs e)
		{
			SpielfeldGrößeAktualisieren(spielfeld.RenderSize);
		}

		private void SpielfeldGrößeAktualisieren(Size size)
		{
			//double zielBreite;
			//double zielHöhe;
			//if (size.Width > size.Height)
			//{
			//	zielBreite = size.Height * 4 / 3;
			//	zielHöhe = size.Height;
			//	double rand = size.Width - zielBreite / 2;
			//	spielfeld.Margin = new Thickness(rand, 0, rand, 0);
			//}
			//else
			//{
			//	zielHöhe = size.Width * 3 / 4;
			//	zielBreite = size.Width;
			//	double rand = size.Height - zielHöhe / 2;
			//	spielfeld.Margin = new Thickness(0, rand, 0, rand);
			//}
			//spielfeld.Height = zielHöhe;
			//spielfeld.Width = zielBreite;

		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SpielfeldGrößeAktualisieren(e.NewSize);
			viewModel.SpielfeldGröße = e.NewSize;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
			Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
		}

		private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
		{
			viewModel.KeyUp(args.VirtualKey);
		}

		private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
		{
			viewModel.KeyDown(args.VirtualKey);
		}
	}
}
