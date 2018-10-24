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
		public CTerkepiCella aktualisCella { get; set; }
		public string bitmap { get; set; }

		void IHarcoloTerkepiEgyseg.Tamadas()
		{
			
		}

	}
}
