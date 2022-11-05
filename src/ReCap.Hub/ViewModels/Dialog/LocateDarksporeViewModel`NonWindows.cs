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
    public partial class LocateDarksporeViewModel : ViewModelBase, IDialogViewModel<(string winePrefix, string wineExecutable, string darksporeInstallPath)>
    {
        string _wineExecutableFromBrowseDialog = new Func<string>(() => TryGetDefaultWINEExecutable(out string exec) ? exec : string.Empty)();
        public string WineExecutableFromBrowseDialog
        {
            get => _wineExecutableFromBrowseDialog;
            set => RASIC(ref _wineExecutableFromBrowseDialog, value);
        }

        string _winePrefixFromBrowseDialog = new Func<string>(() => TryGetDefaultWINEPrefix(out string prefix) ? prefix : string.Empty)();
        public string WinePrefixFromBrowseDialog
        {
            get => _winePrefixFromBrowseDialog;
            set => RASIC(ref _winePrefixFromBrowseDialog, value);
        }

        bool _showWineBrowseControls = !OperatingSystem.IsWindows();
        public bool ShowWineBrowseControls
        {
            get => _showWineBrowseControls;
            protected set => RASIC(ref _showWineBrowseControls, value);
        }

        static bool TryGetDefaultWINEExecutable(out string wineExec)
        {
            if (OperatingSystem.IsWindows()) //WINE is just drunk Windows, change my mind
                goto ret;

            if (OperatingSystem.IsLinux())
                return Linux_TryGetDefaultWINEExecutable(out wineExec);

            ret:
            Debug.WriteLine($"{nameof(TryGetDefaultWINEExecutable)}: {nameof(wineExec) != null}");
            wineExec = null;
            return false;
        }
        static bool TryGetDefaultWINEPrefix(out string winePrefix)
        {
            if (OperatingSystem.IsWindows())
                goto ret;

            if (OperatingSystem.IsLinux())
                return Linux_TryGetDefaultWINEPrefix(out winePrefix);

            ret:
            Debug.WriteLine($"{nameof(TryGetDefaultWINEPrefix)}: {nameof(winePrefix) != null}");
            winePrefix = null;
            return false;
        }
    }
}
