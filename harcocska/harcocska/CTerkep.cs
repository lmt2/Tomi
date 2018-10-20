using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    class CTerkep
    {
        int szelesseg { get; set; }
        int magassag { get; set; }

        List<List<CTerkepiCella>> cellak = new List<List<CTerkepiCella>>();
        public CTerkep()
        {
            szelesseg = 20;
            magassag = 20;
            for (int j = 0; j < magassag; j++)
            {
                List<CTerkepiCella> sor = new List<CTerkepiCella>();
                for (int i = 0; i < szelesseg; i++)
                {
                    CTerkepiCella c = new CTerkepiCella();
                    c.cellaTipus = ECellaTipus.viz;
                    sor.Add(c);
                }
                cellak.Add(sor);
            }
        }
    }
}