using Invaders.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace Invaders.View
{
	public sealed partial class AnimiertesBild : UserControl
	{
		public AnimiertesBild()
		{
			this.InitializeComponent();
		}

		public AnimiertesBild(string bild) : this()
		{
			AnimationStarten(bild);
		}

    public void AnimationStarten(string bildname)
		{
			image.Source = (BildAusAssetErstellen(bildname));
		}

		private BitmapImage BildAusAssetErstellen(string bildname)
		{
			return new BitmapImage(new Uri("ms-appx:///Assets/" + bildname));
		}

		public void InvaderAnim(string axis, TimeSpan dur,double von,  double nach)
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = von;
			animation.To = nach;
			animation.Duration = dur;
			animation.RepeatBehavior = RepeatBehavior.Forever;
			animation.AutoReverse = true;
			Storyboard.SetTarget(animation, image);
			Storyboard.SetTargetProperty(animation, "(UIElement.Projection).(PlaneProjection.Rotation" + axis + ")");
			story.Children.Add(animation);
			story.Begin();
		}

		public void InvaderGetroffen()
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = 1;
			animation.To = 0;
			animation.Duration = TimeSpan.FromSeconds(1);
			Storyboard.SetTarget(animation, image);
			Storyboard.SetTargetProperty(animation, "(Opacity)");
			story.Children.Add(animation);
			story.Begin();
		}

		public void SpielerGetroffen()
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = 1;
			animation.To = 0;
			animation.Duration = TimeSpan.FromSeconds(0.3);
			animation.AutoReverse = true;
			animation.RepeatBehavior = new RepeatBehavior(5);
			Storyboard.SetTarget(animation, image);
			Storyboard.SetTargetProperty(animation, "(Opacity)");
			story.Children.Add(animation);
			story.Begin();
		}

		public void SturzflugAnimation(double y)
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animationY = new DoubleAnimation();
			animationY.From = y;
			animationY.To = 900;
			animationY.Duration = TimeSpan.FromSeconds(1);
			Storyboard.SetTarget(animationY, image);
			Storyboard.SetTargetProperty(animationY, "(Canvas.Top)");
		}
	
	}
}
