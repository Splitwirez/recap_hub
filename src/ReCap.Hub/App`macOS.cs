using Avalonia;
using System;

namespace ReCap.Hub
{
    public partial class App
        : Application
    {
        static bool GetShouldUseManagedWindowDecorationsByDefault_macOS()
        {
            return false;
        }
    }
}