using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ResurrectionCapsule.Launcher.Context
{
    public class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.Default;

        public override void Write(string value)
        {
            base.Write(value);
            WrittenTo?.Invoke(this, value);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            WrittenTo?.Invoke(this, value);
        }

        public event EventHandler<string> WrittenTo;
    }
}
