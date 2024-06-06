using System;
using System.Collections.ObjectModel;
using ReCap.Hub.ViewModels;
using ReCap.UITest.ViewModels.Pages;

namespace ReCap.UITest.ViewModels
{
    public class UITestViewModel : ViewModelBase
    {
        double _scaleIncrement = 0.25;
        public double ScaleIncrement
        {
            get => _scaleIncrement;
            private set => RASIC(ref _scaleIncrement, value);
        }
        
        double _scaleMinimum = 0.25;
        public double ScaleMinimum
        {
            get => _scaleMinimum;
            private set => RASIC(ref _scaleMinimum, value);
        }
        
        double _scaleMaximum = 4;
        public double ScaleMaximum
        {
            get => _scaleMaximum;
            private set => RASIC(ref _scaleMaximum, value);
        }


        const double _DEFAULT_SCALE_FAC = 1;
        double _scaleFactor = _DEFAULT_SCALE_FAC;
        public double ScaleFactor
        {
            get => _scaleFactor;
            set
            {
                var clamped = Math.Clamp(value, ScaleMinimum, ScaleMaximum);
                RASIC(ref _scaleFactor, clamped);
            }
        }


        public void ResetScaleFactor()
            => ScaleFactor = _DEFAULT_SCALE_FAC;
        public void ResetScaleFactorCommand(object _)
            => ResetScaleFactor();


        public void AdjustScaleFactor(bool increment)
        {
            if (increment)
                ScaleFactor += ScaleIncrement;
            else
                ScaleFactor -= ScaleIncrement;
        }
        public void AdjustScaleFactorCommand(object parameter)
        {
            bool increment;
            if (parameter == null)
                return;
            
            if (parameter is bool pBool)
            {
                increment = pBool;
            }
            else
            {
                string prmStr = (parameter is string sPrm)
                        ? sPrm
                        : parameter.ToString()
                ;
                
                if (!bool.TryParse(prmStr, out increment))
                    return;
            }

            AdjustScaleFactor(increment);
        }


        ObservableCollection<PageTabViewModel> _tabs = new()
        {
            new PageTabViewModel("Button", new ButtonViewModel()),
            new PageTabViewModel("ComboBox", new ComboBoxViewModel()),
            new PageTabViewModel("ListBox", new ListBoxViewModel()),
            new PageTabViewModel("ScrollViewer", new ScrollViewerViewModel()),
            new PageTabViewModel("TabControl", new TabControlViewModel()),
            new PageTabViewModel("TextBox", new TextBoxViewModel()),
            new PageTabViewModel("WindowChrome", new WindowChromeViewModel()),
        };
        public ObservableCollection<PageTabViewModel> Tabs
        {
            get => _tabs;
            private set => RASIC(ref _tabs, value);
        }
    }
}