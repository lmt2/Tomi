using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    public class CTerkepiEgyseg
    {
        public CJatekos jatekos { get; set; }
		public CTerkepiCella aktualisCella { get; set; }
		public void jobbra() {
			aktualisCella=App.jatek.terkep.right(aktualisCella);
		}
    }
}
