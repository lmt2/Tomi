using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
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
			CKatona e1 = new CKatona();
			e1.aktualisCella = App.jatek.terkep.cellak[1][0];
			e1.bitmap = "katona.png";
            e1.jatekos = this;
            egysegekLista.Add(e1);
			CMozgoTerkepiEgyseg e2 = new CMozgoTerkepiEgyseg();
			e2.aktualisCella = App.jatek.terkep.cellak[2][2];
			e2.bitmap = "tank.png";
			e2.jatekos = this;
			egysegekLista.Add(e2);

			CMozgoTerkepiEgyseg e3 = new CMozgoTerkepiEgyseg();
			e3.aktualisCella = App.jatek.terkep.cellak[5][1];
			e3.bitmap = "tank.png";
			e3.jatekos = this;
			egysegekLista.Add(e3);



		}
    }
	public enum ESzin
	{
		piros,
		kek,
		zöld,
		sarga,
		barna
	}
}
