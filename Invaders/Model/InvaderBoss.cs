using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Invaders.Model
{
	class InvaderBoss : Schiff
	{
		public new Point Ort { get; private set; }
		public new Size Größe { get; private set; }

		public InvaderTyp InvaderTyp = InvaderTyp.Boss;
		public bool Schussbereit { get; private set; }

		private DispatcherTimer timer = new DispatcherTimer();

		public double Lebenspunkte = 100;

		public InvaderBoss(Point ort, Size größe) : base(ort, größe)
		{
			Ort = ort;
			Größe = größe;

			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			if (Schussbereit)
			{
				Schussbereit = false;
			}
			else
			{
				Schussbereit = true;
			}
		}

		public void SchadenErleiden(double schaden)
		{
			Lebenspunkte -= schaden;
		}

		public override void Bewegen(Richtung richtung)
		{
			double Weite = 100;
			if (richtung == Richtung.Rechts)
			{
				Weite *= 1;
				Ort = new Point(Ort.X + Weite, Ort.Y);
			}
			if (richtung == Richtung.Links)
			{
				Weite *= -1;
				Ort = new Point(Ort.X + Weite, Ort.Y);
			}
			if (richtung == Richtung.Runter)
			{
				Ort = new Windows.Foundation.Point(Ort.X, Ort.Y + Weite);
			}
			else
			{
				Ort = new Windows.Foundation.Point(Ort.X, Ort.Y - Weite);
			}
		}
	}
}
