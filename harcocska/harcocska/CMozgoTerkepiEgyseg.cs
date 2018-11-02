using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
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

    public class CKatona : CMozgoTerkepiEgyseg
    {
        public CKatona()
        {
            elet = 14;
            range = 3;
            vedekezesikepesseg = 2;
            tamadasikepesseg = 3;
            bitmap = "katona";
            Console.WriteLine("Katona gyartas");
            
        }

    }
    public class CTank : CMozgoTerkepiEgyseg
    {
        public CTank()
        {
            elet = 15;
            range = 4;
            vedekezesikepesseg = 4;
            tamadasikepesseg = 6;
            bitmap = "tank";
            Console.WriteLine("Tank gyartas");
        }
    }

}
