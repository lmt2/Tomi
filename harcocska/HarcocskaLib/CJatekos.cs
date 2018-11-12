using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	[Serializable()]
	public class CJatekos
	{
		public string nev { get; set; }
		public List<CTerkepiEgyseg> egysegekLista = new List<CTerkepiEgyseg>();
		public CFejlesztes f { get; set; }
		public ESzin szin { get; set; }


		public CJatekos()
		{

		}
		public CJatekos(string n, ESzin sz)
		{
			nev = n;
			szin = sz;
		}
	}
	
}
