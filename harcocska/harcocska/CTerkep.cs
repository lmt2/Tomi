using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    public class CTerkep
    {
        public int szelesseg { get; set; }
        public int magassag { get; set; }

        public List<List<CTerkepiCella>> cellak = new List<List<CTerkepiCella>>();
        public CTerkep()
        {
            szelesseg = 20;
            magassag = 20;
            for (int j = 0; j < magassag; j++)
            {
                List<CTerkepiCella> sor = new List<CTerkepiCella>();
                for (int i = 0; i < szelesseg; i++)
                {
                    CTerkepiCella c = new CTerkepiCella(i,j);
                    c.cellaTipus = ECellaTipus.viz;
                    sor.Add(c);
                }
                cellak.Add(sor);
            }
        }
    }
}