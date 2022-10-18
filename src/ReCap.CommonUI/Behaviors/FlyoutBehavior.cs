using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ReCap.CommonUI
{
    public class FlyoutBehavior : AvaloniaObject
    {
#if ALSO_NAH
        public static readonly AttachedProperty<bool> ShowOnHoverProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, Control, bool>("ShowOnHover", false);
        public static bool GetShowOnHover(Control ctrl)
            => ctrl.GetValue(ShowOnHoverProperty);
        public static void SetShowOnHover(Control ctrl, bool newValue)
            => ctrl.SetValue(ShowOnHoverProperty, newValue);
#if NAH


        static FlyoutBehavior()
        {
            ShowOnHoverProperty.Changed.Subscribe(x => ShowOnHoverPropertyChanged((Control)(x.Sender), x.OldValue.GetValueOrDefault(), x.NewValue.GetValueOrDefault()));
            SetPresenterForFlyoutProperty.Changed.Subscribe(x => SetPresenterForFlyoutPropertyChanged((FlyoutPresenter)(x.Sender), x.OldValue.GetValueOrDefault(), x.NewValue.GetValueOrDefault()));
            //FlyoutShowMode.Transient
        }
#endif
        static void ShowOnHoverPropertyChanged(Control sender, bool oldValue, bool newValue)
        {
            if (sender == null)
                return;

            var flyout = FlyoutBase.GetAttachedFlyout(sender);
            if (flyout == null)
                return;

            if (/*(!oldValue) && */newValue)
            {
                //_flyoutBehaviors.Add(sender, new FlyoutBehavior(sender));
                /*sender.PointerEnter += Element_PointerEnter;
                sender.PointerLeave += Element_PointerLeave;*/
                SetAttachedTarget(flyout, sender);
                Debug.WriteLine("Set target for attached flyout");
            }
            else if (/*oldValue && */(!newValue))
            {
                /*if (_flyoutBehaviors.TryGetValue(sender, out FlyoutBehavior value))
                {
                    _flyoutBehaviors.Remove(sender);
                    value.Detach();
                }*/
                /*sender.PointerEnter -= Element_PointerEnter;
                sender.PointerLeave -= Element_PointerLeave;*/
                SetAttachedTarget(flyout, null);
            }
        }
#if NAH
        static void SetPresenterForFlyoutPropertyChanged(FlyoutPresenter sender, bool oldValue, bool newValue)
        {
            if (sender == null)
                return;

            if (newValue)
            {
                SetPresenter(sender.ContextFlyout, sender);
                sender.AttachedToVisualTree += FlyoutPresenter_AttachedToVisualTree;
            }
            else
            {
                SetPresenter(sender.ContextFlyout, null);
                sender.AttachedToVisualTree -= FlyoutPresenter_AttachedToVisualTree;
            }
        }

        static readonly Dictionary<Control, FlyoutBehavior> _flyoutBehaviors = new Dictionary<Control, FlyoutBehavior>();





        Control _associated = null;
//#endif
        bool _alreadyOpened = false;
//#if NAH
        private FlyoutBehavior(Control associated)
        {
            _associated = associated;
            Attach();
        }

        void Attach()
        {
            _associated.PointerEnter += Associated_PointerEnter;
            _associated.PointerLeave += Associated_PointerLeave;
            _associated.ContextRequested += Associated_ContextRequested;
        }

        bool _suppressContextRequest = true;
        private void Associated_ContextRequested(object sender, ContextRequestedEventArgs e)
        {
            if (_suppressContextRequest)
            {
                e.RoutedEvent = null;
                e.Source = null;
                e.Handled = true;
                return;
            }
            else
                _suppressContextRequest = true;
        }

        void Detach()
        {
            _associated.PointerEnter -= Associated_PointerEnter;
            _associated.PointerLeave -= Associated_PointerLeave;
            _associated.ContextRequested -= Associated_ContextRequested;
            _associated = null;
        }

//#endif
        static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private void Associated_PointerEnter(object sender, PointerEventArgs e)
        {
            Debug.WriteLine(nameof(Associated_PointerEnter));
#if NOPE
            if (!(sender is Control associated))
                return;

            var flyout = Flyout.GetAttachedFlyout(associated);
            if (!(_alreadyOpened || flyout.IsOpen))
            {
                Debug.WriteLine("Flyout shown");
                //Flyout.ShowAttachedFlyout(_associated);
                flyout.ShowAt(associated);
                //flyout.Closed += Flyout_Closed;
                //_associated.PointerEnter -= Element_PointerEnter;
                _alreadyOpened = true;
            }
#else
            var flyout = _associated.ContextFlyout;
            if (flyout == null)
                return;

            if ((!_alreadyOpened) && (!flyout.IsOpen))
            {
                _suppressContextRequest = false;
                var pointerArgs = new PointerEventArgs(
                    Control.ContextRequestedEvent
                    , _associated
                    , new Pointer(Pointer.GetNextFreeId(), PointerType.Mouse, false)
                    , null
                    , new Point()
                    , Convert.ToUInt64(DateTime.UtcNow.ToUniversalTime().Subtract(UNIX_EPOCH).TotalMilliseconds)
                    , new PointerPointProperties(RawInputModifiers.RightMouseButton, PointerUpdateKind.RightButtonReleased)
                    , KeyModifiers.None
                );
                _associated.RaiseEvent(new ContextRequestedEventArgs(pointerArgs));
                flyout.Closed += Flyout_Closed;
            }
#endif
        }

        private void Flyout_Closed(object sender, EventArgs e)
        {
#if NOPE
            if (sender is FlyoutBase flyout)
            {
                var target = flyout.Target;
                if (GetShowOnHover(target))
                {
                    target.PointerEnter += Element_PointerEnter;
                }
            }
#else
            _alreadyOpened = false;
#endif
        }


        private void Associated_PointerLeave(object sender, PointerEventArgs e)
        {
            Debug.WriteLine(nameof(Associated_PointerLeave));
#if NOPE
            if (!_alreadyOpened)
                return;

            var flyout = Flyout.GetAttachedFlyout(_associated);
            var flyoutPresenter = GetPresenter(flyout);

            /*if (!(flyout is IVisual flyoutEl))
                return;
            //Avalonia.VisualTree.
            var flyoutContent = flyoutEl.FindDescendantOfType<Control>(false);*/
            if (flyoutPresenter != null)
            {
                if ((!_associated.IsPointerOver) && (!flyoutPresenter.IsPointerOver))
                {
                    Debug.WriteLine("Flyout hidden");
                    flyout.Hide();
                    _alreadyOpened = false;
                }
            }
            else
            {
                Debug.WriteLine("presenter is null");
            }
            /*if (sender is Control ctrl)
                //Flyout.ShowAttachedFlyout(ctrl, false);
                //Flyout.HideAttachedFlyout(ctrl);
                //???
             }*/
#endif
        }
#else
        public static readonly AttachedProperty<bool> OpenWhenIsProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, FlyoutBase, bool>("OpenWhenIs", false, defaultBindingMode: BindingMode.OneWay);
        public static bool GetOpenWhenIs(FlyoutBase ctrl)
            => ctrl.GetValue(OpenWhenIsProperty);
        public static void SetOpenWhenIs(FlyoutBase ctrl, bool newValue)
            => ctrl.SetValue(OpenWhenIsProperty, newValue);



        public static readonly AttachedProperty<Control> AttachedTargetProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, FlyoutBase, Control>("AttachedTarget", null);
        public static Control GetAttachedTarget(FlyoutBase ctrl)
            => ctrl.GetValue(AttachedTargetProperty);
        public static void SetAttachedTarget(FlyoutBase ctrl, Control newValue)
            => ctrl.SetValue(AttachedTargetProperty, newValue);

        static FlyoutBehavior()
        {
            OpenWhenIsProperty.Changed.Subscribe(x => OpenWhenIsPropertyChanged((FlyoutBase)(x.Sender), x.OldValue.GetValueOrDefault(), x.NewValue.GetValueOrDefault()));
            //Flyout.SetAttachedFlyout(new Border(), null);

            ShowOnHoverProperty.Changed.Subscribe(x => ShowOnHoverPropertyChanged((Control)(x.Sender), x.OldValue.GetValueOrDefault(), x.NewValue.GetValueOrDefault()));
        }

        static void OpenWhenIsPropertyChanged(FlyoutBase flyout, bool oldValue, bool newValue)
        {
            /*if (!x.NewValue.HasValue)
                return;*/

            if (newValue)
            {
                Debug.WriteLine("OPENWHENIS IS TRUE");
                var target = GetAttachedTarget(flyout);
                if (target == null)
                {
                    Debug.WriteLine("...but no target :(");
                    return;
                }

                flyout.ShowAt(target, false);
            }
            else
            {
                Debug.WriteLine("OPENWHENIS IS FALSE");
                flyout.Hide();
            }
        }
#endif
#endif
        public static readonly AttachedProperty<bool> IsAttachedFlyoutShownProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, Control, bool>("IsAttachedFlyoutShown", false, defaultBindingMode: BindingMode.OneWay);
        public static bool GetIsAttachedFlyoutShown(Control ctrl)
            => ctrl.GetValue(IsAttachedFlyoutShownProperty);
        public static void SetIsAttachedFlyoutShown(Control ctrl, bool newValue)
            => ctrl.SetValue(IsAttachedFlyoutShownProperty, newValue);
        
        
        
        
        public static readonly AttachedProperty<bool> IsAttachedFlyoutHiddenProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, Control, bool>("IsAttachedFlyoutHidden", false, defaultBindingMode: BindingMode.OneWay);
        public static bool GetIsAttachedFlyoutHidden(Control ctrl)
            => ctrl.GetValue(IsAttachedFlyoutHiddenProperty);
        public static void SetIsAttachedFlyoutHidden(Control ctrl, bool newValue)
            => ctrl.SetValue(IsAttachedFlyoutHiddenProperty, newValue);




        public static readonly AttachedProperty<FlyoutPresenter> PresenterProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, FlyoutBase, FlyoutPresenter>("Presenter", null);
        public static FlyoutPresenter GetPresenter(FlyoutBase ctrl)
            => ctrl.GetValue(PresenterProperty);
        public static void SetPresenter(FlyoutBase ctrl, FlyoutPresenter newValue)
            => ctrl.SetValue(PresenterProperty, newValue);




        public static readonly AttachedProperty<bool> SetPresenterForFlyoutProperty
            = AvaloniaProperty.RegisterAttached<FlyoutBehavior, FlyoutPresenter, bool>("SetPresenterForFlyout", false);
        public static bool GetSetPresenterForFlyout(FlyoutPresenter ctrl)
            => ctrl.GetValue(SetPresenterForFlyoutProperty);
        public static void SetSetPresenterForFlyout(FlyoutPresenter ctrl, bool newValue)
            => ctrl.SetValue(SetPresenterForFlyoutProperty, newValue);




        static FlyoutBehavior()
        {
            //FlyoutPresenter.IsVisibleProperty.Changed.AddClassHandler<FlyoutPresenter>(FlyoutPresenter_IsVisibleChanged);
            IsAttachedFlyoutShownProperty.Changed.AddClassHandler<Control>((sender, e) => IsAttachedFlyoutShownPropertyChanged(sender, (bool)(e.NewValue)));
            PresenterProperty.Changed.AddClassHandler<FlyoutBase>(PresenterPropertyChanged);
            /*IsAttachedFlyoutHiddenProperty.Changed.AddClassHandler<Control>((sender, e) =>
            {
                var flyout = FlyoutBase.GetAttachedFlyout(sender);
                if (!GetIsAttachedFlyoutShown(sender))
                    HideFlyout(flyout);
            });*/
            SetPresenterForFlyoutProperty.Changed.AddClassHandler<FlyoutPresenter>((sender, e) =>
            {
                
                var flyout = sender.ContextFlyout;
                if ((e.NewValue is bool newValue) && newValue)
                {
                    if (flyout != null)
                        SetPresenter(flyout, sender);
                    else
                    {
                        //sender.GetPropertyChangedObservable(FlyoutPresenter.IsVisibleProperty).Subscribe(FlyoutPresenter_IsVisibleChanged);
                    }

                    sender.PointerLeave += Presenter_PointerLeave;
                }
                else
                {
                    if (flyout != null)
                        SetPresenter(null, sender);
                    //sender.LayoutUpdated -= FlyoutPresenter_LayoutUpdated;
                    sender.PointerLeave -= Presenter_PointerLeave;
                }
            });
        }

        static void PresenterPropertyChanged(FlyoutBase sender, AvaloniaPropertyChangedEventArgs e)
        {
            /*if (!((e.NewValue != null) && (e.NewValue is FlyoutPresenter presenter)))
                return;

            presenter.PointerLeave += Presenter_PointerLeave;
            var flyout = presenter.ContextFlyout;
            if (flyout == null)
                return;
            else if (flyout.Target == null)
                return;
            else
            {
                HideFlyout(flyout.Target, presenter, flyout);
                Debug.WriteLine("PresenterPropertyChanged");
            }*/
        }

        private static void Presenter_PointerLeave(object sender, PointerEventArgs e)
        {
            Debug.WriteLine("Presenter_PointerLeave");
            if (!(sender is FlyoutPresenter presenter))
                return;
            Debug.WriteLine("sender is FlyoutPresenter");

            var flyout = presenter.ContextFlyout;
            var target = flyout.Target;
            /*if (target != null)
            {*/
            HideFlyout(target, presenter, flyout);
                /*if (!target.IsPointerOver)
                    flyout.Hide();
            }*/
        }

        static void IsAttachedFlyoutShownPropertyChanged(Control sender, bool newValue)
        {
            var flyout = FlyoutBase.GetAttachedFlyout(sender);
            if (flyout == null)
                return;

            var presenter = GetPresenter(flyout);


            if (newValue)
            {
                //if (!flyout.IsOpen) // && (flyout.Target != sender))
                {
                    //var focused = FocusManager.Instance.Current;
                    flyout.ShowAt(sender);
                    //focused.Focus();
                    var topLevel = sender.FindAncestorOfType<TopLevel>();
                    
                    var ldoLayer = LightDismissOverlayLayer.GetLightDismissOverlayLayer(topLevel);
                    //LightDismissOverlayLayer.GetLightDismissOverlayLayer(topLevel).IsVisible = false;
                    //ldoLayer.InputPassThroughElement = topLevel; //.Find<ContentControl>("SelectedSaveArea");
                    Debug.WriteLine($"ldoLayer.InputPassThroughElement: {ldoLayer.InputPassThroughElement != null}");
                    //ldoLayer.IsVisible = false;
                        //[LightDismissOverlayLayer.WidthProperty]

                    /*double x = 16;
                    double y = 30;
                    OverlayLayer.GetOverlayLayer(topLevel).Margin = new Thickness(-x, -y, x, y);*/
                    Debug.WriteLine("Flyout shown");
                    if (presenter != null)
                        Debug.WriteLine("Presenter has parent?: " + (presenter.Parent != null));
                }
            }
            else //if (presenter != null)
            {
                HideFlyout(sender, presenter, flyout);

                /*if (!presenter.IsPointerOver) // ? (!presenter.IsPointerOver) : true)
                    HideFlyout(flyout);*/
            }
        }

        private static void FlyoutPresenter_IsVisibleChanged(FlyoutPresenter sender, AvaloniaPropertyChangedEventArgs e)
        {
            Debug.WriteLine("FlyoutPresenter_IsVisibleChanged");
            /*if (!(sender is FlyoutPresenter presenter))
                return;*/
            if (!((e.NewValue is bool newValue) && newValue))
                return;
            
            var presenter = sender;

            var flyout = presenter.ContextFlyout;
            if (flyout != null)
            {
                SetPresenter(flyout, presenter);
                //presenter.LayoutUpdated -= FlyoutPresenter_LayoutUpdated;
            }
        }

        static void HideFlyout(Control target, FlyoutPresenter presenter, FlyoutBase flyout)
        {
            if (presenter != null)
                Debug.WriteLine("Presenter has parent?: " + (presenter.Parent != null));

            if (ShouldHide(target) && ShouldHide(presenter))
            {
                HideFlyout(flyout);
            }
        }

        static bool ShouldHide(IControl ctrl)
        {
            return (ctrl != null)
                ? ctrl.IsVisible && (!ctrl.IsPointerOver)
                : true
            ;
        }


        static void HideFlyout(FlyoutBase flyout)
        {
            if (flyout == null)
                return;
            var presenter = GetPresenter(flyout);
            flyout.Hide();
            Debug.WriteLine("Flyout hidden");
            if (presenter != null)
                Debug.WriteLine("Presenter has parent?: " + (presenter.Parent != null));
        }
    }
}