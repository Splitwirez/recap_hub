using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace ReCap.Hub
{
    [SupportedOSPlatform("linux")]
    public static class LinuxPInvoke
    {
        const string SYS_NATIVE = "System.Native";

        //https://stackoverflow.com/questions/58326739/how-can-i-find-the-target-of-a-linux-symlink-in-c-sharp
        //https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/shared/Interop/Unix/System.Native/Interop.ReadLink.cs
        /// <summary>
        /// Takes a path to a symbolic link and attempts to place the link target path into the buffer. If the buffer is too
        /// small, the path will be truncated. No matter what, the buffer will not be null terminated.
        /// </summary>
        /// <param name="path">The path to the symlink</param>
        /// <param name="buffer">The buffer to hold the output path</param>
        /// <param name="bufferSize">The size of the buffer</param>
        /// <returns>
        /// Returns the number of bytes placed into the buffer on success; bufferSize if the buffer is too small; and -1 on error.
        /// </returns>
        [DllImport(SYS_NATIVE, EntryPoint = "SystemNative_ReadLink", SetLastError = true)]
        public static extern unsafe int ReadLink(string path, byte[] buffer, int bufferSize);



        //https://github.com/dotnet/corert/blob/master/src/Common/src/Interop/Unix/System.Native/Interop.POpen.cs
        [DllImport(SYS_NATIVE, EntryPoint = "SystemNative_POpen", SetLastError = true)]
        public static extern IntPtr POpen(string command, string type);

        [DllImport(SYS_NATIVE, EntryPoint = "SystemNative_PClose", SetLastError = true)]
        public static extern int PClose(IntPtr stream);


        //https://github.com/mono/mono/blob/main/mcs/class/Mono.Posix/Mono.Unix.Native/Syscall.cs
        //[DllImport (MPH, EntryPoint="Mono_Posix_Syscall_WEXITSTATUS")]
        [DllImport (SYS_NATIVE, EntryPoint="SystemNative_WEXITSTATUS")]
		public static extern int WEXITSTATUS(int status);






        /*[DllImport(SYS_NATIVE, EntryPoint = "SystemNative_FGets", SetLastError = true)]
        public static extern int FGets(out string str, int n, IntPtr stream);*/
    }
}
