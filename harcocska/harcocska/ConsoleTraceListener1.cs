using System;
using System.Diagnostics;

namespace harcocska
{
    internal class ConsoleTraceListener1 : ConsoleTraceListener
    {
        public ConsoleTraceListener1(bool b)
            : base(b)
        {

        }
        public override void WriteLine(string x)
        {
            // Use whatever format you want here...
            base.WriteLine(string.Format("{0}: {1}", DateTime.Now, x));
        }
    }
}