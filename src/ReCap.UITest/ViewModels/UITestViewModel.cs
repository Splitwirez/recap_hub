using System;

namespace ReCap.UITest.ViewModels
{
    public partial class UITestViewModel
        : TabsViewModelBase
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
            if (parameter == null)
                return;
            bool increment;
            
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

        public void NextTabCommand(object _ = null)
            => SelectedIndex = WrapTabIndex(SelectedIndex + 1);
        public void PreviousTabCommand(object _ = null)
            => SelectedIndex = WrapTabIndex(SelectedIndex - 1);
        public bool JumpToTab(int index)
        {
            if (!IsTabIndexvalid(index))
                return false;
            
            SelectedIndex = index;
            return true;
        }

        
        public void JumpToTabCommand(object parameter)
        {
            if (parameter == null)
                return;
            int index;
            
            if (parameter is int pBool)
            {
                index = pBool;
            }
            else
            {
                string prmStr = (parameter is string sPrm)
                        ? sPrm
                        : parameter.ToString()
                ;
                
                if (!int.TryParse(prmStr, out index))
                    return;
            }

            JumpToTab(index);
        }


        public UITestViewModel()
        {
        }
    }
}