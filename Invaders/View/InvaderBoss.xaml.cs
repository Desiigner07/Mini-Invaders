using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace Invaders.View
{
	public sealed partial class InvaderBoss : UserControl
	{
		public InvaderBoss()
		{
			this.InitializeComponent();
		}

		private IEnumerable<string> BildNamen()
		{
			List<string> bilder = new List<string>();
			bilder.Add("BossApplyDamage.png");
			bilder.Add("SpaceInvadersBoss.png");
			return bilder;
		}

		public void ApplyDamage()
		{
			Storyboard storyboard = new Storyboard();
			ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();
			Storyboard.SetTarget(animation, image);
			Storyboard.SetTargetProperty(animation, "Source");

			TimeSpan aktuellesInterval = TimeSpan.FromMilliseconds(0);
			foreach (string bildname in BildNamen())
			{
				ObjectKeyFrame keyFrame = new DiscreteObjectKeyFrame();
				keyFrame.Value = BildAusAssetErstellen(bildname);
				keyFrame.KeyTime = aktuellesInterval;
				animation.KeyFrames.Add(keyFrame);
				aktuellesInterval = TimeSpan.FromMilliseconds(150);
			}
			storyboard.AutoReverse = false;
			storyboard.Children.Add(animation);
			storyboard.Begin();
		}

		public void BossStirbt()
		{
			Storyboard story = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = 1;
			animation.To = 0;
			animation.Duration = TimeSpan.FromSeconds(2);
			Storyboard.SetTarget(animation, image);
			Storyboard.SetTargetProperty(animation, "(Opacity)");
			story.Children.Add(animation);
			story.Begin();
		}

		private BitmapImage BildAusAssetErstellen(string bildname)
		{
			return new BitmapImage(new Uri("ms-appx:///Assets/" + bildname));
		}
	}
}
