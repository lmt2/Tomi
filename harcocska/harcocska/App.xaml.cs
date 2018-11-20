using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace harcocska
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		public static CGame jatek=new CGame();

        public App()
        {
            DateTime programStartTime = DateTime.Now;
            string startingFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Trace.Listeners.Clear();
            string logfilename = Environment.GetEnvironmentVariable("computername") + "_" + Environment.GetEnvironmentVariable("username") + "_" + programStartTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".log.txt";
            if (!Directory.Exists(startingFolder + "\\_log"))
            {
                Directory.CreateDirectory(startingFolder + "\\_log");
            }


            TextWriterTraceListener1 twtl = new TextWriterTraceListener1(startingFolder + "//_log//" + logfilename);
            twtl.Name = "TextLogger";
            twtl.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;

            ConsoleTraceListener1 ctl = new ConsoleTraceListener1(false);
            //ctl.TraceOutputOptions = TraceOptions.DateTime;

            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);
            Trace.AutoFlush = true;
        }
    }
}
