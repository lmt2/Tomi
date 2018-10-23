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


		public CJatekos()
        {

        }
        public CJatekos(string n)
        {
            nev = n;
            CTerkepiEgyseg e1 = new CTerkepiEgyseg();
            e1.jatekos = this;
            egysegekLista.Add(e1);



        }
    }
}
