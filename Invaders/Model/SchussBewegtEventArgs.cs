using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.Model
{
	class SchussBewegtEventArgs : EventArgs
	{
		public Schuss Schuss { get; private set; }
		public bool Verschwunden { get; private set; }
		public Model.Waffen Waffentyp { get; private set; }

		public SchussBewegtEventArgs(Schuss schuss, bool verschwunden, Waffen typ)
		{
			Waffentyp = typ;
			Schuss = schuss;
			Verschwunden = verschwunden;
		}

	}
}
