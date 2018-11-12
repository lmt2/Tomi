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
using System.Windows.Threading;
using Microsoft.Win32;
using harcocska;

namespace Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer RajzoloTimer = new DispatcherTimer();
		CTerkepiCella from;
		public CTerkepiEgyseg aktualisEgyseg { get; set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		private void init()
		{
			App.jatek.init();
			canvas.AllowDrop = true;
			RajzoloTimer.Tick += RajzoloTimer_Tick;
			RajzoloTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
			RajzoloTimer.Start();

			UserControl1 u1 = new UserControl1(App.jatek.jatekosok[0]);
			UserControl1 u2 = new UserControl1(App.jatek.jatekosok[1]);
			UserControl1 u3 = new UserControl1(App.jatek.jatekosok[2]);
			stackpanel.Children.Add(u1);
			stackpanel.Children.Add(u2);
			stackpanel.Children.Add(u3);


			App.jatek.run();
			var window = Window.GetWindow(canvas);
			window.KeyDown += HandleKeyPressOnCanvas;

			terkeprajzolas();
		}
		private void RajzoloTimer_Tick(object sender, EventArgs e)
		{
			switch (App.jatek.aktualisallapot)
			{
				case EJatekAllapotok.elokeszulet:
					b1.Content = "elokeszulet (next:penzosztas)";
					break;

				case EJatekAllapotok.penzosztas:
					b1.Content = "penzosztas (next:fejlesztes)";
					break;

				case EJatekAllapotok.fejlesztes:
					b1.Content = "fejlesztes (next:egysegmozgatas)";
					break;

				case EJatekAllapotok.egysegmozgatas:
					b1.Content = "egysegmozgatas (next:harc)";
					break;

				case EJatekAllapotok.harc:
					b1.Content = "harc (next:penzosztas)";
					break;

			}
			//App.jatek.terkep.terkeprajzolas();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				Properties.Settings.Default.fileName = openFileDialog.FileName;
				this.Title = "Harcocska [" + Properties.Settings.Default.fileName + "]";
				Properties.Settings.Default.Save();
				App.jatek = CGame.ReadFromBinaryFile<CGame>(openFileDialog.FileName);
				App.jatek.init1();
				terkeprajzolas();
			}
		}

		private void b1_Click(object sender, RoutedEventArgs e)
		{
			App.jatek.lepes();
		}

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			App.jatek.start();
		}

		private void HandleKeyPressOnCanvas(object sender, KeyEventArgs e)
		{
			Point pointToWindow = Mouse.GetPosition(canvas);
			if (App.jatek.terkep.terkepAllapot == ETerkepAllapot.szerkesztes)
			{
				CTerkepiCella cella = getTerkepiCellaAtScreenPosition(pointToWindow);
				if (cella != null)
				{
					if (e.IsDown && e.Key == Key.D1)
					{
						cella.tulaj = App.jatek.jatekosok[0];
					}
					if (e.IsDown && e.Key == Key.D2)
					{
						cella.tulaj = App.jatek.jatekosok[1];
					}
					if (e.IsDown && e.Key == Key.D3)
					{
						cella.tulaj = App.jatek.jatekosok[2];
					}
					if (e.IsDown && e.Key == Key.D0)
					{
						cella.tulaj = null;
						cella.extraSzomszed = null;
					}
					if (e.IsDown && e.Key == Key.LeftCtrl)
					{
						from = getTerkepiCellaAtScreenPosition(pointToWindow);
					}
					if (e.IsDown && e.Key == Key.LeftShift)
					{
						if (getTerkepiCellaAtScreenPosition(pointToWindow).extraSzomszed == null)
							getTerkepiCellaAtScreenPosition(pointToWindow).extraSzomszed = new List<CTerkepiCella>();
						CTerkepiCella c = getTerkepiCellaAtScreenPosition(pointToWindow);
						if (c.tulaj == from.tulaj)
							c.extraSzomszed.Add(from);
					}
					if (e.IsDown && e.Key == Key.T)
					{
						CMozgoTerkepiEgyseg e3 = new CTank();
						e3.aktualisCella = getTerkepiCellaAtScreenPosition(pointToWindow);
						e3.jatekos = App.jatek.jatekosok[0];
						App.jatek.jatekosok[0].egysegekLista.Add(e3);
					}
					if (e.IsDown && e.Key == Key.K)
					{
						CMozgoTerkepiEgyseg e3 = new CKatona();
						e3.aktualisCella = getTerkepiCellaAtScreenPosition(pointToWindow);
						e3.jatekos = App.jatek.jatekosok[0];
						App.jatek.jatekosok[0].egysegekLista.Add(e3);
					}
				}
				terkeprajzolas();
			}


		}
		public CTerkepiCella getTerkepiCellaAtScreenPosition(Point p)
		{
			CTerkepiCella ret = null;
			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = getScreenCoords(App.jatek.terkep.cellak[i][j]);
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
		public static int offsetX()
		{
			return (int)(Math.Cos(Math.PI * 60 / 180.0) * App.jatek.oldalhossz);
		}
		public static int offsetY()
		{
			return (int)(Math.Sin(Math.PI * 60 / 180.0) * App.jatek.oldalhossz);
		}

		public Point getScreenCoord(CTerkepiCella tc)
		{
			Point p1 = new Point();
			if ((tc.Oszlop % 2) == 0)
			{
				p1.X = (int)(tc.Oszlop * offsetX() + tc.Oszlop * App.jatek.oldalhossz);
				p1.Y = (int)(tc.Sor * 2 * offsetY());
			}
			else
			{
				p1.X = (int)(tc.Oszlop * offsetX() + tc.Oszlop * App.jatek.oldalhossz);
				p1.Y = (int)(tc.Sor * 2 * offsetY() + offsetY());
			}

			return p1;
		}

		public PointCollection getScreenCoords(CTerkepiCella tc)
		{
			Point p1 = getScreenCoord(tc);

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

			PointCollection curvePoints = new PointCollection();
			curvePoints.Add(p1);
			curvePoints.Add(p2);
			curvePoints.Add(p3);
			curvePoints.Add(p4);
			curvePoints.Add(p5);
			curvePoints.Add(p6);

			return curvePoints;
		}

		private void MyImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			aktualisEgyseg = ((CTerkepiImage)sender).terkepiEgyseg;

			App.jatek.terkep.Dijkstra(aktualisEgyseg.aktualisCella);
			App.jatek.terkep.terkepAllapot = ETerkepAllapot.egysegmozgatas;
			terkeprajzolas();
			if (!((CMozgoTerkepiEgyseg)aktualisEgyseg).lepettMar)
			{
				Image ti = e.Source as CTerkepiImage;
				DataObject data = new DataObject(typeof(CTerkepiImage), (CTerkepiImage)sender);
				DragDrop.DoDragDrop(ti, data, DragDropEffects.All);
			}


			//terkepAllapot = ETerkepAllapot.egysegmozgatas;
			//throw new NotImplementedException();
		}

		public System.Windows.Media.SolidColorBrush getSzin(CJatekos j)
		{
			switch (j.szin)
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

		public System.Windows.Media.SolidColorBrush getVonalszín(CJatekos j)
		{
			switch (j.szin)
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
		private void MyImage_Harc()
		{
			if (App.jatek.aktualisallapot == EJatekAllapotok.harc)
				App.jatek.terkep.terkepAllapot = ETerkepAllapot.harc;
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
			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = getScreenCoords(App.jatek.terkep.cellak[j][i]);


					Polygon p = new Polygon();
					RenderOptions.SetEdgeMode((DependencyObject)p, EdgeMode.Aliased);
					p.Stroke = Brushes.Black;

					if (App.jatek.terkep.cellak[j][i].tulaj != null)
					{
						p.Fill = getSzin(App.jatek.terkep.cellak[j][i].tulaj);
					}
					else
					{
						p.Fill = Brushes.Gray;
					}
					if (App.jatek.terkep.terkepAllapot == ETerkepAllapot.egysegmozgatas)
					{

						//if (Tavolsag(cellak[i][j], aktualisEgyseg.aktualisCella) <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
						//Console.WriteLine("Tavolsag:{0}", dist[j][i]);
						if (App.jatek.terkep.tavolsagTabla[j][i] <= ((CMozgoTerkepiEgyseg)aktualisEgyseg).range)
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

					if (App.jatek.terkep.cellak[j][i].extraSzomszed != null)
					{
						foreach (CTerkepiCella extra in App.jatek.terkep.cellak[j][i].extraSzomszed)
						{
							if (extra != null && extra.tulaj != null)
							{
								Line myLine1 = new Line();
								myLine1.Stroke = getVonalszín(App.jatek.terkep.cellak[j][i].tulaj);
								Point from = getScreenCoord(App.jatek.terkep.cellak[j][i]);
								myLine1.X1 = from.X + App.jatek.oldalhossz / 1.5;
								myLine1.Y1 = from.Y + App.jatek.oldalhossz / 2;

								Point to = getScreenCoord(extra);
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

					myBitmapImage.UriSource = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, te.bitmap + "_" + szamlalo.ToString() + ".png"), UriKind.Absolute);




					myBitmapImage.DecodePixelWidth = 20;
					myBitmapImage.EndInit();
					//set image source
					myImage.Source = myBitmapImage;
					canvas.Children.Add(myImage);

					//myImage.MouseUp += MyImage_MouseUp;
					//myImage.MouseRightButtonDown += MyImage_RightMouseDown;
					myImage.MouseLeftButtonDown += MyImage_MouseLeftButtonDown;
					myImage.ContextMenu = contextMenu;




					Canvas.SetTop(myImage, getScreenCoord(te.aktualisCella).Y - App.jatek.oldalhossz / 2);
					Canvas.SetLeft(myImage, getScreenCoord(te.aktualisCella).X + App.jatek.oldalhossz / 2);

					TextBlock textBlockElet = new TextBlock();
					textBlockElet.Text = String.Format("{0}", te.elet.ToString());
					//textBlock.Foreground = new SolidColorBrush(Brushes.Black);
					Canvas.SetTop(textBlockElet, getScreenCoord(te.aktualisCella).Y - App.jatek.oldalhossz / 2 - 12);
					Canvas.SetLeft(textBlockElet, getScreenCoord(te.aktualisCella).X + App.jatek.oldalhossz / 2 + 12);
					canvas.Children.Add(textBlockElet);

					if (App.jatek.aktualisallapot == EJatekAllapotok.egysegmozgatas && ((CMozgoTerkepiEgyseg)te != null) && !((CMozgoTerkepiEgyseg)te).lepettMar)
					{
						Line myLine1 = new Line();
						myLine1.Stroke = getVonalszín(j);
						Point from = getScreenCoord(te.aktualisCella);
						from.Offset(App.jatek.oldalhossz / 1.5, App.jatek.oldalhossz / 3);
						myLine1.X1 = from.X;
						myLine1.Y1 = from.Y;

						Point to = from;
						to.Offset(App.jatek.oldalhossz / 2, 0);
						myLine1.X2 = to.X;
						myLine1.Y2 = to.Y;
						//myLine1.HorizontalAlignment = HorizontalAlignment.Left;
						//myLine1.VerticalAlignment = VerticalAlignment.Center;
						myLine1.StrokeThickness = 1;

						canvas.Children.Add(myLine1);
					}

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
						Canvas.SetTop(myImage1, getScreenCoord(te.aktualisCella).Y - App.jatek.oldalhossz / 2);
						Canvas.SetLeft(myImage1, getScreenCoord(te.aktualisCella).X + App.jatek.oldalhossz / 2);
					}
					//((IMozgoTerkepiEgyseg)te).MozgasJobbra();

				}
			}
			canvas.InvalidateVisual();
		}

		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			App.jatek.jatekosok.Clear();
			init();
			App.jatek.init1();
		}

		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				Properties.Settings.Default.fileName = saveFileDialog.FileName;
				Properties.Settings.Default.Save();
				CGame.WriteToBinaryFile<CGame>(saveFileDialog.FileName, App.jatek);
			}
		}

		private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta == -120)
				App.jatek.oldalhossz -= 5;
			if (e.Delta == 120)
				App.jatek.oldalhossz += 5;
			terkeprajzolas();
		}

		private void canvas_Drop(object sender, DragEventArgs e)
		{
			App.jatek.terkep.terkepAllapot = ETerkepAllapot.szabad;
			Point pointToWindow = e.GetPosition(canvas);
			CTerkepiImage img = (CTerkepiImage)e.Data.GetData(typeof(CTerkepiImage));
			CTerkepiCella cella = getTerkepiCellaAtScreenPosition(pointToWindow);
			IMozgoTerkepiEgyseg tempMozgoEgyseg = null;
			tempMozgoEgyseg = (IMozgoTerkepiEgyseg)img.terkepiEgyseg;
			if (tempMozgoEgyseg != null)
			{

				if (cella.tulaj != null)
					tempMozgoEgyseg.mozgasCellara(App.jatek.terkep,cella);
			}
			terkeprajzolas();
		}
	}
}
