using System;
using System.Diagnostics;

namespace harcocska
{
    internal class TextWriterTraceListener1 : TextWriterTraceListener
    {
        public TextWriterTraceListener1(string s)
            : base(s)
        {

        }
        public override void WriteLine(string x)
        {
            // Use whatever format you want here...
            base.WriteLine(string.Format("{0}: {1}", DateTime.Now, x));
        }
    }
}