using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	[Serializable()]
	public class CMozgoTerkepiEgyseg : CTerkepiEgyseg, IMozgoTerkepiEgyseg
	{
		public int range { get; set; }

		public CMozgoTerkepiEgyseg()
		{
			range = 1;
		}
		void IMozgoTerkepiEgyseg.MozgasJobbra()
		{
			aktualisCella = App.jatek.terkep.Jobbra(aktualisCella);
		}
		void IMozgoTerkepiEgyseg.mozgasIde(CTerkepiCella tc)
		{
			this.aktualisCella = tc;
		}

		
	}
	[Serializable()]
	public class CKatona : CMozgoTerkepiEgyseg
    {
        public CKatona()
        {
            eletOriginal = 14;
			elet = eletOriginal;
			range = 3;
            vedekezesikepesseg = 3;
            tamadasikepesseg = 5;
            bitmap = "katona";
            Console.WriteLine("Katona gyartas");
            
        }

    }
	[Serializable()]
	public class CTank : CMozgoTerkepiEgyseg
    {
        public CTank()
        {
			eletOriginal = 15;
			elet = eletOriginal;
			range = 4;
            vedekezesikepesseg = 4;
            tamadasikepesseg = 6;
            bitmap = "tank";
            Console.WriteLine("Tank gyartas");
        }
    }

}
