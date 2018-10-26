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
			App.jatek.terkep.canvas = canvas1;
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
			App.jatek.terkep.terkeprajzolas();
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

		private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Point pointToWindow = Mouse.GetPosition(canvas1);
			//Point pointToScreen = PointToScreen(pointToWindow);

			//Point controlRelatedCoords = canvas1.mo PointToClient(pointToWindow);
			//controlRelatedCoords.Offset(panel1.HorizontalScroll.Value, panel1.VerticalScroll.Value);
			App.jatek.terkep.mozgasIde(pointToWindow);
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm





	}
}
