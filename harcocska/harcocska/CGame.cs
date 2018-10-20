using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
    class CGame
    {
        public List<CJatekos> jatekosok = new List<CJatekos>();
        CTerkep terkep = new CTerkep();


        public CGame()
        {
            CJatekos j1 = new CJatekos("elso");
            CJatekos j2 = new CJatekos("masodik");
            CJatekos j3 = new CJatekos("harmadik");
            jatekosok.Add(j1);
            jatekosok.Add(j2);
            jatekosok.Add(j3);



        }
    }
    enum ECellaTipus
    {
        szarazfold,
        viz
    }
}
