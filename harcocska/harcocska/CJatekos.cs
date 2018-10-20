using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    class CJatekos
    {
        string nev { get; set; }
        List<CTerkepiEgyseg> egysegekLista = new List<CTerkepiEgyseg>();

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
