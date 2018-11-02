using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    public class CTerkepiEgyseg : IHarcoloTerkepiEgyseg
	{
        public CJatekos jatekos { get; set; }
        public int elet { get; set; }
        public CTerkepiCella aktualisCella { get; set; }
		public string bitmap { get; set; }
        public int tamadasikepesseg { get; set; }
        public int vedekezesikepesseg { get; set; }

        public CTerkepiEgyseg() {
            elet = 10;
        }




        void IHarcoloTerkepiEgyseg.Tamadas(CTerkepiEgyseg tamadott)
		{

            if (tamadott.jatekos == this.jatekos) {
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
            Random sorsolás = new Random();
            sorsolás.Next(0,veletlentomb.Count);
            int eredmény = sorsolás.Next();
            if (eredmény == 1)
            {
                Console.WriteLine("nyert:{0}", this.jatekos.nev);
                tamadott.elet -= this.tamadasikepesseg;
                this.elet -= tamadott.vedekezesikepesseg;
            }
            else
            {
                Console.WriteLine("nyert:{0}", tamadott.jatekos.nev);
                tamadott.elet -= this.tamadasikepesseg - 1;
                this.elet -= tamadott.vedekezesikepesseg + 1;
            }
            if (tamadott.elet < 0)
                tamadott.elet = 0;

            if (this.elet < 0)
                this.elet = 0;
        }

	}
}
