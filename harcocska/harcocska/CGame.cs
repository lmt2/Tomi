using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Xml.Serialization;


namespace harcocska
{
	[Serializable()]
	public class CGame
    {
		#region members
		public List<CJatekos> jatekosok = new List<CJatekos>();
        public CTerkep terkep = null;
		bool isRunning = false;
		bool update = false;
		int korszamlalo=0;
		public EJatekAllapotok aktualisallapot = EJatekAllapotok.elokeszulet;
		[NonSerialized()]
		[XmlIgnoreAttribute]
		DispatcherTimer AllapotValtoTimer = new DispatcherTimer();
		public int oldalhossz { get; set; }

		public static Random sorsolás = new Random();
		#endregion

		public CGame()
        {
			
		}

		public void init1()
		{
			if (AllapotValtoTimer==null)
				AllapotValtoTimer = new DispatcherTimer();
			AllapotValtoTimer.Tick += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = new TimeSpan(0, 0, 1);
			App.jatek.terkep.canvas.AllowDrop = true;

			App.jatek.run();
			App.jatek.terkep.terkeprajzolas();



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

			if (AllapotValtoTimer == null)
				AllapotValtoTimer = new DispatcherTimer();
			AllapotValtoTimer.Tick += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = new TimeSpan(0, 0, 1);

			Console.WriteLine("elokeszulet");

			jatekos1.f = new CFejlesztes();
			jatekos2.f = new CFejlesztes();
			jatekos3.f = new CFejlesztes();

			CKatona e0 = new CKatona();
            e0.aktualisCella = App.jatek.terkep.cellak[7][7];
			e0.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e0);

			CKatona e1 = new CKatona();
			e1.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e1);
            e1.aktualisCella = App.jatek.terkep.cellak[9][17];

            CTank e2 = new CTank();
            e2.jatekos = jatekos2;
            e2.aktualisCella = App.jatek.terkep.cellak[2][10];
            jatekos2.egysegekLista.Add(e2);

            CTank e3 = new CTank();
            jatekos3.egysegekLista.Add(e3);
            e3.aktualisCella = App.jatek.terkep.cellak[1][8];
            e3.jatekos = jatekos3;


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
					foreach (CJatekos j in App.jatek.jatekosok)
					{
						foreach (CTerkepiEgyseg te in j.egysegekLista)
						{
							if ((CMozgoTerkepiEgyseg)te!=null)
							{
								((CMozgoTerkepiEgyseg)te).lepettMar = false;
							}
						}
					}
					aktualisallapot = EJatekAllapotok.egysegmozgatas;
					App.jatek.terkep.terkeprajzolas();
					Console.WriteLine("egysegmozgatas");
					break;

				case EJatekAllapotok.egysegmozgatas:
					aktualisallapot = EJatekAllapotok.harc;
					Console.WriteLine("harc");
					App.jatek.terkep.terkeprajzolas();
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

		public void mindenkiFeltamasztasa()
		{
			foreach (CJatekos j in App.jatek.jatekosok)
			{

				foreach (CTerkepiEgyseg te in j.egysegekLista)
				{

					te.elet = te.eletOriginal;

				}
			}
		}
		public void lepes()
		{
			update = true;
		}

		public void run()
		{
			AllapotValtoTimer.Start();
		}
		public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
		{
			using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
			{
				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binaryFormatter.Serialize(stream, objectToWrite);
			}
		}
		public static T ReadFromBinaryFile<T>(string filePath)
		{
			using (Stream stream = File.Open(filePath, FileMode.Open))
			{
				var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				return (T)binaryFormatter.Deserialize(stream);
			}
		}

	}

	


}
