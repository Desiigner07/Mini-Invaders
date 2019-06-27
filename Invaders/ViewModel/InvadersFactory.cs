using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invaders.Model;
using Invaders.View;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Invaders.ViewModel
{
	class InvadersFactory
	{
		public static AnimiertesBild InvaderFactory(Invader invader)
		{
			if (invader.Invadertyp == InvaderTyp.Insekt)
			{
				AnimiertesBild bild = new AnimiertesBild("bug.png");
				bild.InvaderAnim("X", TimeSpan.FromSeconds(1), 0, 40);
				return bild;
			}
			if (invader.Invadertyp == InvaderTyp.Nuke)
			{
				AnimiertesBild bild = new AnimiertesBild("satellite.png");
				bild.InvaderAnim("Y", TimeSpan.FromSeconds(1.5),0, 60);
				return bild;
			}
			if (invader.Invadertyp == InvaderTyp.Stern)
			{
				AnimiertesBild bild = new AnimiertesBild("star.png");
				bild.InvaderAnim("Z", TimeSpan.FromSeconds(4),0, 360);
				return bild;
			}
			if (invader.Invadertyp == InvaderTyp.Untertasse)
			{
				AnimiertesBild bild = new AnimiertesBild("flyingsaucer.png");
				bild.InvaderAnim("Z", TimeSpan.FromSeconds(4), -20, 20);
				return bild;
			}
			else
			{
				AnimiertesBild bild = new AnimiertesBild("spaceship.png");
				return bild;
			}
		}

		public static View.InvaderBoss BossFactory()
		{
			View.InvaderBoss boss = new View.InvaderBoss();
			return boss;
		}

		public static AnimiertesBild SpielerControlFactory()
		{
			List<string> bildnamen = new List<string>();
			bildnamen.Add("player.png");
			AnimiertesBild bild = new AnimiertesBild("player.png");
			return bild;
		}

		public static void OrtSetzen(UIElement element, double x, double y)
		{
			Canvas.SetLeft(element, x);
			Canvas.SetTop(element, y);
		}

		public static void ElementBewegen(UIElement element, double zielX, double ziely, TimeSpan dur)
		{
			double startX = Canvas.GetLeft(element);   //Problem
			double startY = Canvas.GetTop(element);

			Storyboard story = new Storyboard();
			DoubleAnimation animationX = AnimationErstellen(startX, zielX, element, "(Canvas.Left)", dur);
			DoubleAnimation animationY = AnimationErstellen(startY, ziely, element, "(Canvas.Top)", dur);
			story.Children.Add(animationX);
			story.Children.Add(animationY);
			story.Begin();
		}

		private static DoubleAnimation AnimationErstellen(double von, double nach, UIElement element, string property, TimeSpan dur)
		{
			DoubleAnimation animation = new DoubleAnimation();
			animation.From = von;
			animation.To = nach;
			animation.Duration = dur;
			Storyboard.SetTarget(animation, element);
			Storyboard.SetTargetProperty(animation, property);

			return animation;
		}


		public static UIElement SternControlFactory()
		{
			Random random = new Random();
			int münze = random.Next(2);
			if (münze == 0)
			{
				Stern stern = new Stern(new SolidColorBrush(SternFarbe()));
				stern.Height = 10;
				stern.Width = 10;
				return stern;
			}
			else
			{
				Ellipse ellipse = new Ellipse();
				ellipse.Height = 5;
				ellipse.Width = 5;
				ellipse.Fill = new SolidColorBrush(SternFarbe());
				return ellipse;
			}
		}

		private static Color SternFarbe()
		{
			Random random = new Random();
			int zufall = random.Next(0, 8);
			switch (zufall)
			{
				case 0:
					return Colors.LightBlue;
				case 1:
					return Colors.Yellow;
				case 2:
					return Colors.Red;
				case 3:
					return Colors.White;
				case 4:
					return Colors.Snow;
				case 5:
					return Colors.Salmon;
				case 6:
					return Colors.Purple;
				case 7:
					return Colors.PaleGoldenrod;
				default:
					return Colors.Orange;
			}
		}

		public static Rectangle SchussFactory()
		{
			Rectangle rectangle = new Rectangle()
			{
				Height = 18,
				Width = 6,
				Fill = new SolidColorBrush(Colors.Red),
			};
			return rectangle;
		}

		public static Rectangle RaktenFactory()
		{
			Rectangle rectangle = new Rectangle()
			{
				Height = 36,
				Width = 12,
				StrokeThickness = 2,
				Stroke = new SolidColorBrush(Colors.Red),
				Fill = new SolidColorBrush(Colors.DimGray),
			};
			return rectangle;
		}

		public static Rectangle LenkRaketenFactory()
		{
			Rectangle rectangle = new Rectangle()
			{
				Height = 26,
				Width = 8,
				Fill = new SolidColorBrush(Colors.AntiqueWhite),
			};
			return rectangle;
		}

		public static Rectangle HyperschallraketenFactory()
		{
			Rectangle rectangle = new Rectangle()
			{
				Height = 6,
				Width = 20,
				Fill = new SolidColorBrush(Colors.Aquamarine),
				Stroke = new SolidColorBrush(Colors.AntiqueWhite),
				StrokeThickness = 1,
			};
			RotationSetzen(rectangle, 50);
			return rectangle;
		}

		public static void RotationSetzen(Rectangle rectangle, double rotation)
		{
			RotateTransform rot = new RotateTransform();
			rot.Angle = rotation + 90;
			rectangle.RenderTransform = rot;
		}
		
	}
}
