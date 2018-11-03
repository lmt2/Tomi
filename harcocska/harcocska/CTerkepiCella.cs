using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace harcocska
{
	[Serializable()]
	public class CTerkepiCella
    {
        public ECellaTipus cellaTipus { get; set; }
        public CJatekos tulaj;
		public int Sor { get; set; }
		public int Oszlop { get; set; }
		public List<CTerkepiCella> extraSzomszed { get; set; }
		public CTerkepiCella() { }

		public CTerkepiCella(int sor, int oszlop) {
			Oszlop = oszlop;
			Sor = sor;
			extraSzomszed = null;
		}

		public static int offsetX()
		{
			return (int)(Math.Cos(Math.PI * 60 / 180.0) * App.jatek.oldalhossz);
		}
		public static int offsetY()
		{
			return (int)(Math.Sin(Math.PI * 60 / 180.0) * App.jatek.oldalhossz);
		}

		public Point getScreenCoord()
		{
			Point p1 = new Point();
			if ((Oszlop % 2) == 0)
			{
				p1.X = (int)(Oszlop * offsetX() + Oszlop * App.jatek.oldalhossz);
				p1.Y = (int)(Sor * 2 * offsetY());
			}
			else
			{
				p1.X = (int)(Oszlop * offsetX() + Oszlop * App.jatek.oldalhossz);
				p1.Y = (int)(Sor * 2 * offsetY() + offsetY());
			}

			return p1;
		}

		public PointCollection getScreenCoords()
		{
			Point p1 = getScreenCoord();

			Point p2 = p1;
			p2.X += offsetX();
			p2.Y += offsetY();
			
			Point p3 = p2;
			p3.X += App.jatek.oldalhossz;

			Point p4 = p3;
			p4.X += offsetX();
			p4.Y -= offsetY();

			Point p5 = p4;
			p5.X -= offsetX();
			p5.Y -= offsetY();

			Point p6 = p5;
			p6.X -= App.jatek.oldalhossz;

			PointCollection curvePoints=new PointCollection();
			curvePoints.Add(p1);
			curvePoints.Add(p2);
			curvePoints.Add(p3);
			curvePoints.Add(p4);
			curvePoints.Add(p5);
			curvePoints.Add(p6);

			return curvePoints;
		}
	}
}
