using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public class ForeignServerViewModel
        : ViewModelBase
    {
        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        double _lastLaunchTime = -1;
        public double LastLaunchTime
        {
            get => _lastLaunchTime;
        }
    }
}
