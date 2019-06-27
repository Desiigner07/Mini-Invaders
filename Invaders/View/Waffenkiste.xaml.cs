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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Invaders.View
{
	public sealed partial class Waffenkiste : UserControl
	{
		public Waffenkiste(double von)
		{
			this.InitializeComponent();
		}

		public void Fallen(double von)
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = von;
			animation.To = 800;
			animation.Duration = TimeSpan.FromSeconds(3);
			Storyboard.SetTarget(animation, kiste);
			Storyboard.SetTargetProperty(animation, "(Canvas.Top)");
			story.Children.Add(animation);
			story.Begin();
		}
	}
}
