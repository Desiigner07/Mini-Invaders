using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
	class Schuss
	{
		public Point Ort { get; private set; }
		public static Size SchussGröße { get; private set; }
		public Richtung Richtung { get; private set; }
		public const int SchussPixelProSek = 100;
		public Waffen Waffentyp { get; private set; }
		public Point ZielPoint { get; set; }
		public Invader ZielInvader { get; set; }

		private DateTime _zuletzteBewegt;

		public Schuss(Point ort, Richtung richtung, Waffen aktWaffe)
		{
			Waffentyp = aktWaffe;
			Ort = ort;
			Richtung = richtung;
			_zuletzteBewegt = DateTime.Now;
			if (aktWaffe == Waffen.Laser)
			{
				SchussGröße = new Size(2, 8);
			}
			if (aktWaffe == Waffen.Raketen)
			{
				SchussGröße = new Size(4, 16);
			}
			if (aktWaffe == Waffen.Lenkrakete)
			{
				SchussGröße = new Size(3, 12);
			}
		}

		public void Bewegen()
		{
			TimeSpan zeitSetZuletztBewegt = DateTime.Now - _zuletzteBewegt;
			double weite = zeitSetZuletztBewegt.Milliseconds * SchussPixelProSek / 500;
			if (Richtung == Richtung.Hoch)
			{
				weite *= -1;
			}
			else
			{
				weite *= 1;
			}
			Ort = new Point(Ort.X, Ort.Y + weite);
			_zuletzteBewegt = DateTime.Now;
		}

		public void HyperSchallBewegen()
		{
			Ort = ZielPoint;
		}


		public void ZuZielBewegen()
		{
			TimeSpan zuletztBewegt = DateTime.Now - _zuletzteBewegt;
			double weite = zuletztBewegt.Milliseconds * SchussPixelProSek / 500;

			Ort = ZielPunktFinden();
			_zuletzteBewegt = DateTime.Now;
		}

		private Point ZielPunktFinden()
		{
			double widthX = ZielPoint.X - Ort.X;
			double widthY = Ort.Y - ZielPoint.Y;

			return new Point(Ort.X + (widthX / 8), Ort.Y - (widthY / 8));
		}

		public double Rotation()
		{
			double x = Ort.X - ZielPoint.X;
			double y = Ort.Y - ZielPoint.Y;
			double winkel = Math.Atan2(y, x);
			double angle = winkel * (180 / Math.PI);
			return angle;
		}
	}
}

