using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;


namespace harcocska
{
    public class CGame
    {
		#region members
		public List<CJatekos> jatekosok = new List<CJatekos>();
        public CTerkep terkep = null;
		bool isRunning = false;
		bool update = false;
		int korszamlalo=0;
		EJatekAllapotok aktualisallapot = EJatekAllapotok.elokeszulet;
		DispatcherTimer AllapotValtoTimer = new DispatcherTimer();
		public int oldalhossz { get; set; }
		#endregion

		public CGame()
        {
			
		}


		public void init() {
			Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
			oldalhossz = 30;

			CJatekos jatekos1 = new CJatekos("aaaaa", ESzin.piros);
			CJatekos jatekos2 = new CJatekos("bbbbb", ESzin.kek);
			CJatekos jatekos3 = new CJatekos("ccccc", ESzin.sarga);
			jatekosok.Add(jatekos1);
			jatekosok.Add(jatekos2);
			jatekosok.Add(jatekos3);

			terkep = new CTerkep();

			AllapotValtoTimer.Tick += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = new TimeSpan(0, 0, 1);

			Console.WriteLine("elokeszulet");

			jatekos1.f = new CFejlesztes();
			jatekos2.f = new CFejlesztes();
			jatekos3.f = new CFejlesztes();

			CKatona e0 = new CKatona();
			e0.aktualisCella = App.jatek.terkep.cellak[7][7];
			e0.range = 4;
			e0.bitmap = "katona.png";
			e0.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e0);

			CKatona e1 = new CKatona();
			e1.aktualisCella = App.jatek.terkep.cellak[1][0];
			e1.range = 4;
			e1.bitmap = "katona.png";
			e1.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e1);

			CMozgoTerkepiEgyseg e2 = new CMozgoTerkepiEgyseg();
			e2.aktualisCella = App.jatek.terkep.cellak[2][2];
			e2.bitmap = "tank.png";
			e2.range = 4;
			e2.jatekos = jatekos2;
			jatekos2.egysegekLista.Add(e2);

			CMozgoTerkepiEgyseg e3 = new CMozgoTerkepiEgyseg();
			e3.aktualisCella = App.jatek.terkep.cellak[5][1];
			e3.bitmap = "tank.png";
			e3.jatekos = jatekos3;
			jatekos3.egysegekLista.Add(e3);

			//App.jatek.terkep.Tavolsag(App.jatek.terkep.cellak[5][5], App.jatek.terkep.cellak[2][7]);
		}

		private void AllapotValtoTimer_Tick(object sender, EventArgs e)
		{
			if (!isRunning)
				return;
			if (!update)
				return;

			switch (aktualisallapot)
			{
				case EJatekAllapotok.elokeszulet:
					aktualisallapot = EJatekAllapotok.penzosztas;
					Console.WriteLine("Körszámláló:{0}", korszamlalo);
					Console.WriteLine("penzosztas");
					break;

				case EJatekAllapotok.penzosztas:
					aktualisallapot = EJatekAllapotok.fejlesztes;
					Console.WriteLine("fejlesztes");
					break;

				case EJatekAllapotok.fejlesztes:
					aktualisallapot = EJatekAllapotok.egysegmozgatas;
					Console.WriteLine("egysegmozgatas");
					break;

				case EJatekAllapotok.egysegmozgatas:
					aktualisallapot = EJatekAllapotok.harc;
					Console.WriteLine("harc");
					korszamlalo++;
					break;

				case EJatekAllapotok.harc:
					aktualisallapot = EJatekAllapotok.penzosztas;
					Console.WriteLine("Körszámláló:{0}", korszamlalo);
					Console.WriteLine("penzosztas");
					break;

			}

			update = false;

		}

		public void start()
		{
			isRunning = true;
		}
		public void lepes()
		{
			update = true;
		}

		public void run()
		{
			AllapotValtoTimer.Start();
		}
    }

	
}
