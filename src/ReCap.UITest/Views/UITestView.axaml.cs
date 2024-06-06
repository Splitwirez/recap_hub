using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReCap.UITest.ViewModels;

namespace ReCap.UITest.Views
{
    public partial class UITestView
        : UserControl
    {
        public UITestViewModel VM
        {
            get => DataContext as UITestViewModel;
        }
        
        public UITestView()
        {
            InitializeComponent();
            Dispatcher.UIThread.Post(() => {
                var topLevel = TopLevel.GetTopLevel(this);
                
                topLevel.KeyDown += This_KeyDown;
                topLevel.PointerWheelChanged += This_PointerWheelChanged;
            }, DispatcherPriority.ApplicationIdle);
        }




        void This_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;
            
            var deltaY = e.Delta.Y;
            
            if (deltaY > 0)
                VM.AdjustScaleFactor(true);
            else if (deltaY < 0)
                VM.AdjustScaleFactor(false);
            else
                return;
            
            e.Handled = true;
        }




        static readonly IReadOnlyList<Key> _ZOOM_IN_KEYS = new List<Key>()
        {
            Key.Add,
            Key.OemPlus,
        }.AsReadOnly();
        static readonly IReadOnlyList<Key> _ZOOM_OUT_KEYS = new List<Key>()
        {
            Key.Subtract,
            Key.OemMinus,
        }.AsReadOnly();
        static readonly IReadOnlyList<Key> _ZOOM_RESET_KEYS = new List<Key>()
        {
            Key.D0,
            Key.NumPad0,
        }.AsReadOnly();
        void This_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;
            
            var key = e.Key;
            
            if (_ZOOM_IN_KEYS.Contains(key))
                VM.AdjustScaleFactor(true);
            else if (_ZOOM_OUT_KEYS.Contains(key))
                VM.AdjustScaleFactor(false);
            else if (_ZOOM_RESET_KEYS.Contains(key))
                VM.ResetScaleFactor();
            else
                return;
            
            e.Handled = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
