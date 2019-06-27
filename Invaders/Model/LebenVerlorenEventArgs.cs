using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.Model
{
	class LebenVerlorenEventArgs : EventArgs
	{
		public double Leben { get; private set; }

		public LebenVerlorenEventArgs(double leben)
			{
			Leben = leben;
			}
	}
}
