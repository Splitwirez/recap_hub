using System.Runtime.InteropServices;

namespace ReCap.Hub
{
    public static class RunningOn
    {
        public static readonly bool Windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool Linux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static readonly bool MacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}
