using System;
using System.Collections.Generic;


namespace harcocska
{
	[Serializable()]
	public class CTerkepiCella
    {
        public ECellaTipus cellaTipus { get; set; }
        public CJatekos tulaj;
		public int Sor { get; set; }
		public int Oszlop { get; set; }
		public List<CTerkepiCella> extraSzomszed { get; set; }
		public CTerkepiCella() { }

		public CTerkepiCella(int sor, int oszlop) {
			Oszlop = oszlop;
			Sor = sor;
			extraSzomszed = null;
		}

		
	}
}
