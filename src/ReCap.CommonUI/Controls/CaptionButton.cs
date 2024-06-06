using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public class CaptionButton
        : Button
    {
        public static readonly DirectProperty<CaptionButton, bool> IsActiveProperty
            = Window.IsActiveProperty.AddOwner<CaptionButton>(s => s.IsActive, (s, v) => s.IsActive = v);
        
        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            private set => SetAndRaise(IsActiveProperty, ref _isActive, value);
        }
        static CaptionButton()
        {
            AffectsRender<CaptionButton>(IsActiveProperty);
        }
    }
}
