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
        public CTerkep terkep = new CTerkep();
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

			AllapotValtoTimer.Tick += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = new TimeSpan(0, 0, 1);

			Console.WriteLine("elokeszulet");

			jatekos1.f = new CFejlesztes();
			jatekos2.f = new CFejlesztes();
			jatekos3.f = new CFejlesztes();
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

	public enum EJatekAllapotok
	{
		elokeszulet,
		penzosztas,
		fejlesztes,
		egysegmozgatas,
		harc,
		vege

	}
    public enum ECellaTipus
    {
        szarazfold,
        viz
    }
}
