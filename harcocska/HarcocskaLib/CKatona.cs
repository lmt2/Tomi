using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    [Serializable()]
    public class CKatona : CMozgoTerkepiEgyseg
    {
        public CKatona()
        {
            eletOriginal = 14;
            elet = eletOriginal;
            range = 3;
            vedekezesikepesseg = 3;
            tamadasikepesseg = 5;
            bitmap = "katona";
            Console.WriteLine("Katona gyartas");

        }

    }
}
