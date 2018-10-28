﻿using System;
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
        public CTerkepiEgyseg() {
            elet = 10;
        }
        void IHarcoloTerkepiEgyseg.Tamadas(CTerkepiEgyseg te)
		{
            te.elet = 0;
		}

	}
}
