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
			aktualisCella = App.jatek.terkep.right(aktualisCella);
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

		}

	}
}
