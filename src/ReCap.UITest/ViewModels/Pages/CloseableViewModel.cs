using Control = Avalonia.Controls.Control;
using ReCap.Hub.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace ReCap.UITest.ViewModels.Pages
{
    public class CloseableViewModel
        : ViewModelBase
    {
        bool _showCloseButton = true;
        public bool ShowCloseButton
        {
            get => _showCloseButton;
            set => RASIC(ref _showCloseButton, value);
        }




        object _CTemplatedCloseableContent = null;
        public object CTemplatedCloseableContent
        {
            get => _CTemplatedCloseableContent;
            protected set => RASIC(ref _CTemplatedCloseableContent, value);
        }
        
        
        bool _isCTemplatedCloseableOpen = false;
        public bool IsCTemplatedCloseableOpen
        {
            get => _isCTemplatedCloseableOpen;
            set => RASIC(ref _isCTemplatedCloseableOpen, value);
        }




        Control _DContentCloseableContent = null;
        public Control DContentCloseableContent
        {
            get => _DContentCloseableContent;
            protected set => RASIC(ref _DContentCloseableContent, value);
        }


        bool _isDContentCloseableOpen = false;
        public bool IsDContentCloseableOpen
        {
            get => _isDContentCloseableOpen;
            set => RASIC(ref _isDContentCloseableOpen, value);
        }




        public bool IsAnyCloseableOpen
        {
            get => IsCTemplatedCloseableOpen || IsDContentCloseableOpen;
        }




        protected void ShowCloseableWithContentTemplateObject(object dataObject)
        {
            if (IsAnyCloseableOpen)
                return;
            
            CTemplatedCloseableContent = dataObject;
            IsCTemplatedCloseableOpen = true;
        }

        protected void ShowCloseableWithDirectContent(Control control)
        {
            if (IsAnyCloseableOpen)
                return;
            
            DContentCloseableContent = control;
            IsDContentCloseableOpen = true;
        }
        
        public void ShowCloseableWithDirectContentCommand(object parameter)
            => ShowCloseableWithDirectContent((Control)parameter);
        public void ShowCloseableWithContentTemplateObjectCommand(object parameter)
            => ShowCloseableWithContentTemplateObject(parameter);




        public CloseableViewModel()
            : base()
        {
            PropertyChanged += CloseableViewModel_PropertyChanged;
        }

        void CloseableViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;
            if (propName == nameof(IsCTemplatedCloseableOpen))
            {
                if (!IsCTemplatedCloseableOpen)
                    CTemplatedCloseableContent = null;
            }
            else if (propName == nameof(IsDContentCloseableOpen))
            {
                if (!IsDContentCloseableOpen)
                    DContentCloseableContent = null;
            }
            else
            {
                Debug.WriteLine($"Property changed: '{propName}'");
            }
        }
    }
}