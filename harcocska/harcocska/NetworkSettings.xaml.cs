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
using System.Windows.Shapes;

namespace harcocska
{
    /// <summary>
    /// Interaction logic for NetworkSettings.xaml
    /// </summary>
    public partial class NetworkSettings : Window
    {
        public NetworkSettings()
        {
            InitializeComponent();
            remHostEB.Text = Properties.Settings.Default.remoteHost;
            remPortEB.Text = Properties.Settings.Default.remotePort.ToString();
            isServerCB.IsChecked = Properties.Settings.Default.isServer;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.remoteHost = remHostEB.Text;
            Properties.Settings.Default.remotePort = int.Parse(remPortEB.Text);
            Properties.Settings.Default.isServer = (bool)isServerCB.IsChecked;

            harcocska.Properties.Settings.Default.Save();
        }
    }
}
