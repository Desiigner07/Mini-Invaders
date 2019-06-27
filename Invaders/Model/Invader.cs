using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
	class Invader : Schiff
	{
		static Size InvaderGröße { get; set; }
		int Punkte
		{
			get { return (int)Invadertyp; }
		}
		public InvaderTyp Invadertyp { get; private set; }
		public new Point Ort { get; private set; }
		public Invader(Point ort, Size größe, InvaderTyp typ) : base(ort, größe)
		{
			Ort = ort;
			InvaderGröße = größe;
			Invadertyp = typ;
		}


		public override void Bewegen(Richtung richtung)
		{
			double Weite = 50;
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
		}

		public void Sturzflug()
		{
			
		}
	}
}
