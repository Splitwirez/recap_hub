using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace ReCap.Hub
{
    [SupportedOSPlatform("linux")]
    public static class LinuxPInvoke
    {
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
        [DllImport("System.Native", EntryPoint = "SystemNative_ReadLink", SetLastError = true)]
        public static extern unsafe int ReadLink(string path, byte[] buffer, int bufferSize);
    }
}
