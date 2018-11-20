using System;
using System.Collections.Generic;

using System.IO;
using System.Xml.Serialization;
using System.Timers;
using System.Diagnostics;
using network;

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
		Timer AllapotValtoTimer = new Timer();
        [NonSerialized()]
        [XmlIgnoreAttribute]
        public CHalozatiSzerver wsSzerver;
        [NonSerialized()]
        [XmlIgnoreAttribute]
        public CHalozatiKliens halozatiKliens;

        public static string host = "";
        public static int port = 11000;
        public static bool isServer = false;

        public static Random sorsolás = new Random();
		#endregion

		public CGame()
        {
			
		}

		public void init1()
		{
			if (AllapotValtoTimer==null)
				AllapotValtoTimer = new Timer();
			AllapotValtoTimer.Elapsed += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = 2000;

            halozatiKliens = new CHalozatiKliens(CGame.host, CGame.port, "");
            halozatiKliens.autoconnect = false;
            //halozatiKliens.HalozatiEsemeny += new HalozatiKliensEvent(KliensEsemeny_kezelo);

            run();
			



		}
		public void init() {


			Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
			

			CJatekos jatekos1 = new CJatekos("aaaaa", ESzin.piros);
			CJatekos jatekos2 = new CJatekos("bbbbb", ESzin.kek);
			CJatekos jatekos3 = new CJatekos("ccccc", ESzin.sarga);
            CBank bankocska1 = new CBank();
            bankocska1.penztermelokepesseg = 0.1;
            jatekos1.bank = bankocska1;

            CBank bankocska2 = new CBank();
            bankocska2.penztermelokepesseg = 0.2;
            jatekos2.bank = bankocska2;

            CBank bankocska3 = new CBank();
            bankocska3.penztermelokepesseg = 0.3;
            jatekos3.bank = bankocska3;




            jatekosok.Add(jatekos1);
			jatekosok.Add(jatekos2);
			jatekosok.Add(jatekos3);

			terkep = new CTerkep(this);


			if (AllapotValtoTimer == null)
				AllapotValtoTimer = new Timer();
			AllapotValtoTimer.Elapsed += AllapotValtoTimer_Tick;
			AllapotValtoTimer.Interval = 2000;

			Console.WriteLine("elokeszulet");

			jatekos1.f = new CFejlesztes();
			jatekos2.f = new CFejlesztes();
			jatekos3.f = new CFejlesztes();

			CKatona e0 = new CKatona();
            e0.aktualisCella = terkep.sorok[7][7];
			e0.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e0);

			CKatona e1 = new CKatona();
			e1.jatekos = jatekos1;
			jatekos1.egysegekLista.Add(e1);
            e1.aktualisCella = terkep.sorok[9][17];

            CTank e2 = new CTank();
            e2.jatekos = jatekos2;
            e2.aktualisCella = terkep.sorok[2][10];
            jatekos2.egysegekLista.Add(e2);

            CTank e3 = new CTank();
            jatekos3.egysegekLista.Add(e3);
            e3.aktualisCella = terkep.sorok[1][8];
            e3.jatekos = jatekos3;


        }

        /// <summary>
        /// Szerver mód
        /// </summary>
        public void Listen()
        {
            if (wsSzerver == null)
            {
                wsSzerver = new CHalozatiSzerver(CGame.port);

                //wsSzerver.KliensekEsemenyei += new KliensKezeloHalozatiEvent(KliensekEsemenyei_kezelo);
                wsSzerver.serverListenStart();
            }
            else
            {
                if (!wsSzerver.listening)
                    wsSzerver.serverListenStart();
            }
        }

        /// <summary>
        /// Szerver mód
        /// </summary>
        public void ListenStop()
        {
            if (wsSzerver != null)
            {
                wsSzerver.serverListenStop();
            }
        }

        /// <summary>
        /// Kliens mód
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool KliensEsemeny_kezelo(object sender, string command)
        {
            switch (command)
            {
                case "kapcsolodva":
                    
                    //notifyIcon1.Icon = Properties.Resources.zold;
                    return true;
               


                     

            }
            return false;
        }

        /// <summary>
        /// Szerver mód
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool KliensekEsemenyei_kezelo(object sender, string command)
        {
            switch (command)
            {
                case "process":

                default:
                    Trace.WriteLine("Default case");
                    return false;
            }
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
                    penzosztas();
                    aktualisallapot = EJatekAllapotok.fejlesztes;
					Console.WriteLine("fejlesztes");
					break;

				case EJatekAllapotok.fejlesztes:
					foreach (CJatekos j in jatekosok)
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
					//terkep.terkeprajzolas();
					Console.WriteLine("egysegmozgatas");
					break;

				case EJatekAllapotok.egysegmozgatas:
					aktualisallapot = EJatekAllapotok.harc;
					Console.WriteLine("harc");
					//terkep.terkeprajzolas();
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

        private void penzosztas()
        {
            foreach (CJatekos j in jatekosok)
            {
                int hanyteruletemvan = 0;
                foreach (List<CTerkepiCella> sor in terkep.sorok)
                {
                    foreach (CTerkepiCella tc in sor) {
                        if (tc.tulaj!=null && tc.tulaj.nev == j.nev)
                            hanyteruletemvan++;
                    }
                }
                
                j.penztarca += (int)(j.bank.penztermelokepesseg * hanyteruletemvan);
            }
        }

        public void start()
		{
			isRunning = true;
		}

		public void mindenkiFeltamasztasa()
		{
			foreach (CJatekos j in jatekosok)
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
