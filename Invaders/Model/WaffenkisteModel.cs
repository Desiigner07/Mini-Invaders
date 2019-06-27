
using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Invaders.Model
{
	class WaffenkisteModel
	{
		public Waffen WaffenTyp { get; private set; }
		public Point Ort { get; private set; }
		public bool Greifbar { get; private set; }

		DispatcherTimer timer = new DispatcherTimer();

		public WaffenkisteModel(Waffen waffentyp, Point ort)
		{
			Greifbar = false;
			WaffenTyp = waffentyp;
			Ort = ort;
			timer.Interval = TimeSpan.FromSeconds(3);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			Greifbar = true;
			timer.Start();
		}
	}
}
