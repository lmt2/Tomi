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

		public List<List<int>> vertex = new List<List<int>>();
		public List<List<int>> latogatottVertex = new List<List<int>>();
		public List<List<int>> dist = new List<List<int>>();
		public List<csucs> prev = new List<csucs>();
		List<csucs> Q = new List<csucs>();
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
					CTerkepiCella c = new CTerkepiCella(j, i);
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
			if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
				Dijkstra(aktualisEgyseg.aktualisCella);

			for (int j = 0; j < magassag; j++)
			{
				for (int i = 0; i < szelesseg; i++)
				{
					PointCollection curvePoints = cellak[j][i].getScreenCoords();


					Polygon p = new Polygon();
					RenderOptions.SetEdgeMode((DependencyObject)p, EdgeMode.Aliased);
					p.Stroke = Brushes.Black;

					if (cellak[j][i].tulaj != null)
					{
						switch (cellak[j][i].tulaj.szin)
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
					if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
					{
						
						//if (Tavolsag(cellak[i][j], aktualisEgyseg.aktualisCella) <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
						//Console.WriteLine("Tavolsag:{0}", dist[j][i]);
						if (dist[j][i] <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
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

					if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
					{
						if (dist[j][i] < 50000)
						{
							TextBlock textBlock = new TextBlock();
							textBlock.Text = dist[j][i].ToString();
							//textBlock.Foreground = new SolidColorBrush(Brushes.Black);
							Canvas.SetLeft(textBlock, cellak[j][i].getScreenCoord().X + App.jatek.oldalhossz / 1.5);
							Canvas.SetTop(textBlock, cellak[j][i].getScreenCoord().Y + App.jatek.oldalhossz / 2-5);
							canvas.Children.Add(textBlock);
						}
					}



					TextBlock textBlock1 = new TextBlock();
					textBlock1.Text = String.Format("{0},{1}",j,i);
					//textBlock.Foreground = new SolidColorBrush(Brushes.Black);
					Canvas.SetLeft(textBlock1, cellak[j][i].getScreenCoord().X + App.jatek.oldalhossz / 1.5);
					Canvas.SetTop(textBlock1, cellak[j][i].getScreenCoord().Y - App.jatek.oldalhossz+2);
					canvas.Children.Add(textBlock1);


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


			foreach (CJatekos j in App.jatek.jatekosok)
			{

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


					if (te.elet == 0)
					{
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
					PointCollection curvePoints = cellak[i][j].getScreenCoords();
					for (int k = 0; k < 6; k++)
					{
						if (IsPointInPolygon4(curvePoints, p))
						{
							ret = cellak[i][j];
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
			CTerkepiCella tc = getTerkepiCellaAtScreenPosition(p);
			foreach (CJatekos j in App.jatek.jatekosok)
			{
				foreach (CTerkepiEgyseg te in j.egysegekLista)
				{
					if (te.aktualisCella.Oszlop == tc.Oszlop && te.aktualisCella.Sor == tc.Sor)
					{
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


		public void Dijkstra(CTerkepiCella startcella)
		{

			int startSor = startcella.Sor;
			int startOszlop=startcella.Oszlop;

			vertex.Clear();
			latogatottVertex = vertex;

			dist.Clear();
			prev.Clear();

			for (int j = 0; j < magassag * szelesseg; j++)
			{
				List<int> sor = new List<int>();
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					sor.Add(0);
				}
				vertex.Add(sor);
			}

			for (int j = 0; j < magassag; j++)
			{
				for (int i = 0; i < szelesseg; i++)
				{
					CTerkepiCella tc = null;
					if (cellak[j][i].tulaj != null)
					{
						tc = Fel(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraFel(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraLe(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = Le(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraLe(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraFel(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							vertex[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							vertex[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
					}
				}
			}

			for (int j = 0; j < magassag; j++)
			{
				List<int> sor = new List<int>();
				for (int i = 0; i < szelesseg; i++)
				{
					sor.Add(50000);
				}
				dist.Add(sor);
			}


			dist[startSor][startOszlop] = 0;

			Q.Clear();
			for (int j = 0; j < magassag; j++)
			{
				for (int i = 0; i < szelesseg; i++)
				{
					if (cellak[j][i].tulaj != null)
					{
						csucs cs;
						cs.Sor = j;
						cs.Oszlop = i;
						Q.Add(cs);
					}
				}
			}

			
			int next = 0;

			for (int k = 0; k < Q.Count; k++)
			{
				if (Q[k].Sor == startSor && Q[k].Oszlop == startOszlop)
				{
					next = k;
					break;
				}
			}

			List<csucs> Qv = new List<csucs>();

			Qv.Add(Q[next]);
			csucs v = new csucs();

			while (Qv.Count > 0)
			{

				csucs u = Qv.Last<csucs>();
				Qv.Remove(Qv.Last<csucs>());

				int ujUt = 0;
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					if (vertex[u.Sor * szelesseg + u.Oszlop][i] == 1)
					{
						v.Sor = i / magassag;
						v.Oszlop = i % szelesseg;
						
						int alt = dist[u.Sor][u.Oszlop] + 1;
						if (alt < dist[v.Sor][v.Oszlop])
						{
							dist[v.Sor][v.Oszlop] = alt;
							prev.Add(u);
							Qv.Add(v);
							ujUt++;
						}
						
					}
				}
			}
			
		}

		public struct csucs
		{
			public int Sor;
			public int Oszlop;
		}

		public CTerkepiCella BalraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Sor - 1 < 0) || (tc.Oszlop - 1 < 0))
					return ret;
				ret = cellak[tc.Sor - 1][tc.Oszlop - 1];
			}
			else
			{
				if (tc.Oszlop - 1 < 0)
					return ret;
				ret = cellak[tc.Sor][tc.Oszlop - 1];
			}
			return ret;
		}

		public CTerkepiCella BalraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Oszlop - 1 < 0)
				return ret;
			if ((tc.Oszlop % 2) == 0)
			{
				ret = cellak[tc.Sor][tc.Oszlop - 1];
			}
			else
			{
				if (tc.Sor + 1 > magassag - 1)
					return ret;
				ret = cellak[tc.Sor + 1][tc.Oszlop - 1];
			}
			return ret;
		}

		public CTerkepiCella JobbraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;

			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Sor - 1 < 0) || (tc.Oszlop + 1 > szelesseg - 1))
					return ret;
				ret = cellak[tc.Sor - 1][tc.Oszlop + 1];
			}
			else
			{
				if (tc.Oszlop + 1 > szelesseg - 1)
					return ret;
				ret = cellak[tc.Sor][tc.Oszlop + 1];
			}
			return ret;
		}
		public CTerkepiCella JobbraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;

			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Oszlop + 1) > (szelesseg - 1))
					return ret;
				ret = cellak[tc.Sor][tc.Oszlop + 1];
			}
			else
			{
				if (((tc.Sor + 1) > (magassag - 1) || (tc.Oszlop + 1) > (szelesseg - 1)))
					return ret;
				ret = cellak[tc.Sor + 1][tc.Oszlop + 1];
			}
			return ret;
		}
		public CTerkepiCella Le(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Sor + 1 > magassag - 1)
				return ret;
			ret = cellak[tc.Sor + 1][tc.Oszlop];
			return ret;
		}
		public CTerkepiCella Fel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Sor - 1 < 0)
				return ret;
			ret = cellak[tc.Sor - 1][tc.Oszlop];
			return ret;
		}

		public CTerkepiCella Balra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Sor][tc.Oszlop - 1];
			return ret;
		}
		public CTerkepiCella Jobbra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = cellak[tc.Sor][tc.Oszlop + 1];
			return ret;
		}

		public bool EgySorbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false;
			if (tc1.Sor == tc2.Sor)
				ret = true;
			return ret;
		}

		public bool EgyOszlopbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false;
			if (tc1.Oszlop == tc2.Oszlop)
				ret = true;
			return ret;
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

			aktualisEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;
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
			CTerkepiCella tc = getTerkepiCellaAtScreenPosition(windowCoord);
			if (terkepAllapot == ETerkepAllapot.harc)
			{
				if (dist[tc.Sor][tc.Oszlop] > ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
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
				
				if (dist[tc.Sor][tc.Oszlop] > ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
				{
					return;
				}
				IMozgoTerkepiEgyseg tempMozgoEgyseg = null;
				tempMozgoEgyseg = (IMozgoTerkepiEgyseg)aktualisEgyseg;
				if (tempMozgoEgyseg != null)
				{
					CTerkepiCella cella = getTerkepiCellaAtScreenPosition(windowCoord);
					if (cella.tulaj != null)
						tempMozgoEgyseg.mozgasIde(cella);
				}

				terkepAllapot = ETerkepAllapot.szabad;
			}
		}
	}


}