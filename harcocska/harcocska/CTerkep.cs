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
		public CTerkepiEgyseg aktualisEgyseg { get; set; }

		public List<List<CTerkepiCella>> cellak = new List<List<CTerkepiCella>>();
        public CTerkep()
        {
			terkepAllapot = ETerkepAllapot.szabad;
			szelesseg = 20;
            magassag = 20;
			//int szamlalo = 0;
            for (int j = 0; j < magassag; j++)
            {
                List<CTerkepiCella> sor = new List<CTerkepiCella>();
                for (int i = 0; i < szelesseg; i++)
                {
                    CTerkepiCella c = new CTerkepiCella(i,j);
                    c.cellaTipus = ECellaTipus.viz;
                    sor.Add(c);
					//szamlalo++;
					//if (szamlalo>0 && szamlalo < 133) { c.tulaj = App.jatek.jatekosok[0]; }
					//else if (szamlalo >= 133 && szamlalo < 266) { c.tulaj = App.jatek.jatekosok[1]; }
					//else { c.tulaj = App.jatek.jatekosok[2]; }


				}
                cellak.Add(sor);
            }
        }
		
		public void terkeprajzolas()
		{
			canvas.Children.Clear();
			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = App.jatek.terkep.cellak[i][j].getScreenCoords();

					
					Polygon p = new Polygon();
					RenderOptions.SetEdgeMode((DependencyObject)p, EdgeMode.Aliased);
					p.Stroke = Brushes.Black;
					
					if (App.jatek.terkep.cellak[i][j].tulaj != null) { 
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
					}
					else
					{
						p.Fill = Brushes.Gray;
					}
					if (terkepAllapot == ETerkepAllapot.egysegmozgatas) {
						if (Tavolsag(cellak[i][j], aktualisEgyseg.aktualisCella) <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
						{
							p.StrokeThickness = 2;
							p.Stroke = Brushes.Yellow;
						}
						else
						{
							
							//p.StrokeThickness = 0.8;
							p.Stroke = Brushes.Black;
						}
					}
					else
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

            MenuItem item1 = new MenuItem();
            item1.Header = "harc";
            item1.Click += delegate { MyImage_Harc(); };
            contextMenu.Items.Add(item1);


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


                    if (te.elet==0) { 
                    Image myImage1 = new Image();
                    myImage1.Width = 20;

                    // Create source
                    BitmapImage myBitmapImage1 = new BitmapImage();

                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage1.BeginInit();

                    myBitmapImage1.UriSource = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "megsemmisult.png"), UriKind.Absolute);
                    

                    myBitmapImage1.DecodePixelWidth = 20;
                    myBitmapImage1.EndInit();
                    myImage1.Source = myBitmapImage1;
                    canvas.Children.Add(myImage1);
                    Canvas.SetTop(myImage1, te.aktualisCella.getScreenCoord().Y - App.jatek.oldalhossz / 2);
                    Canvas.SetLeft(myImage1, te.aktualisCella.getScreenCoord().X + App.jatek.oldalhossz / 2);
                    }
                    //((IMozgoTerkepiEgyseg)te).MozgasJobbra();

                }
			}
		}

		public CTerkepiCella getTerkepiCellaAtScreenPosition(Point p)
		{
			CTerkepiCella ret = null;
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

        public CTerkepiEgyseg getTerkepiEgysegAtScreenPosition(Point p)
        {
            CTerkepiEgyseg ret = null;
            CTerkepiCella tc= getTerkepiCellaAtScreenPosition(p);
            foreach (CJatekos j in App.jatek.jatekosok)
            {
                foreach (CTerkepiEgyseg te in j.egysegekLista)
                {
                    if (te.aktualisCella.X == tc.X && te.aktualisCella.Y == tc.Y) {
                        return te;
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


		public CTerkepiCella BalraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.X % 2) == 0)
			{
				ret = cellak[tc.Y - 1][tc.X - 1];
			}
			else
			{
				ret = cellak[tc.Y][tc.X - 1];
			}
			return ret;
		}

		public CTerkepiCella BalraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.X % 2) == 0)
			{
				ret = cellak[tc.Y][tc.X - 1];
			}
			else
			{
				ret = cellak[tc.Y + 1][tc.X - 1];
			}
			return ret;
		}

		public CTerkepiCella JobbraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.X % 2) == 0)
			{
				ret = cellak[tc.Y - 1][tc.X + 1];
			}
			else
			{
				ret = cellak[tc.Y][tc.X + 1];
			}
			return ret;
		}
		public CTerkepiCella JobbraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.X % 2) == 0)
			{
				ret = cellak[tc.Y][tc.X + 1];
			}
			else
			{
				ret = cellak[tc.Y + 1][tc.X + 1];
			}
			return ret;
		}
		public CTerkepiCella Le(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Y + 1][tc.X];
			return ret;
		}
		public CTerkepiCella Fel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Y - 1][tc.X];
			return ret;
		}

		public CTerkepiCella Balra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Y][tc.X - 1];
			return ret;
		}
		public CTerkepiCella Jobbra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Y][tc.X + 1];
			return ret;
		}

		public bool EgySorbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false; ;
			if (tc1.Y == tc2.Y)
				ret = true;
			return ret;
		}

		public bool EgyOszlopbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false; ;
			if (tc1.X == tc2.X)
				ret = true;
			return ret;
		}

		public int Tavolsag(CTerkepiCella from, CTerkepiCella to)
		{
			int tavolsag=0;

			CTerkepiCella dummy = to;

			while (!(dummy.X == from.X && dummy.Y == from.Y))
			{
                int lepesVolt = 0;



				if (!EgySorbanVannak(from, dummy) && !EgyOszlopbanVannak(from, dummy))
				{
					if (dummy.X > from.X && dummy.Y > from.Y)
					{
						if (BalraFel(dummy).tulaj != null)
						{
							dummy = BalraFel(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
					if (dummy.X < from.X && dummy.Y > from.Y)
					{
						if (JobbraFel(dummy).tulaj != null)
						{
							dummy = JobbraFel(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
					if (dummy.X < from.X && dummy.Y < from.Y)
					{
						if (JobbraLe(dummy).tulaj != null)
						{
							dummy = JobbraLe(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
					if (dummy.X > from.X && dummy.Y < from.Y)
					{
						if (BalraLe(dummy).tulaj != null)
						{
							dummy = BalraLe(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}

				}



				if (EgySorbanVannak(from, dummy))
				{
					if (dummy.X > from.X)
					{
						if (Balra(dummy).tulaj != null)
						{
							dummy = Balra(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
					if (dummy.X < from.X)
					{
						if (Jobbra(dummy).tulaj != null)
						{
							dummy = Jobbra(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
				}


				if (EgyOszlopbanVannak(from, dummy))
				{
					if (dummy.Y > from.Y)
					{
						if (Fel(dummy).tulaj != null)
						{
							dummy = Fel(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
					if (dummy.Y < from.Y)
					{
						if (Le(dummy).tulaj != null)
						{
							dummy = Le(dummy);
							lepesVolt++;
							tavolsag++;
						}
					}
				}


				if (lepesVolt == 0)
				{
					tavolsag = 100000;
					break;
				}


			}
            //Console.WriteLine("Távolság:{0}", tavolsag);
			return tavolsag;
		}

		

		private void MyImage_Mozgas()
		{
			terkepAllapot = ETerkepAllapot.egysegmozgatas;
		}

        private void MyImage_Harc()
        {
            terkepAllapot = ETerkepAllapot.harc;
        }

        private void MyImage_RightMouseDown(object sender, MouseButtonEventArgs e)
		{

			aktualisEgyseg=((CTerkepiImage)sender).terkepiEgyseg;
		}

		private void MyImage_RightMouseUp(object sender, MouseButtonEventArgs e)
		{
			//mozgatottEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;
		}

		private void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			
		}

		public void OnLeftMouseDown(Point windowCoord)
		{
            if (terkepAllapot == ETerkepAllapot.harc)
            {
                if (Tavolsag(getTerkepiCellaAtScreenPosition(windowCoord), aktualisEgyseg.aktualisCella) > ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
                {
                    return;
                }
                IHarcoloTerkepiEgyseg tempHarcoloEgyseg = null;
                tempHarcoloEgyseg = (IHarcoloTerkepiEgyseg)aktualisEgyseg;
                if (tempHarcoloEgyseg != null)
                    tempHarcoloEgyseg.Tamadas(getTerkepiEgysegAtScreenPosition(windowCoord));
                terkepAllapot = ETerkepAllapot.szabad;
            }
            if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
			{
				if (Tavolsag(getTerkepiCellaAtScreenPosition(windowCoord), aktualisEgyseg.aktualisCella) > ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
				{
					return;
				}
				IMozgoTerkepiEgyseg tempMozgoEgyseg = null;
				tempMozgoEgyseg = (IMozgoTerkepiEgyseg)aktualisEgyseg;
				if (tempMozgoEgyseg != null)
				{
					CTerkepiCella cella= getTerkepiCellaAtScreenPosition(windowCoord);
					if (cella.tulaj!=null)
						tempMozgoEgyseg.mozgasIde(cella);
				}
					
				terkepAllapot = ETerkepAllapot.szabad;
			}
		}
	}


}