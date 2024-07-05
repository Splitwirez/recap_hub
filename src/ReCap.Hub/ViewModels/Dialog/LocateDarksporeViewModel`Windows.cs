using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ReCap.Hub.ViewModels
{
    public partial class LocateDarksporeViewModel
        : ViewModelBase
        , IDialogViewModel<DarksporeInstallPaths>
    {
        [SupportedOSPlatform("windows")]
        DarksporeInstallPaths Windows_GetDarksporeInstallPathFromProcess(Process process)
            => new DarksporeInstallPaths(process.GetExecutablePath());
    }
}
