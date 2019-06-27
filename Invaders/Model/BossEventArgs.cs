using System;

namespace Invaders.Model
{
	class BossEventArgs : EventArgs
	{
		public double Lebenspunkte { get; private set; }
		public bool Angriff { get; private set; }
		public bool Tod { get; private set; }

		public InvaderBoss InvaderBoss { get; private set; }

		public BossEventArgs(double leben, bool angriff, InvaderBoss boss)
		{
			InvaderBoss = boss;
			Lebenspunkte = leben;
			Angriff = angriff;

			if (Lebenspunkte < 0)
			{
				Tod = true;
			}
		}
	}
}
