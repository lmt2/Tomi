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

namespace harcocska
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Line myLine = new Line();
            //myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            //myLine.X1 = 1;
            //myLine.X2 = 50;
            //myLine.Y1 = 1;
            //myLine.Y2 = 50;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            //myLine.StrokeThickness = 2;
            //aaa.Children.Add(myLine);

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

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
