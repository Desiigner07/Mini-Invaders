using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
	class Spieler : Schiff
	{
		static Size SpielerGröße;
		public new Point Ort;
		public Waffen AktuelleWaffe;
		public int MaxSpielerSchüsse;
		public int Munition;
		public Spieler(Point ort, Size größe, Waffen aktWaffe) : base(ort, größe)
		{
			AktuelleWaffe = aktWaffe;
			SpielerGröße = größe;
			Ort = ort;
			WaffeAusrüsten(aktWaffe);
		}

		public void WaffeAusrüsten(Waffen aktWaffe)
		{
			if (aktWaffe == Waffen.Laser)
			{
				AktuelleWaffe = aktWaffe;
				MaxSpielerSchüsse = 5;
			}
			else if (aktWaffe == Waffen.Lenkrakete)
			{
				AktuelleWaffe = aktWaffe;
				MaxSpielerSchüsse = 10;
				Munition = 10;
			}
			else if (aktWaffe == Waffen.Raketen)
			{
				AktuelleWaffe = aktWaffe;
				MaxSpielerSchüsse = 6;
				Munition = 6;
			}
			else if (aktWaffe == Waffen.Hyperschallrakete)
			{
				AktuelleWaffe = aktWaffe;
				MaxSpielerSchüsse = 20;
				Munition = 20;
			}
		}

		private void MunitionPrüfen()
		{
			if (Munition == 0)
			{
				WaffeAusrüsten(Waffen.Laser);
			}
		}

		public void SchussVerbrauchen()
		{
			Munition--;
			MunitionPrüfen();
		}

		public override void Bewegen(Richtung richtung)
		{
			double Weite = 20;
			if (richtung == Richtung.Rechts)
			{
				Weite *= 1;
			}
			if (richtung == Richtung.Links)
			{
				Weite *= -1;
			}
			Ort = new Point(Ort.X + Weite, Ort.Y);
		}
	}
}
