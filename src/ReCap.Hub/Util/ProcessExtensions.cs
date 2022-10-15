using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ReCap.Hub
{
    public static class ProcessExtensions
    {
        public static string GetExecutablePath(this Process process)
        {
            if (!OperatingSystem.IsWindows())
            {
                var mainModule = process.MainModule;
                if (mainModule == null)
                    return null;
                else
                    return mainModule.FileName;
            }

            string ret = null;
            var pid = process.Id;
            ret = GetExecutablePath(pid);
            return ret;
        }



#pragma warning disable CA1416
        static bool GetCanOpenProcess(int processId, WindowsPInvoke.ProcessAccess desiredAccess)
        {
            var ret = TryOpenProcess(processId, desiredAccess, out IntPtr hProcess);
            WindowsPInvoke.CloseHandle(hProcess);
            return ret;
        }
        static bool TryOpenProcess(int processId, WindowsPInvoke.ProcessAccess desiredAccess, out IntPtr hProcess)
        {
            hProcess = WindowsPInvoke.OpenProcess(desiredAccess, false, processId);
            return hProcess == IntPtr.Zero;
        }

        static bool GetCanOpenProcessLimited(int processId)
            => GetCanOpenProcess(processId, WindowsPInvoke.ProcessAccess.QueryLimitedInformation);
        static bool TryOpenProcessLimited(int processId, out IntPtr hProcess)
            => TryOpenProcess(processId, WindowsPInvoke.ProcessAccess.QueryLimitedInformation, out hProcess);
#pragma warning restore CA1416

        public static string GetExecutablePath(int ProcessId)
        {
            if (!OperatingSystem.IsWindows())
                return Process.GetProcessById(ProcessId).MainModule.FileName;

            string ret = null;
            var buffer = new StringBuilder(1024);
            IntPtr hprocess = WindowsPInvoke.OpenProcess(WindowsPInvoke.ProcessAccess.QueryLimitedInformation,
                                          false, ProcessId);
            if (hprocess != IntPtr.Zero)
            {
                try
                {
                    int size = buffer.Capacity;
                    bool success = WindowsPInvoke.QueryFullProcessImageName(hprocess, 0, buffer, out size);
                    if (success)
                        ret = buffer.ToString();
                }
                catch
                {
                }
                WindowsPInvoke.CloseHandle(hprocess);
            }
            return ret;
        }
    }
}