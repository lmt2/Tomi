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
using System.Xml.Serialization;

namespace harcocska
{
	[Serializable()]
	public class CTerkep
	{
		public int szelesseg { get; set; }
		public int magassag { get; set; }
		[XmlIgnoreAttribute]
		[NonSerialized()]
		private Canvas canvas1;
		public ETerkepAllapot terkepAllapot { get; set; }
		public CTerkepiEgyseg aktualisEgyseg { get; set; }
		public List<List<CTerkepiCella>> cellak = new List<List<CTerkepiCella>>();

		public List<List<int>> szomdossagiMatrix = new List<List<int>>();
		public List<List<int>> latogatottVertex = new List<List<int>>();
		public List<List<int>> tavolsagTabla = new List<List<int>>();
		public List<csucs> prev = new List<csucs>();
		//List<csucs> Q = new List<csucs>();
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
					c.tulaj = App.jatek.jatekosok[1];
					sor.Add(c);

					//szamlalo++;
					//if (szamlalo>0 && szamlalo < 133) { c.tulaj = App.jatek.jatekosok[0]; }
					//else if (szamlalo >= 133 && szamlalo < 266) { c.tulaj = App.jatek.jatekosok[1]; }
					//else { c.tulaj = App.jatek.jatekosok[2]; }


				}
				cellak.Add(sor);
			}
		}
		
		public Canvas canvas
		{
			get { return canvas1; }
			set
			{
				canvas1 = value;
			}
		}
		public void terkeprajzolas()
		{
			
			canvas.Children.Clear();
			//if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
			//{
			//	Dijkstra(aktualisEgyseg.aktualisCella);
			//}
				

			//if (terkepAllapot == ETerkepAllapot.harc)
			//	Dijkstra(aktualisEgyseg.aktualisCella);

			List<Line> lines = new List<Line>();
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
						p.Fill = cellak[j][i].tulaj.getSzin();
					}
					else
					{
						p.Fill = Brushes.Gray;
					}
					if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
					{
						
						//if (Tavolsag(cellak[i][j], aktualisEgyseg.aktualisCella) <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
						//Console.WriteLine("Tavolsag:{0}", dist[j][i]);
						if (tavolsagTabla[j][i] <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
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

					//Line myLine = new Line();
					//myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
					//myLine.X1 = 1;
					//myLine.X2 = 500;
					//myLine.Y1 = 1;
					//myLine.Y2 = 500;
					//myLine.HorizontalAlignment = HorizontalAlignment.Left;
					//myLine.VerticalAlignment = VerticalAlignment.Center;
					//myLine.StrokeThickness = 10;
					//canvas.Children.Add(myLine);

					if (cellak[j][i].extraSzomszed != null)
					{
						foreach (CTerkepiCella extra in cellak[j][i].extraSzomszed)
						{
							if (extra != null && extra.tulaj != null)
							{
								Line myLine1 = new Line();
								myLine1.Stroke = cellak[j][i].tulaj.getVonalszín();
								Point from=cellak[j][i].getScreenCoord();
								myLine1.X1 = from.X + App.jatek.oldalhossz / 1.5;
								myLine1.Y1 = from.Y + App.jatek.oldalhossz / 2;

								Point to = extra.getScreenCoord();
								myLine1.X2 = to.X + App.jatek.oldalhossz / 1.5;
								myLine1.Y2 = to.Y + App.jatek.oldalhossz / 2;
								myLine1.HorizontalAlignment = HorizontalAlignment.Left;
								myLine1.VerticalAlignment = VerticalAlignment.Center;
								myLine1.StrokeThickness = 1;

								lines.Add(myLine1);
								
							}
						}

					}

					//if (terkepAllapot == ETerkepAllapot.egysegmozgatas)
					//{
					//	if (tavolsagTabla[j][i] < 50000)
					//	{
					//		TextBlock textBlock = new TextBlock();
					//		textBlock.Text = tavolsagTabla[j][i].ToString();
					//		//textBlock.Foreground = new SolidColorBrush(Brushes.Black);
					//		Canvas.SetLeft(textBlock, cellak[j][i].getScreenCoord().X + App.jatek.oldalhossz / 1.5);
					//		Canvas.SetTop(textBlock, cellak[j][i].getScreenCoord().Y + App.jatek.oldalhossz / 2-5);
					//		canvas.Children.Add(textBlock);
					//	}
					//}



					//TextBlock textBlock1 = new TextBlock();
					//textBlock1.Text = String.Format("{0},{1}",j,i);
					////textBlock.Foreground = new SolidColorBrush(Brushes.Black);
					//Canvas.SetLeft(textBlock1, cellak[j][i].getScreenCoord().X + App.jatek.oldalhossz / 1.5);
					//Canvas.SetTop(textBlock1, cellak[j][i].getScreenCoord().Y - App.jatek.oldalhossz+2);
					//canvas.Children.Add(textBlock1);



				}
			}

			foreach (Line l in lines)
			{
				canvas.Children.Add(l);
			}


			CTerkepiImage myImage = null;
			ContextMenu contextMenu = new ContextMenu();

            //contextMenu.Items.Add("Mozgás");

            //contextMenu.Items.Add("Harc");

            //if (App.jatek.aktualisallapot == EJatekAllapotok.egysegmozgatas)
            //{
            //    MenuItem item = new MenuItem();
            //    item.Header = "mozgás";
            //    item.Click += delegate { MyImage_Mozgas(); };
            //    contextMenu.Items.Add(item);
            //}

            if (App.jatek.aktualisallapot == EJatekAllapotok.harc)
            {
                MenuItem item1 = new MenuItem();
                item1.Header = "harc";
                item1.Click += delegate { MyImage_Harc(); };
                contextMenu.Items.Add(item1);
            }

			int szamlalo = 0;
			foreach (CJatekos j in App.jatek.jatekosok)
			{

				szamlalo++;
				foreach (CTerkepiEgyseg te in j.egysegekLista)
				{


                    if (te.aktualisCella == null)
                        break;
					// Create Image Element
					myImage = new CTerkepiImage(te);
					myImage.Width = 20;

					// Create source
					BitmapImage myBitmapImage = new BitmapImage();

					// BitmapImage.UriSource must be in a BeginInit/EndInit block
					myBitmapImage.BeginInit();

					myBitmapImage.UriSource = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, te.bitmap+"_"+szamlalo.ToString()+".png"), UriKind.Absolute);

					
					

					myBitmapImage.DecodePixelWidth = 20;
					myBitmapImage.EndInit();
					//set image source
					myImage.Source = myBitmapImage;
					canvas.Children.Add(myImage);

					myImage.MouseUp += MyImage_MouseUp;
					myImage.MouseRightButtonDown += MyImage_RightMouseDown;
					myImage.MouseLeftButtonDown += MyImage_MouseLeftButtonDown;
					myImage.ContextMenu = contextMenu;

					

					
					Canvas.SetTop(myImage, te.aktualisCella.getScreenCoord().Y - App.jatek.oldalhossz / 2);
					Canvas.SetLeft(myImage, te.aktualisCella.getScreenCoord().X + App.jatek.oldalhossz / 2);

					TextBlock textBlockElet = new TextBlock();
					textBlockElet.Text = String.Format("{0}", te.elet.ToString());
					//textBlock.Foreground = new SolidColorBrush(Brushes.Black);
					Canvas.SetTop(textBlockElet, te.aktualisCella.getScreenCoord().Y - App.jatek.oldalhossz / 2-12);
					Canvas.SetLeft(textBlockElet, te.aktualisCella.getScreenCoord().X + App.jatek.oldalhossz / 2+12);
					canvas.Children.Add(textBlockElet);

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
			canvas.InvalidateVisual();
		}

		private void MyImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			aktualisEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;

			Dijkstra(aktualisEgyseg.aktualisCella);
			terkepAllapot = ETerkepAllapot.egysegmozgatas;
			terkeprajzolas();

			Image ti = e.Source as CTerkepiImage;
			DataObject data = new DataObject(typeof(CTerkepiImage), (CTerkepiImage)sender);
			DragDrop.DoDragDrop(ti, data, DragDropEffects.All);

			
			//terkepAllapot = ETerkepAllapot.egysegmozgatas;
			//throw new NotImplementedException();
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


		public void Dijkstra(CTerkepiCella startCella)
		{

			szomdossagiMatrix.Clear();
			tavolsagTabla.Clear();
			prev.Clear();

			for (int j = 0; j < magassag * szelesseg; j++)
			{
				List<int> sor = new List<int>();
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					sor.Add(0);
				}
				szomdossagiMatrix.Add(sor);
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
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraFel(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraLe(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = Le(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraLe(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraFel(cellak[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						
						if (cellak[j][i].extraSzomszed != null){ 
							foreach (CTerkepiCella extra in cellak[j][i].extraSzomszed)
							{
								if (extra != null && extra.tulaj != null)
								{
									szomdossagiMatrix[j * szelesseg + i][extra.Sor * szelesseg + extra.Oszlop] = 3;
									szomdossagiMatrix[extra.Sor * szelesseg + extra.Oszlop][j * szelesseg + i] = 3;
								}
							}
						
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
				tavolsagTabla.Add(sor);
			}


			tavolsagTabla[startCella.Sor][startCella.Oszlop] = 0;



			List<csucs> Qv = new List<csucs>();

			csucs kezdopont;
			kezdopont.Sor = startCella.Sor;
			kezdopont.Oszlop = startCella.Oszlop;
			Qv.Add(kezdopont);
			csucs v = new csucs();

			while (Qv.Count > 0)
			{

				csucs u = Qv.Last<csucs>();
				Qv.Remove(Qv.Last<csucs>());

				int ujUt = 0;
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					if (szomdossagiMatrix[u.Sor * szelesseg + u.Oszlop][i] !=0 )
					{
						v.Sor = i / magassag;
						v.Oszlop = i % szelesseg;
						
						int alt = tavolsagTabla[u.Sor][u.Oszlop] + szomdossagiMatrix[u.Sor * szelesseg + u.Oszlop][i];
						if (alt < tavolsagTabla[v.Sor][v.Oszlop])
						{
							tavolsagTabla[v.Sor][v.Oszlop] = alt;
							prev.Add(u);
							Qv.Add(v);
							ujUt++;
						}
						
					}
				}
			}
			
		}
		[Serializable()]
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
            if (App.jatek.aktualisallapot== EJatekAllapotok.egysegmozgatas)
			terkepAllapot = ETerkepAllapot.egysegmozgatas;
		}

		private void MyImage_Harc()
		{
            if (App.jatek.aktualisallapot == EJatekAllapotok.harc)
                terkepAllapot = ETerkepAllapot.harc;
		}

		private void MyImage_RightMouseDown(object sender, MouseButtonEventArgs e)
		{

			//aktualisEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;
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
				if (tavolsagTabla[tc.Sor][tc.Oszlop] > ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
				{
					return;
				}
				IHarcoloTerkepiEgyseg tempHarcoloEgyseg = null;
				tempHarcoloEgyseg = (IHarcoloTerkepiEgyseg)aktualisEgyseg;
				if (tempHarcoloEgyseg != null)
					tempHarcoloEgyseg.Tamadas(getTerkepiEgysegAtScreenPosition(windowCoord));
				App.jatek.terkep.terkeprajzolas();
				terkepAllapot = ETerkepAllapot.szabad;
			}
			
			//}
		}
	}


}