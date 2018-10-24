using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	public class CMozgoTerkepiEgyseg : CTerkepiEgyseg, IMozgoTerkepiEgyseg
	{
		public CMozgoTerkepiEgyseg() {
		}
		void IMozgoTerkepiEgyseg.MozgasJobbra()
		{
			aktualisCella = App.jatek.terkep.right(aktualisCella);
		}
	}

	public class CKatona : CMozgoTerkepiEgyseg
	{
		public CKatona()
		{

		}
		
	}
}
