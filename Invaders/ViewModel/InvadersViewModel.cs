using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Invaders.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using System.Collections.Specialized;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Invaders.ViewModel
{
	public class InvadersViewModel : INotifyPropertyChanged
	{
		InvadersModel _model = new InvadersModel();
		private ObservableCollection<UIElement> _sprites = new ObservableCollection<UIElement>();
		public INotifyCollectionChanged Sprites { get { return _sprites; } }

		private Dictionary<Schuss, Rectangle> _schüsse = new Dictionary<Schuss, Rectangle>();
		private Dictionary<Schiff, View.AnimiertesBild> _invader = new Dictionary<Schiff, View.AnimiertesBild>();
		private Dictionary<Point, UIElement> _sterne = new Dictionary<Point, UIElement>();
		private Dictionary<WaffenkisteModel, View.Waffenkiste> _waffenkiste = new Dictionary<WaffenkisteModel, View.Waffenkiste>();
		private Dictionary<InvaderBoss, View.InvaderBoss> _invaderBoss = new Dictionary<InvaderBoss, View.InvaderBoss>();

		private string _welle = "Welle ";
		public string Welle { get { return _welle; } set { this.SetField(ref this._welle, value); } }

		private int _punkte;
		public int Punkte { get { return _punkte; } set { this.SetField(ref this._punkte, value); } }

		private string _waffe = "Waffe ";
		public string Waffe { get  { return _waffe; } set { this.SetField(ref this._waffe, value); } }

		private int _munition = 0;
		public int Munition { get { return _munition; } set { this.SetField(ref this._munition, value); } }

		private double _maxMunition;
		public double MaxMunition { get { return _maxMunition; } set { this.SetField(ref this._maxMunition, value);  } }

		#region
		private bool _1livepoint = false;
		private bool _2livepoint = false;
		private bool _3livepoint = false;
		private bool _gameOver = true;
		public bool Live1Visible             //true = unsichtbar
		{
			get
			{
				return _1livepoint;
			}
			set
			{
				this.SetField(ref this._1livepoint, value);
			}
		}
		public bool Live2Visible
		{
			get
			{
				return _2livepoint;
			}
			set
			{
				this.SetField(ref this._2livepoint, value);
			}
		}
		public bool Live3Visible
		{
			get
			{
				return _3livepoint;
			}
			set
			{
				this.SetField(ref this._3livepoint, value);
			}
		}
		public bool GameOver
		{
			get
			{
				return _gameOver;
			}
			set
			{
				this.SetField(ref this._gameOver, value);
			}
		}
		#endregion

		DispatcherTimer timer = new DispatcherTimer();
		View.AnimiertesBild Spieler = InvadersFactory.SpielerControlFactory();
		bool right, left;

		public Size SpielfeldGröße
		{
			get
			{
				return InvadersModel.SpielfeldGröße;
			}
			set
			{
				Model.InvadersModel.SpielfeldGröße = value;
			}
		}
		public InvadersViewModel()
		{
			_model.SchiffGeändert += _model_SchiffGeändert;
			_model.SternGeändert += _model_SternGeändert;
			_model.SchussBewegt += _model_SchussBewegt;
			_model.LebenVerloren += _model_LebenVerloren;
			_model.KisteGeändert += _model_KisteGeändert;
			_model.BossEvent += _model_BossEvent;

			timer.Interval = TimeSpan.FromMilliseconds(100);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		  {
			if (right)
			{
				_model.SpielerBewegen(Richtung.Rechts);
			}
			if (left)
			{
				_model.SpielerBewegen(Richtung.Links);
			}
			_model.Aktualisieren();
			UIAktualisieren();
		}

		private void UIAktualisieren()
		{
			//Welle
			Welle = "Welle " + _model.Welle.ToString();

			//Punkte
			Punkte = _model.Punkte;

			//Waffe
			Waffe = _model._spieler.AktuelleWaffe.ToString();

			//Munition 
			Munition = _model._spieler.Munition;

			//Maximale Munition
			MaxMunition = _model._spieler.MaxSpielerSchüsse;
		}

		internal void KeyUp(VirtualKey virtualKey)
		{
			if (virtualKey == VirtualKey.Right)
			{
				right = false;
			}
			if (virtualKey == VirtualKey.Left)
			{
				left = false;
			}
		}

		internal void KeyDown(VirtualKey virtualKey)
		{
			if (virtualKey == VirtualKey.Right)
			{
				right = true;
				left = false;
			}
			if (virtualKey == VirtualKey.Left)
			{
				left = true;
				right = false;
			}
			if (virtualKey == VirtualKey.Space)
			{
				_model.Feuern();
			}
		}

		private void _model_SchussBewegt(object sender, SchussBewegtEventArgs e)
		{
			if (!e.Verschwunden)
			{
				if (!_schüsse.Keys.Contains(e.Schuss))
				{
					if (e.Waffentyp == Waffen.Laser)
					{
						Rectangle rec = InvadersFactory.SchussFactory();
						_schüsse.Add(e.Schuss, rec);
						_sprites.Add(rec);
						InvadersFactory.OrtSetzen(rec, e.Schuss.Ort.X, e.Schuss.Ort.Y);
					}
					if (e.Waffentyp == Waffen.Raketen)
					{
						Rectangle rec = InvadersFactory.RaktenFactory();
						_schüsse.Add(e.Schuss, rec);
						_sprites.Add(rec);
						InvadersFactory.OrtSetzen(rec, e.Schuss.Ort.X, e.Schuss.Ort.Y);
					}
					if (e.Waffentyp == Waffen.Lenkrakete)
					{
						Rectangle rec = InvadersFactory.LenkRaketenFactory();
						_schüsse.Add(e.Schuss, rec);
						_sprites.Add(rec);
						InvadersFactory.OrtSetzen(rec, e.Schuss.Ort.X, e.Schuss.Ort.Y);
					}
					if (e.Waffentyp == Waffen.Hyperschallrakete)
					{
						Rectangle rec = InvadersFactory.HyperschallraketenFactory();
						_schüsse.Add(e.Schuss, rec);
						_sprites.Add(rec);
						InvadersFactory.OrtSetzen(rec, e.Schuss.Ort.X, e.Schuss.Ort.Y);
					}
				}
				else                      //Wen der Schuss im Dictionary ist wird er bewegt
				{
					if (e.Schuss.Waffentyp == Waffen.Hyperschallrakete)
					{
						InvadersFactory.RotationSetzen(_schüsse[e.Schuss], e.Schuss.Rotation());
						InvadersFactory.ElementBewegen(_schüsse[e.Schuss], e.Schuss.Ort.X, e.Schuss.Ort.Y, TimeSpan.FromSeconds(1));
					}
					else if (e.Schuss.Waffentyp == Waffen.Lenkrakete)
					{
						InvadersFactory.RotationSetzen(_schüsse[e.Schuss], e.Schuss.Rotation());
						InvadersFactory.ElementBewegen(_schüsse[e.Schuss], e.Schuss.Ort.X, e.Schuss.Ort.Y, TimeSpan.FromSeconds(0.1));
					}
					else
					{
						InvadersFactory.ElementBewegen(_schüsse[e.Schuss], e.Schuss.Ort.X, e.Schuss.Ort.Y, TimeSpan.FromSeconds(0.1));
					}
				}
			}
			else                       //Wenn Verschwunden == true ist wird der Schuss entfernt
			{
				if (_schüsse.Keys.Contains(e.Schuss))
				{
					_sprites.Remove(_schüsse[e.Schuss]);
					_schüsse.Remove(e.Schuss);
				}
			}
		}

		private void _model_SternGeändert(object sender, SternGeändertEventArgs e)
		{
			if (e.Verschwunden && _sterne.Keys.Contains(e.Point))
			{
				_sprites.Remove(_sterne[e.Point]);
			}
			else
			{
				if (!_sterne.ContainsKey(e.Point))
				{
					UIElement stern = InvadersFactory.SternControlFactory();
					_sterne.Add(e.Point, stern);
					_sprites.Add(stern);
					InvadersFactory.OrtSetzen(stern, e.Point.X, e.Point.Y);
				}
			}
		}

		private void _model_SchiffGeändert(object sender, SchiffGeändertEventArgs e)
		{
			if (e.SchiffAktualisiert is Invader)
			{
				if (_invader.Keys.Contains(e.SchiffAktualisiert))
				{
					if (e.Tod)
					{
						_invader[e.SchiffAktualisiert].InvaderGetroffen();
					}
					else
					{
						if ((e.SchiffAktualisiert as Invader).Invadertyp != InvaderTyp.Satellit)
						{
							InvadersFactory.ElementBewegen(_invader[e.SchiffAktualisiert], (e.SchiffAktualisiert as Invader).Ort.X, (e.SchiffAktualisiert as Invader).Ort.Y, TimeSpan.FromSeconds(0.3));
						}
						else
						{
							InvadersFactory.ElementBewegen(_invader[e.SchiffAktualisiert], (e.SchiffAktualisiert as Invader).Ort.X, (e.SchiffAktualisiert as Invader).Ort.Y, TimeSpan.FromSeconds(10));
						}
					}
				}
				else
				{
					View.AnimiertesBild invader = InvadersFactory.InvaderFactory(e.SchiffAktualisiert as Invader);
					_invader.Add(e.SchiffAktualisiert, invader);
					_sprites.Add(invader);
					InvadersFactory.OrtSetzen(invader, e.SchiffAktualisiert.Ort.X, e.SchiffAktualisiert.Ort.Y);
				}
			}
			else if (e.SchiffAktualisiert is Spieler)
			{
				if (_invader.Keys.Contains(e.SchiffAktualisiert))
				{
					InvadersFactory.ElementBewegen(_invader[e.SchiffAktualisiert], (e.SchiffAktualisiert as Spieler).Ort.X, (e.SchiffAktualisiert as Spieler).Ort.Y, TimeSpan.FromSeconds(0.3));
				}
				else
				{
					_invader.Add(e.SchiffAktualisiert, Spieler);
					_sprites.Add(Spieler);
					InvadersFactory.OrtSetzen(Spieler, e.SchiffAktualisiert.Ort.X, e.SchiffAktualisiert.Ort.Y);
				}
			}
			else if (e.SchiffAktualisiert is InvaderBoss)
			{
				if (_invaderBoss.Keys.Contains(e.SchiffAktualisiert))
				{
					InvadersFactory.ElementBewegen(_invaderBoss[e.SchiffAktualisiert as InvaderBoss], (e.SchiffAktualisiert as InvaderBoss).Ort.X, (e.SchiffAktualisiert as InvaderBoss).Ort.Y, TimeSpan.FromSeconds(0.5));
				}
				else
				{
					View.InvaderBoss Boss = InvadersFactory.BossFactory();
					_invaderBoss.Add((e.SchiffAktualisiert as InvaderBoss), Boss);
					_sprites.Add(Boss);
					InvadersFactory.OrtSetzen(Boss, (e.SchiffAktualisiert as InvaderBoss).Ort.X, (e.SchiffAktualisiert as InvaderBoss).Ort.Y);
				}
			}
		}

		private void _model_BossEvent(object sender, BossEventArgs e)
		{
			if (e.Angriff)
			{
				//AngriffsAnimation
				
			}
			else
			{
				//Schaden Erleiden Animation
				_invaderBoss[e.InvaderBoss].ApplyDamage();
			}

			if (e.Tod)
			{
				_invaderBoss[e.InvaderBoss].BossStirbt();
				//_sprites.Remove(_invaderBoss[e.InvaderBoss]);
			}
		}

		private void _model_LebenVerloren(object sender, LebenVerlorenEventArgs e)
		{
			if (e.Leben <= 0)
			{
				GameOver = false;
				Live1Visible = true;
				Live2Visible = true;
				Live3Visible = true;
				Spieler.SpielerGetroffen();
			}
			if (e.Leben == 1)
			{
				Live1Visible = false;                                                               
				Live2Visible = true;
				Live3Visible = true;
				Spieler.SpielerGetroffen();
			}
			if (e.Leben == 2)
			{
				Live1Visible = false;
				Live2Visible = false;
				Live3Visible = true;
				Spieler.SpielerGetroffen();
			}
			if (e.Leben == 3)
			{
				Live1Visible = false;
				Live2Visible = false;
				Live3Visible = false;
				Spieler.SpielerGetroffen();
			}
		}

		private void _model_KisteGeändert(object sender, KisteEventArgs e)
		{
			if (_waffenkiste.ContainsKey(e.Waffenkiste))
			{
				if (e.Aufgesammelt)
				{
					_sprites.Remove(_waffenkiste[e.Waffenkiste]);
					_waffenkiste.Remove(e.Waffenkiste);
				}
			}
			else
			{
				View.Waffenkiste waffenkiste = new View.Waffenkiste(e.PositionX);
				_waffenkiste.Add(e.Waffenkiste, waffenkiste);
				InvadersFactory.OrtSetzen(waffenkiste, e.PositionX, 300);
				_sprites.Add(waffenkiste);
				InvadersFactory.ElementBewegen(waffenkiste, e.PositionX, 780, TimeSpan.FromSeconds(3));
			}
		}

		//Property Changed Logic
		public event PropertyChangedEventHandler PropertyChanged;
		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(field, value)) return false;
			field = value;
			this.NotifyPropertyChanged(propertyName);
			return true;
		}

		private void NotifyPropertyChanged(string propertyName)
		{
			var eventHandler = this.PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}

