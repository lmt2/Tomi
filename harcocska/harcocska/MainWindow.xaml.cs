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
using Microsoft.Win32;

namespace harcocska
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
	/// a főablak
    /// </summary>
    public partial class MainWindow : Window
    {
		#region members
		DispatcherTimer RajzoloTimer = new DispatcherTimer();
        CTerkepiCella from;
		#endregion
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		#region constructors
		//főablak konstruktor
		public MainWindow()
        {
            InitializeComponent();
		}
		#endregion

		private void init()
		{
			App.jatek.init();
			App.jatek.terkep.canvas = canvas1;
			App.jatek.terkep.canvas.AllowDrop = true;
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
			var window = Window.GetWindow(canvas1);
			window.KeyDown += HandleKeyPressOnCanvas;

			App.jatek.terkep.terkeprajzolas();
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
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click_1(object sender, RoutedEventArgs e)
        {
			App.jatek.lepes();


			//Line myLine = new Line();
			//myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
			//myLine.X1 = 1;
			//myLine.X2 = 500;
			//myLine.Y1 = 1;
			//myLine.Y2 = 500;
			//myLine.HorizontalAlignment = HorizontalAlignment.Left;
			//myLine.VerticalAlignment = VerticalAlignment.Center;
			//myLine.StrokeThickness = 10;
			//canvas1.Children.Add(myLine);

			//double angle = 0.0;

			//Transform transform = b1.RenderTransform;

			//if (transform is TransformGroup)
			//{
			//    TransformGroup tg = (TransformGroup)b1.RenderTransform;
			//    foreach (Transform t in tg.Children)
			//        if (t is RotateTransform)
			//        {
			//            RotateTransform r = (RotateTransform)t;
			//            angle += r.Angle;
			//        }
			//}

			//if (transform is RotateTransform)
			//{
			//    angle = ((RotateTransform)transform).Angle;
			//}

			//b1.RenderTransform = new RotateTransform(angle+10);
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		/// <summary>
		/// Kilépés menü
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		/// <summary>
		/// játék start menü
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			App.jatek.start();
		}


		private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Point pointToWindow = Mouse.GetPosition(canvas1);
			//Point pointToScreen = PointToScreen(pointToWindow);

			//Point controlRelatedCoords = canvas1.mo PointToClient(pointToWindow);
			//controlRelatedCoords.Offset(panel1.HorizontalScroll.Value, panel1.VerticalScroll.Value);
			App.jatek.terkep.OnLeftMouseDown(pointToWindow);

            from = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
            //Console.WriteLine("Kezdopont:{0},{1}",from.Sor,from.Oszlop);
        }

		private void canvas1_MouseUp_1(object sender, MouseButtonEventArgs e)
		{
			
		}

        private void canvas1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(canvas1);
			//App.jatek.terkep.Tavolsag(from, App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow));
			//App.jatek.terkep.Dijkstra(0,0);
			//Console.WriteLine("Tavolsag:{0}", App.jatek.terkep.dist[App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow).Sor][App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow).Oszlop]);
		}

		private void MenuItem_Terkeprajzolas_Click(object sender, RoutedEventArgs e)
		{
			if (App.jatek.terkep.terkepAllapot == ETerkepAllapot.szabad) { 
				App.jatek.terkep.terkepAllapot = ETerkepAllapot.szerkesztes;
				return;
			}
			if (App.jatek.terkep.terkepAllapot == ETerkepAllapot.szerkesztes)
			{
				App.jatek.terkep.terkepAllapot = ETerkepAllapot.szabad;
				return;
			}
		}


		private void HandleKeyPressOnCanvas(object sender, KeyEventArgs e)
		{
			Point pointToWindow = Mouse.GetPosition(canvas1);
			if (App.jatek.terkep.terkepAllapot== ETerkepAllapot.szerkesztes)
			{
				CTerkepiCella cella = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
				if (cella != null) { 
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
						from = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
					}
					if (e.IsDown && e.Key == Key.LeftShift)
					{
						if (App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow).extraSzomszed == null)
							App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow).extraSzomszed = new List<CTerkepiCella>();
                        CTerkepiCella c = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
                        if (c.tulaj==from.tulaj) 
						    c.extraSzomszed.Add(from);
					}
                    if (e.IsDown && e.Key == Key.T)
                    {
                        CMozgoTerkepiEgyseg e3 =new CTank();
                        e3.aktualisCella = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
                        e3.jatekos = App.jatek.jatekosok[0];
                        App.jatek.jatekosok[0].egysegekLista.Add(e3);
                    }
                    if (e.IsDown && e.Key == Key.K)
                    {
                        CMozgoTerkepiEgyseg e3 =new CKatona();
                        e3.aktualisCella = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
                        e3.jatekos = App.jatek.jatekosok[0];
                        App.jatek.jatekosok[0].egysegekLista.Add(e3);
                    }
                }
				App.jatek.terkep.terkeprajzolas();
			}
			

		}

		//save to file
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				Properties.Settings.Default.fileName = saveFileDialog.FileName;
				Properties.Settings.Default.Save();
				CGame.WriteToBinaryFile<CGame>(saveFileDialog.FileName, App.jatek);
			}
		}

		//open from file
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				Properties.Settings.Default.fileName = openFileDialog.FileName;
				this.Title = "Harcocska [" + Properties.Settings.Default.fileName + "]";
				Properties.Settings.Default.Save();
				App.jatek = CGame.ReadFromBinaryFile<CGame>(openFileDialog.FileName);
				App.jatek.terkep.canvas = canvas1;
				App.jatek.init1();
			}
		}

		private void canvas1_DragOver(object sender, DragEventArgs e)
		{

			
			
		}

		private void canvas1_Drop(object sender, DragEventArgs e)
		{
			App.jatek.terkep.terkepAllapot = ETerkepAllapot.szabad;
			Point pointToWindow = e.GetPosition(canvas1);
			CTerkepiImage img = (CTerkepiImage)e.Data.GetData(typeof(CTerkepiImage));
			CTerkepiCella cella = App.jatek.terkep.getTerkepiCellaAtScreenPosition(pointToWindow);
			IMozgoTerkepiEgyseg tempMozgoEgyseg = null;
			tempMozgoEgyseg = (IMozgoTerkepiEgyseg)img.terkepiEgyseg;
			if (tempMozgoEgyseg != null)
			{

				if (cella.tulaj != null)
					tempMozgoEgyseg.mozgasCellara(cella);
			}
			App.jatek.terkep.terkeprajzolas();
		}

		//zoom
		private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta==-120)
				App.jatek.oldalhossz-=5;
			if (e.Delta == 120)
				App.jatek.oldalhossz += 5;
			App.jatek.terkep.terkeprajzolas();
		}

		//
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				App.jatek = CGame.ReadFromBinaryFile<CGame>(Properties.Settings.Default.fileName);
				
			}
			catch {
				init();
			}
			this.Title = "Harcocska [" + Properties.Settings.Default.fileName+"]";
			App.jatek.terkep.canvas = canvas1;
			UserControl1 u1 = new UserControl1(App.jatek.jatekosok[0]);
			UserControl1 u2 = new UserControl1(App.jatek.jatekosok[1]);
			UserControl1 u3 = new UserControl1(App.jatek.jatekosok[2]);
			stackpanel.Children.Add(u1);
			stackpanel.Children.Add(u2);
			stackpanel.Children.Add(u3);
			
			var window = Window.GetWindow(canvas1);
			window.KeyDown += HandleKeyPressOnCanvas;

			RajzoloTimer.Tick += RajzoloTimer_Tick;
			RajzoloTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
			RajzoloTimer.Start();
			App.jatek.init1();
			App.jatek.run();

		}

		//új, üres térkép
		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			App.jatek.jatekosok.Clear();
			init();
			App.jatek.terkep.canvas = canvas1;
			App.jatek.init1();
		}

		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			App.jatek.mindenkiFeltamasztasa();
			App.jatek.terkep.terkeprajzolas();
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm





	}
}
