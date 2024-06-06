using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI
{
    public static class OSInfo
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static readonly bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static readonly Version Version;
        public static readonly bool IsVersionDefinitelyAccurate;
        static OSInfo()
        {
            if (IsWindows)
            {
                if (TryGetWindowsVersion(out Version))
                {
                    IsVersionDefinitelyAccurate = true;
                }
                else
                {
                    Version = Environment.OSVersion.Version;
                    IsVersionDefinitelyAccurate = false;
                }
            }
            else
            {
                Version = Environment.OSVersion.Version;
                IsVersionDefinitelyAccurate = true;
            }
        }



        static bool TryGetWindowsVersion(out Version osVersion)
        {
            osVersion = default;
            try
            {
                if (!SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx))
                    return false;

                osVersion = new Version((int)osVersionInfoEx.dwMajorVersion, (int)osVersionInfoEx.dwMinorVersion, (int)osVersionInfoEx.dwBuildNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }
        static bool SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx)
        {
            osVersionInfoEx = new WinUnmanagedMethods.RTL_OSVERSIONINFOEX();
            return WinUnmanagedMethods.RtlGetVersion(ref osVersionInfoEx) == 0;
        }
    }
}
