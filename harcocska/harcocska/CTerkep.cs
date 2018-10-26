using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Drawing;

namespace harcocska
{
    public class CTerkep
    {
        public int szelesseg { get; set; }
        public int magassag { get; set; }
		public Canvas canvas { get; set; }
		public ETerkepAllapot terkepAllapot { get; set; }
		public CTerkepiEgyseg mozgatottEgyseg { get; set; }

		public List<List<CTerkepiCella>> cellak = new List<List<CTerkepiCella>>();
        public CTerkep()
        {
			terkepAllapot = ETerkepAllapot.szabad;
			szelesseg = 20;
            magassag = 20;
			int szamlalo = 0;
            for (int j = 0; j < magassag; j++)
            {
                List<CTerkepiCella> sor = new List<CTerkepiCella>();
                for (int i = 0; i < szelesseg; i++)
                {
                    CTerkepiCella c = new CTerkepiCella(i,j);
                    c.cellaTipus = ECellaTipus.viz;
                    sor.Add(c);
					szamlalo++;
					if (szamlalo>0 && szamlalo < 133) { c.tulaj = App.jatek.jatekosok[0]; }
					else if (szamlalo >= 133 && szamlalo < 266) { c.tulaj = App.jatek.jatekosok[1]; }
					else { c.tulaj = App.jatek.jatekosok[2]; }


				}
                cellak.Add(sor);
            }
        }
		public CTerkepiCella right(CTerkepiCella c)
		{
			CTerkepiCella ret=null;
			foreach (List<CTerkepiCella> sor in cellak)
			{
				foreach (CTerkepiCella cell in sor)
				{
					if (cell == c)
					{
						ret = sor[cell.X+1];
					}
				}

			}
			return ret;
		}
		public void terkeprajzolas()
		{
			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = App.jatek.terkep.cellak[i][j].getScreenCoords();

					Polygon p = new Polygon();
					p.Stroke = Brushes.Black;
					
					switch (App.jatek.terkep.cellak[i][j].tulaj.szin)
					{
						case ESzin.piros:
							p.Fill = Brushes.PaleVioletRed;
							break;

						case ESzin.kek:
							p.Fill = Brushes.LightBlue;
							break;

						case ESzin.sarga:
							p.Fill = Brushes.LightYellow;
							break;


					}
					p.StrokeThickness = 1;
					p.HorizontalAlignment = HorizontalAlignment.Left;
					p.VerticalAlignment = VerticalAlignment.Center;
					p.Points = curvePoints;
					canvas.Children.Add(p);

				}
			}

			CTerkepiImage myImage = null;
			ContextMenu contextMenu = new ContextMenu();

			//contextMenu.Items.Add("Mozgás");

			//contextMenu.Items.Add("Harc");

			MenuItem item = new MenuItem();

			item.Header = "mozgás";
			item.Click += delegate { MyImage_Mozgas(); };

			contextMenu.Items.Add(item);


			foreach (CJatekos j in App.jatek.jatekosok) { 

				foreach (CTerkepiEgyseg te in j.egysegekLista)
				{

					// Create Image Element
					myImage = new CTerkepiImage(te);
					myImage.Width = 20;

					// Create source
					BitmapImage myBitmapImage = new BitmapImage();

					// BitmapImage.UriSource must be in a BeginInit/EndInit block
					myBitmapImage.BeginInit();

					myBitmapImage.UriSource = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, te.bitmap), UriKind.Absolute);


					myBitmapImage.DecodePixelWidth = 20;
					myBitmapImage.EndInit();
					//set image source
					myImage.Source = myBitmapImage;
					canvas.Children.Add(myImage);

					myImage.MouseUp += MyImage_MouseUp;
					myImage.MouseRightButtonDown += MyImage_RightMouseDown;
					myImage.ContextMenu = contextMenu;

					Canvas.SetTop(myImage, te.aktualisCella.getScreenCoord().Y - App.jatek.oldalhossz / 2);
					Canvas.SetLeft(myImage, te.aktualisCella.getScreenCoord().X + App.jatek.oldalhossz / 2);

					//((IMozgoTerkepiEgyseg)te).MozgasJobbra();

				}
			}





		}

		public CTerkepiCella getTerkepiCellaAtScreenPosition(Point p)
		{
			CTerkepiCella ret = null;
			double MinDist = 10000;

			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = App.jatek.terkep.cellak[i][j].getScreenCoords();

					for (int k = 0; k < 6; k++)
					{
						if (IsPointInPolygon4(curvePoints, p))
						{
							ret = App.jatek.terkep.cellak[i][j];
							return ret;
						}
						
					}

				}
			}

			

			return ret;
		}

		public static bool IsPointInPolygon4(PointCollection polygon, Point testPoint)
		{
			bool result = false;
			int j = polygon.Count() - 1;
			for (int i = 0; i < polygon.Count(); i++)
			{
				if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
				{
					if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
					{
						result = !result;
					}
				}
				j = i;
			}
			return result;
		}

		

		private void MyImage_Mozgas()
		{
			terkepAllapot = ETerkepAllapot.egysegmozgatas;
			
		}

		private void MyImage_RightMouseDown(object sender, MouseButtonEventArgs e)
		{

			mozgatottEgyseg=((CTerkepiImage)sender).terkepiEgyseg;
		}

		private void MyImage_RightMouseUp(object sender, MouseButtonEventArgs e)
		{

			mozgatottEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;
		}

		private void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			
		}

		public void mozgasIde(Point ide)
		{
			if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
			{
				mozgatottEgyseg.aktualisCella = getTerkepiCellaAtScreenPosition(ide);
				terkepAllapot = ETerkepAllapot.szabad;
			}
		}
	}


}