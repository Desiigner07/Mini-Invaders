using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.Model
{
	class KisteEventArgs : EventArgs
	{
		public WaffenkisteModel Waffenkiste { get; private set; }
		public Waffen Waffentyp { get; private set; }
		public bool Aufgesammelt { get; private set; }

		public double PositionX { get; private set; }
		public KisteEventArgs(WaffenkisteModel kiste, bool aufgesammelt, double x)
		{
			Waffenkiste = kiste;
			Waffentyp = kiste.WaffenTyp;
			Aufgesammelt = aufgesammelt;
			PositionX = x;
		}
	}
}
