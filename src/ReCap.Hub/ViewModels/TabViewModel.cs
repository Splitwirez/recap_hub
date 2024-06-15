using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public class TabViewModel
        : ViewModelBase
    {
        ViewModelBase _contentVM = null;
        public ViewModelBase ContentVM
        {
            get => _contentVM;
            private set => RASIC(ref _contentVM, value);
        }

        string _titleKey = null;
        public string TitleKey
        {
            get => _titleKey;
            private set => RASIC(ref _titleKey, value);
        }

        string _iconKey = null;
        public string IconKey
        {
            get => _iconKey;
            private set => RASIC(ref _iconKey, value);
        }
        

        public TabViewModel(string titleKey, string iconKey, ViewModelBase contentVM)
            : base()
        {
            TitleKey = titleKey;
            IconKey = iconKey;
            ContentVM = contentVM;
        }
    }
}