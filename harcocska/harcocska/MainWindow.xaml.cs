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
		#endregion
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		#region constructors
		//főablak konstruktor
		public MainWindow()
        {
            InitializeComponent();

			App.jatek.init();
			RajzoloTimer.Tick += RajzoloTimer_Tick;
			RajzoloTimer.Interval = new TimeSpan(0, 0, 2);
			RajzoloTimer.Start();

			UserControl1 u1 = new UserControl1(App.jatek.jatekosok[0]);
			UserControl1 u2 = new UserControl1(App.jatek.jatekosok[1]);
			UserControl1 u3 = new UserControl1(App.jatek.jatekosok[2]);
			stackpanel.Children.Add(u1);
			stackpanel.Children.Add(u2);
			stackpanel.Children.Add(u3);

			
			App.jatek.run();

        }
		#endregion
		private void RajzoloTimer_Tick(object sender, EventArgs e)
		{
			terkeprajzolas();
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
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm


		public void terkeprajzolas()
		{
			for (int j = 0; j < App.jatek.terkep.magassag; j++)
			{
				for (int i = 0; i < App.jatek.terkep.szelesseg; i++)
				{
					PointCollection curvePoints = App.jatek.terkep.cellak[i][j].getScreenCoords();

					Polygon p = new Polygon();
					p.Stroke = Brushes.Black;
					p.Fill = Brushes.LightBlue;
					p.StrokeThickness = 1;
					p.HorizontalAlignment = HorizontalAlignment.Left;
					p.VerticalAlignment = VerticalAlignment.Center;
					p.Points = curvePoints;
					canvas1.Children.Add(p);

				}
			}

			Image myImage = null; ;
			ContextMenu contextMenu = new ContextMenu();

			contextMenu.Items.Add("One");

			contextMenu.Items.Add("Two");

			MenuItem item = new MenuItem();

			item.Header = "Three";

			item.Click += delegate { MessageBox.Show("Test"); };


			foreach (CTerkepiEgyseg te in App.jatek.jatekosok[0].egysegekLista)
			{

				// Create Image Element
				myImage = new Image();
				myImage.Width = 20;

				// Create source
				BitmapImage myBitmapImage = new BitmapImage();

				// BitmapImage.UriSource must be in a BeginInit/EndInit block
				myBitmapImage.BeginInit();

				myBitmapImage.UriSource = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tank.png"), UriKind.Absolute);


				myBitmapImage.DecodePixelWidth = 20;
				myBitmapImage.EndInit();
				//set image source
				myImage.Source = myBitmapImage;
				canvas1.Children.Add(myImage);

				myImage.MouseUp += MyImage_MouseUp;


				Canvas.SetTop(myImage, te.aktualisCella.getScreenCoord().Y - App.jatek.oldalhossz / 2);
				Canvas.SetLeft(myImage, te.aktualisCella.getScreenCoord().X + App.jatek.oldalhossz / 2);

				te.jobbra();

			}
			

			

			contextMenu.Items.Add(item);

			myImage.ContextMenu = contextMenu;
		}

		private void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			
		}
	}
}
