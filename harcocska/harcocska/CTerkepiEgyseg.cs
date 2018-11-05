using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	[Serializable()]
	public class CTerkepiEgyseg : IHarcoloTerkepiEgyseg
	{
        public CJatekos jatekos { get; set; }
		public int eletOriginal { get; set; }
		public int elet { get; set; }
        public CTerkepiCella aktualisCella { get; set; }
		public string bitmap { get; set; }
        public int tamadasikepesseg { get; set; }
        public int vedekezesikepesseg { get; set; }

        public CTerkepiEgyseg()
        {

        }




        void IHarcoloTerkepiEgyseg.Tamadas(CTerkepiEgyseg tamadott)
		{

            if (tamadott==null || tamadott.jatekos == this.jatekos) {
				App.jatek.terkep.terkepAllapot = ETerkepAllapot.szabad;
                return;
            }
            List<int> veletlentomb=new List<int>();
            for (int i = 1; i <= this.tamadasikepesseg; i++) {
                veletlentomb.Add(1);
            }
            for (int i = 1; i <= tamadott.vedekezesikepesseg; i++)
            {
                veletlentomb.Add(0);
            }
            
            
            int eredmény = CGame.sorsolás.Next(0, veletlentomb.Count);
			if (veletlentomb[eredmény] == 1)
            {
				//ha a támadó nyert
                Console.WriteLine("támadó nyert:{0}", this.jatekos.nev);
                tamadott.elet -= this.tamadasikepesseg;
                this.elet -= tamadott.vedekezesikepesseg;
            }
            else
            {
				//ha a védekező nyert
				Console.WriteLine("védekező nyert:{0}", tamadott.jatekos.nev);
                tamadott.elet -= this.tamadasikepesseg - 1;
                this.elet -= tamadott.vedekezesikepesseg ;
            }
            if (tamadott.elet < 0)
                tamadott.elet = 0;

            if (this.elet < 0)
                this.elet = 0;
			App.jatek.terkep.terkepAllapot = ETerkepAllapot.szabad;
		}

	}
}
