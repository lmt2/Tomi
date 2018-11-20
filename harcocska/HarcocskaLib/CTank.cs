using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    [Serializable()]
    public class CTank : CMozgoTerkepiEgyseg
    {
        public CTank()
        {
            eletOriginal = 15;
            elet = eletOriginal;
            range = 4;
            vedekezesikepesseg = 4;
            tamadasikepesseg = 6;
            bitmap = "tank";
            Console.WriteLine("Tank gyartas");
        }
    }
}
