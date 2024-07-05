using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Animators;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace ReCap.CommonUI
{
    /*public enum FlashingState
    {
        None,
        Flashing,
        Flashed
    }

    */
    public class Flash : TemplatedControl
    {
        public const string FLASH_CLASS = "flashing";
        ObservableCollection<Visual> _visuals = new ObservableCollection<Visual>();
        TimeSpan FlashDuration = TimeSpan.FromMilliseconds(400);
        Timer _timer = null;
        int _flashIndex = 0;
        Visual _current = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            if (_timer != null)
                _timer.Stop();
            
            _visuals.Clear();
            _flashIndex = 0;



            int i = 1;
            while (true)
            {
                try
                {
                    var vis = e.NameScope.Find<Visual>($"PART_Flash{i}");
                    if (vis == null)
                        break;
                    _visuals.Add(vis);
                    i++;
                }
                catch
                {
                    break;
                }
            }

            _timer = new Timer(FlashDuration.TotalMilliseconds);
            _timer.Elapsed += (_, __) =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    if (_visuals.Count <= 0)
                        return;

                    if (_current != null)
                        _current.Classes.Remove(FLASH_CLASS);

                    _current = _visuals[_flashIndex];
                    _flashIndex++;
                    if (_flashIndex >= _visuals.Count)
                        _flashIndex = 0;

                    //_timer.Stop();
                    _current.Classes.Add(FLASH_CLASS);
                });
            };
            _timer.Start();
        }

        static Flash()
        {
            Extensions.MakeControlTypeNonInteractive<Flash>();
        }
    }
        /*
        public static readonly StyledProperty<FlashingState> FlashStateProperty =
            AvaloniaProperty.Register<Flash, FlashingState>(nameof(FlashState), FlashingState.None);
        public FlashingState FlashState
        {
            get => GetValue(FlashStateProperty);
            set => SetValue(FlashStateProperty, value);
        }

        
        public static readonly StyledProperty<bool> IsFirstFlashProperty =
            AvaloniaProperty.Register<Flash, bool>(nameof(IsFirstFlash), false);
        public bool IsFirstFlash
        {
            get => GetValue(IsFirstFlashProperty);
            set => SetValue(IsFirstFlashProperty, value);
        }

        public static readonly StyledProperty<Flash> NextFlashProperty = AvaloniaProperty.Register<Flash, Flash>(nameof(NextFlash), null);
        public Flash NextFlash
        {
            get => GetValue(NextFlashProperty);
            set => SetValue(NextFlashProperty, value);
        }

        /*public static Animator<FlashingState> FlashStateAnimator
        {
            get;
        } = new ();* /
        static Flash()
        {
            //NextFlashProperty.Changed.AddClassHandler<Flash>(FlashTargetPropertyChanged);
            FlashStateProperty.Changed.AddClassHandler<Flash>(FlashStatePropertyChanged);

            AffectsRender<Flash>(FlashStateProperty, NextFlashProperty);
            Animation.RegisterAnimator<FlashingStateAnimator>(prop => prop.PropertyType == typeof(FlashingState));
        }

        public Flash()
            : base()
        {
            
        }

        /*static void FlashTargetPropertyChanged(Flash sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                sender.PropertyChanged += Visual_PropertyChanged;
            else
                sender.PropertyChanged -= Visual_PropertyChanged;
        }* /

        static void FlashStatePropertyChanged(Flash sender, AvaloniaPropertyChangedEventArgs e)
        {
            sender.OnFlashStatePropertyChanged(e);
        }

        protected virtual void OnFlashStatePropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is FlashingState state)
            {
                if (state == FlashingState.Flashed)
                {
                    if (NextFlash != null)
                    {
                        NextFlash.SetValue(FlashStateProperty, FlashingState.Flashing, BindingPriority.Animation);
                    }
                    
                    SetValue(FlashStateProperty, FlashingState.None, BindingPriority.Animation);
                }
            }
        }

        private static void Visual_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            /*if (IsFirstFlash)
                IsFlashing = true;* /
        }
    }

    public class FlashingStateAnimator : Animator<FlashingState>
    {
        public override FlashingState Interpolate(double progress, FlashingState oldValue, FlashingState newValue)
        {
            return (progress < 1)
                ? oldValue
                : newValue;
        }
    }*/
}
