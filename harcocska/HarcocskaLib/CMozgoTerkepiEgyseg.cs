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
		public bool lepettMar { get; set; }

		public CMozgoTerkepiEgyseg()
		{
			range = 1;
		}

		void IMozgoTerkepiEgyseg.mozgasCellara(CTerkep t, CTerkepiCella to)
		{
			if (t.tavolsagTabla[to.Sor][to.Oszlop] > ((CMozgoTerkepiEgyseg)this).range)
			{
				return;
			}
			((CMozgoTerkepiEgyseg)this).lepettMar = true;
			this.aktualisCella = to;
		}

		
	}
	
	

}
