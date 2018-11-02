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
		}
		public System.Windows.Media.SolidColorBrush getSzin()
		{
			switch (szin)
			{
				case ESzin.piros:
					return System.Windows.Media.Brushes.PaleVioletRed;
					break;

				case ESzin.kek:
					return System.Windows.Media.Brushes.LightBlue;
					break;

				case ESzin.sarga:
					return System.Windows.Media.Brushes.LightYellow;
					break;


			}
			return System.Windows.Media.Brushes.Black;
		}

        public System.Windows.Media.SolidColorBrush getVonalszín()
        {
            switch (szin)
            {
                case ESzin.piros:
                    return System.Windows.Media.Brushes.BlueViolet;
                    break;

                case ESzin.kek:
                    return System.Windows.Media.Brushes.DarkBlue;
                    break;

                case ESzin.sarga:
                    return System.Windows.Media.Brushes.Gold;
                    break;


            }
            return System.Windows.Media.Brushes.Black;
        }
    }
	
}
