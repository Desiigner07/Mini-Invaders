using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
	class InvadersModel
	{
		public static Size SpielfeldGröße = new Size(400, 300);
		public const int StartSternZahl = 50;

		private readonly Random _random = new Random();

		public int Punkte { get; private set; }
		public int Welle { get; private set; }
		public int Leben { get; private set; }
		public bool GameOver { get; private set; }
		public bool BossWelle { get; private set; }
		private DateTime? _spielerTod = null;
		public bool SpielerStirbt { get { return _spielerTod.HasValue; } }

		public Spieler _spieler;
		public InvaderBoss _boss;
		private readonly List<Invader> _invaders = new List<Invader>();
		private readonly List<Schuss> _spielerSchüsse = new List<Schuss>();
		private readonly List<Schuss> _invaderSchüsse = new List<Schuss>();
		private readonly List<Point> _sterne = new List<Point>();
		private List<Invader> _toteInvader = new List<Invader>();
		private List<Schuss> _verschwundeneSchüsse = new List<Schuss>();
		private List<WaffenkisteModel> _aufgesammelteKisten = new List<WaffenkisteModel>();
		private List<WaffenkisteModel> _waffenkisten = new List<WaffenkisteModel>();

		private Richtung _invaderRichtung = Richtung.Rechts;
		private DateTime _zuletztAktualisiert = DateTime.MinValue;
		public InvadersModel()
		{
			SpielStarten();
		}

		private void SpielBeenden()
		{
			GameOver = true;
		}

		public void SpielStarten()
		{
			GameOver = false;
			//Invaders löschen
			foreach (Invader invader in _invaders)
			{
				OnSchiffGeändert(invader, true);
				_invaders.Remove(invader);
			}
			//Spielerschüsse löschen
			foreach (Schuss schuss in _spielerSchüsse)
			{
				OnSchussBewegt(schuss, true, Waffen.Laser);
				_spielerSchüsse.Remove(schuss);
			}
			//Invaderschüsse löschen
			foreach (Schuss schuss in _invaderSchüsse)
			{
				OnSchussBewegt(schuss, true, Waffen.Laser);
				_invaderSchüsse.Remove(schuss);
			}
			//Sterne löschen
			foreach (Point stern in _sterne)
			{
				OnSternGeändert(stern, true);
				_sterne.Remove(stern);
			}
			//Sterne erstellen
			for (int i = 0; i < StartSternZahl; i++)
			{
				Point p = new Point(_random.Next((int)SpielfeldGröße.Width), _random.Next((int)SpielfeldGröße.Height));
				_sterne.Add(p);
				OnSternGeändert(p, false);
			}

			//Spieler erstellen
			_spieler = new Spieler(new Windows.Foundation.Point(400, 800), new Size(50, 50), Waffen.Laser);
			Welle = 0;
			Leben = 3;

			//Welle generieren
			NächsteWelle();
		}

		public void Feuern()
		{
			if (_spielerSchüsse.Count < _spieler.MaxSpielerSchüsse)
			{
				if (_spieler.AktuelleWaffe == Waffen.Laser)
				{
					Point p = new Point(_spieler.Ort.X + 25, _spieler.Ort.Y - 5);
					Schuss schuss = new Schuss(p, Richtung.Hoch, Waffen.Laser);
					_spielerSchüsse.Add(schuss);
					OnSchussBewegt(schuss, false, Waffen.Laser);
				}
				if (_spieler.AktuelleWaffe == Waffen.Raketen)
				{
					Point p = new Point(_spieler.Ort.X + 25, _spieler.Ort.Y - 5);
					Schuss schuss = new Model.Schuss(p, Richtung.Hoch, Waffen.Raketen);
					_spielerSchüsse.Add(schuss);
					OnSchussBewegt(schuss, false, Waffen.Raketen);
				}
				if (_spieler.AktuelleWaffe == Waffen.Lenkrakete)
				{
					Point p = new Point(_spieler.Ort.X + 25, _spieler.Ort.Y - 5);
					Schuss schuss = new Model.Schuss(p, Richtung.Hoch, Waffen.Lenkrakete);
					_spielerSchüsse.Add(schuss);
					OnSchussBewegt(schuss, false, Waffen.Lenkrakete);
				}
				if (_spieler.AktuelleWaffe == Waffen.Hyperschallrakete)
				{
					Point p = new Point(_spieler.Ort.X + 25, _spieler.Ort.Y - 5);
					Schuss schuss = new Schuss(p, Richtung.Hoch, Waffen.Hyperschallrakete);
					_spielerSchüsse.Add(schuss);
					OnSchussBewegt(schuss, false, Waffen.Hyperschallrakete);
				}
				_spieler.SchussVerbrauchen();
			}
		}

		public void SpielerBewegen(Richtung richtung)
		{
			if (SpielerStirbt) return;
			_spieler.Bewegen(richtung);
			OnSchiffGeändert(_spieler, false);
		}

		public void Blinken()
		{
			int münze = _random.Next(2);
			if (münze == 0)
			{
				if (_sterne.Count <= StartSternZahl * 1.5)
				{
					Point p = new Windows.Foundation.Point(_random.Next((int)SpielfeldGröße.Width), _random.Next((int)SpielfeldGröße.Height));
					_sterne.Add(p);
					OnSternGeändert(p, false);
				}
			}
			else
			{
				if (_sterne.Count >= StartSternZahl * 0.85)
				{
					Point p = _sterne[_random.Next(_sterne.Count)];
					_sterne.Remove(p);
					OnSternGeändert(p, true);
				}
			}
		}

		public void Aktualisieren()
		{
			if (GameOver == false)
			{
				NächsteWelle();
				SpielerKollisionPrüfen();
				InvaderKollisionPrüfen();
	  //	SpielerKollisionMitInvaderPrüfen();
				InvaderBewegen();
				ZurückFeuern();
				SchüsseAktualisieren();
				SchüsseBewegen();
				SpielerRespawn();
				Blinken();
				ListenLeeren();
				WaffenKistePrüfen();
			}
		}

		private void WaffenKistePrüfen()
		{
			var kisten = from WaffenkisteModel waffe in _waffenkisten
									 where NearToWeaponChest(_spieler, waffe)
									 select waffe;

			foreach (WaffenkisteModel waffenkiste in kisten)
			{
				if (waffenkiste.Greifbar)
				{
					_aufgesammelteKisten.Add(waffenkiste);
					_spieler.WaffeAusrüsten(waffenkiste.WaffenTyp);
				}
			}
		}

		private bool NearToWeaponChest(Spieler spieler, WaffenkisteModel kiste)
		{
			Rect obj2 = new Rect(kiste.Ort.X, 800, 50, 50);
			Rect obj = new Rect((spieler as Spieler).Ort, spieler.Größe);
			obj.Intersect(obj2);
			if (obj.Height > 0 || obj.Width > 0)
			{
				return true;
			}
			else
				return false;
		}

		private void ListenLeeren()
		{
			foreach (Schuss schuss in _verschwundeneSchüsse)
			{
				if (_spielerSchüsse.Contains(schuss))
				{
					_spielerSchüsse.Remove(schuss);
					OnSchussBewegt(schuss, true, schuss.Waffentyp);
				}
				else if (_invaderSchüsse.Contains(schuss))
				{
					_invaderSchüsse.Remove(schuss);
					OnSchussBewegt(schuss, true, Waffen.Laser);
				}
			}
			foreach (WaffenkisteModel kiste in _aufgesammelteKisten)
			{
				if (_waffenkisten.Contains(kiste))
				{
					_waffenkisten.Remove(kiste);
					OnKisteHinzu(kiste, true, kiste.Ort.X);
					_waffenkisten.Remove(kiste);
				}
			}
			_aufgesammelteKisten.Clear();

			foreach (Invader invader in _toteInvader)
			{
				if (_invaders.Contains(invader))
				{
					_invaders.Remove(invader);
					OnSchiffGeändert(invader, true);
					Punkte += (int)invader.Invadertyp;
					WaffenKisten(invader.Ort);
				}
			}
			_verschwundeneSchüsse.Clear();
			_toteInvader.Clear();
		}

		private void WaffenKisten(Point ort)
		{
			if (_random.Next(0, 25) == 0)
			{
				WaffenkisteModel waffenKiste = new WaffenkisteModel((Waffen)_random.Next(1, 4), ort);
				OnKisteHinzu(waffenKiste, false, ort.X);
				_waffenkisten.Add(waffenKiste);
			}
		}

		private void NächsteWelle()
		{
			if (_invaders.Count == 0 && !BossWelle)
			{
				Welle++;
				_invaders.Clear();
				_boss = null;

				if (_random.Next(0, 8) == 0)
				{
					BossWelle = true;
					_boss = new InvaderBoss(new Point(300, 100), new Size(400, 300));
					OnSchiffGeändert(_boss, false);
				}
				else
				{
					for (int row = 0; row < 6; row++)
					{
						for (int col = 0; col < 10; col++)
						{
							BossWelle = false;
							Invader invader = new Invader(new Windows.Foundation.Point(col * 60, row * 60), new Windows.Foundation.Size(50, 50), InvaderTypFürReihe(row));
							_invaders.Add(invader);
							OnSchiffGeändert(invader, false);
						}
					}
				}
			}
		}

		private void OutOfMapPrüfen()
		{
			var outofInvader = from Invader inv in _invaders
												 where inv.Ort.Y > SpielfeldGröße.Height - 500
												 select inv;
			if (outofInvader.Count() != 0)
			{
				GameOver = true;
			}
		}

		private void SchüsseBewegen()
		{
			foreach (Schuss schuss in _spielerSchüsse)
			{
				if (schuss.Waffentyp == Waffen.Laser)
				{
					schuss.Bewegen();
					OnSchussBewegt(schuss, false, Waffen.Laser);
				}
				else if (schuss.Waffentyp == Waffen.Raketen)
				{
					schuss.Bewegen();
					OnSchussBewegt(schuss, false, Waffen.Raketen);
				}
				else if (schuss.Waffentyp == Waffen.Lenkrakete)
				{
					ZielInvaderFinden(schuss);
					schuss.ZuZielBewegen();
					OnSchussBewegt(schuss, false, Waffen.Lenkrakete);
				}
				else if (schuss.Waffentyp == Waffen.Hyperschallrakete)
				{
					ZielInvaderFinden(schuss);
					schuss.HyperSchallBewegen();
					OnSchussBewegt(schuss, false, Waffen.Hyperschallrakete);
				}
			}
			foreach (Schuss schuss in _invaderSchüsse)
			{
				schuss.Bewegen();
				OnSchussBewegt(schuss, false, Waffen.Laser);
			}
		}

		private void ZielInvaderFinden(Schuss schuss)
		{
			if (!BossWelle)
			{
				if (_invaders.Count() != 0)
				{
					if (schuss.ZielInvader != null && _invaders.Contains(schuss.ZielInvader))
					{
						schuss.ZielPoint = schuss.ZielInvader.Ort;
					}
					else
					{
						var invaderReihe = ZufallsInvader();
						schuss.ZielInvader = invaderReihe.Last();
						schuss.ZielPoint = invaderReihe.Last().Ort;
					}
				}
			}
			else
			{
				schuss.ZielPoint = new Point(_boss.Ort.X + 100, _boss.Ort.Y + 100);
			}
		}

		private IOrderedEnumerable<Invader> ZufallsInvader()
		{
			Invader randomInv = _invaders[_random.Next(_invaders.Count)];
			var invaderReihe = from Invader invader in _invaders
												 where invader.Ort.X == randomInv.Ort.X
												 orderby invader.Ort.X ascending
												 select invader;
			return invaderReihe;
		}

		private void Sturzbomber()
		{
			Invader invader = ZufallsInvader().Last();
			invader.Sturzflug();
		}

		private void SpielerRespawn()
		{
			if (SpielerStirbt && Leben > 0)
			{
				Leben -= 1;
				_spielerTod = null;
				OnLebenVerloren(Leben);
			}
		}

		private InvaderTyp InvaderTypFürReihe(int row)
		{
			if (row == 0)
				return InvaderTyp.Untertasse;
			else if (row == 1)
				return InvaderTyp.Insekt;
			else if (row > 1 && row < 4)
				return InvaderTyp.Nuke;
			else
				return InvaderTyp.Stern;
		}

		private void SpielerKollisionPrüfen()
		{
			var kollision = from Schuss schuss in _invaderSchüsse
											where Kollision(schuss, _spieler)
											select schuss;
			if (kollision.Count() > 0)
			{
				_spielerTod = DateTime.Now;
				_verschwundeneSchüsse.Add(kollision.First());
			}
			if (Leben < 1)
			{
				GameOver = true;
			}
		}

		private void SpielerKollisionMitInvaderPrüfen()
		{
			var kollision = from Invader invader in _invaders
											where KollisionMitInvader(invader, _spieler)
											select invader;
			if (kollision.Count() != 0)
			{
				_spielerTod = DateTime.Now;
				_toteInvader.Add(kollision.First());
			}
			if (Leben < 1)
			{
				GameOver = true;
			}
		}

		private bool KollisionMitInvader(Invader invader, Spieler _spieler)
		{
			Rect inv = new Rect(invader.Ort, invader.Größe);
			Rect spieler = new Rect(_spieler.Ort, _spieler.Größe);

			inv.Intersect(spieler);
			if (inv.Height > 0 || inv.Width > 0)
			{
				return true;
			}
			else
				return false;
		}

		private bool Kollision(Schuss schuss, Schiff element)
		{
			Rect obj2;
			Rect obj;
			if (element is Invader)
			{
				obj2 = new Rect((element as Invader).Ort, new Size(50, 50));
				if (_spieler.AktuelleWaffe == Waffen.Laser)
				{
					obj = new Rect(schuss.Ort, Schuss.SchussGröße);
				}
				if (_spieler.AktuelleWaffe == Waffen.Raketen)
				{
					obj = new Rect(new Point(schuss.Ort.X - 125, schuss.Ort.Y - 125), new Size(250, 250));
				}
				if (_spieler.AktuelleWaffe == Waffen.Lenkrakete)
				{
					obj = new Rect(new Point(schuss.Ort.X - 50, schuss.Ort.Y - 50), new Size(100, 100));
				}
				if (_spieler.AktuelleWaffe == Waffen.Hyperschallrakete)
				{
					obj = new Rect(schuss.Ort, new Size(40, 40));
				}
			}
			else if (element is Spieler)
			{
				obj2 = new Rect((element as Spieler).Ort, new Size(50, 50));
				obj = new Rect(schuss.Ort, Schuss.SchussGröße);
			}
			obj.Intersect(obj2);
			if (obj.Height > 0 || obj.Width > 0)
			{
				return true;
			}
			else
				return false;
		}

		private void InvaderKollisionPrüfen()
		{
			if (!BossWelle)
			{
				foreach (Schuss schuss in _spielerSchüsse)
				{
					InvaderKollisionMit(schuss);
				}
			}
			else
			{
				foreach (Schuss schuss in _spielerSchüsse)
				{
					BossKollisionMit(schuss);
				}
			}
		}

		private void BossKollisionMit(Schuss schuss)
		{
			Rect obj1 = new Rect(schuss.Ort, Schuss.SchussGröße);
			Rect obj2 = new Rect(_boss.Ort, new Size(400, 300));
			obj1.Intersect(obj2);
			if (obj1.Width > 0 || obj1.Height > 0)
			{
				if (schuss.Waffentyp == Waffen.Laser)
				{
					_boss.SchadenErleiden(2);
				}
				else if (schuss.Waffentyp == Waffen.Raketen)
				{
					_boss.SchadenErleiden(10);
				}
				else
				{
					_boss.SchadenErleiden(4);
				}

				_verschwundeneSchüsse.Add(schuss);
				OnBossSchadenErlitten(_boss.Lebenspunkte, false, _boss);

				if (_boss.Lebenspunkte < 0)
				{
					BossWelle = false;
					Punkte += (int)_boss.InvaderTyp;
				}
			}
		}

		private void InvaderKollisionMit(Schuss schuss)
		{
			var kollision = from Invader in _invaders
											where Kollision(schuss, Invader)
											select Invader;

			if (kollision.Count() != 0)
			{
				foreach (Invader invader in kollision)
				{
					_toteInvader.Add(invader);
				}
				if (schuss.Waffentyp == Waffen.Raketen)
				{
					if (kollision.Count() > 1)
					{
						_verschwundeneSchüsse.Add(schuss);
					}
				}
				else
				{
					_verschwundeneSchüsse.Add(schuss);
				}
			}
		}

		int cooldown = 0;
		private void InvaderBewegen()
		{
			cooldown++;
			if (cooldown > 9)
			{
				cooldown = 0;
				if (!BossWelle)
				{
					InvaderBewegRichtung();
					foreach (Invader invader in _invaders)
					{
						invader.Bewegen(_invaderRichtung);
						OnSchiffGeändert(invader, false);
					}
				}
				else
				{
					_boss.Bewegen(BossRichtung());
					OnSchiffGeändert(_boss, false);
				}
			}
		}

		private Richtung BossRichtung()
		{
			if (_boss.Ort.X > SpielfeldGröße.Width - 200)
			{
				return Richtung.Links;
			}
			else if (_boss.Ort.X < 100)
			{
				return Richtung.Rechts;
			}
			else if (_boss.Ort.Y > SpielfeldGröße.Height - 500)
			{
				return Richtung.Hoch;
			}
			else if (_boss.Ort.Y < 100)
			{
				return Richtung.Runter;
			}
			else return (Richtung)_random.Next(0, 4);
		}

		private void InvaderBewegRichtung()
		{
			var InvaderHitRight = from Invader invader in _invaders
														where invader.Ort.X > SpielfeldGröße.Width - 150
														select invader;
			if (InvaderHitRight.Count() > 0)
			{
				_invaderRichtung = Richtung.Links;

				foreach (Invader invader in _invaders)
				{
					invader.Bewegen(Richtung.Runter);
				}
			}

			var InvaderHitLeft = from Invader invader in _invaders
													 where invader.Ort.X < 50
													 select invader;
			if (InvaderHitLeft.Count() > 0)
			{
				_invaderRichtung = Richtung.Rechts;
				foreach (Invader invader in _invaders)
				{
					invader.Bewegen(Richtung.Runter);
					OnSchiffGeändert(invader, false);
				}
			}
		}

		private void ZurückFeuern()
		{
			if (!BossWelle)
			{
				if (_invaderSchüsse.Count() < Welle + 2)
				{
					var invaderReihe = ZufallsInvader();
					Invader TheInvader = invaderReihe.Last();
					Point p = new Point(TheInvader.Ort.X + 20, TheInvader.Ort.Y + 50);
					Schuss schuss = new Schuss(p, Richtung.Runter, Waffen.Laser);
					_invaderSchüsse.Add(schuss);
					OnSchussBewegt(schuss, false, Waffen.Laser);
				}
			}
			else
			{
				if (_boss.Schussbereit)
				{
					if (_invaderSchüsse.Count() < Welle + 7)
					{
						Point p = new Point(_boss.Ort.X + _random.Next(120, 180), _boss.Ort.Y + 300);
						Schuss schuss = new Schuss(p, Richtung.Runter, Waffen.Laser);
						_invaderSchüsse.Add(schuss);
						OnBossSchadenErlitten(_boss.Lebenspunkte, true, _boss);
						OnSchussBewegt(schuss, false, Waffen.Laser);
					}
				}
			}
		}

		private void SchüsseAktualisieren()
		{
			var spielerSchüsse = from Schuss schuss in _spielerSchüsse
													 where schuss.Ort.Y < 0
													 select schuss;
			var invaderSchüsse = from Schuss schuss in _invaderSchüsse
													 where schuss.Ort.Y > SpielfeldGröße.Height
													 select schuss;
			foreach (Schuss schuss in spielerSchüsse)
			{
				OnSchussBewegt(schuss, true, schuss.Waffentyp);
				_verschwundeneSchüsse.Add(schuss);
			}
			foreach (Schuss schuss in invaderSchüsse)
			{
				OnSchussBewegt(schuss, true, Waffen.Laser);
				_verschwundeneSchüsse.Add(schuss);
			}
		}

		public event EventHandler<SchiffGeändertEventArgs> SchiffGeändert;
		public void OnSchiffGeändert(Schiff schiff, bool tod)
		{
			EventHandler<SchiffGeändertEventArgs> schiffGeändert = SchiffGeändert;
			if (schiffGeändert != null)
				schiffGeändert(this, new SchiffGeändertEventArgs(schiff, tod));
		}

		public event EventHandler<SchussBewegtEventArgs> SchussBewegt;
		public void OnSchussBewegt(Schuss schuss, bool verschwunden, Waffen typ)
		{
			EventHandler<SchussBewegtEventArgs> schussBewegt = SchussBewegt;
			if (schussBewegt != null)
				schussBewegt(this, new SchussBewegtEventArgs(schuss, verschwunden, typ));
		}

		public event EventHandler<SternGeändertEventArgs> SternGeändert;
		public void OnSternGeändert(Point point, bool verschwunden)
		{
			EventHandler<SternGeändertEventArgs> sternGeändert = SternGeändert;
			if (sternGeändert != null)
			{
				sternGeändert(this, new SternGeändertEventArgs(point, verschwunden));
			}
		}

		public event EventHandler<LebenVerlorenEventArgs> LebenVerloren;
		public void OnLebenVerloren(double Leben)
		{
			EventHandler<LebenVerlorenEventArgs> lebenVerloren = LebenVerloren;
			if (lebenVerloren != null)
			{
				lebenVerloren(this, new LebenVerlorenEventArgs(Leben));
			}
		}

		public event EventHandler<KisteEventArgs> KisteGeändert;
		public void OnKisteHinzu(WaffenkisteModel kiste, bool aufgesammelt, double x)
		{
			EventHandler<KisteEventArgs> Waffenkiste = KisteGeändert;
			if (Waffenkiste != null)
			{
				Waffenkiste(this, new KisteEventArgs(kiste, aufgesammelt, x));
			}
		}

		public event EventHandler<BossEventArgs> BossEvent;
		public void OnBossSchadenErlitten(double leben, bool angriff, InvaderBoss boss)
		{
			EventHandler<BossEventArgs> bossEvent = BossEvent;
			if (bossEvent != null)
			{
				bossEvent(this, new BossEventArgs(leben, angriff, boss));
			}
		}
	}
}
