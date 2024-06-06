using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Views
{
    public partial class DialogOverlayView
        : UserControl
    {
        /*
        public static readonly AttachedProperty<bool> SynchronizeAlignmentsProperty =
            AvaloniaProperty.RegisterAttached<DialogOverlayView, Layoutable, bool>("SynchronizeAlignments", false);
        public static bool GetSynchronizeAlignments(Layoutable control)
            => control.GetValue(SynchronizeAlignmentsProperty);
        public static void SetSynchronizeAlignments(Layoutable control, bool value)
            => control.SetValue(SynchronizeAlignmentsProperty, value);
        
        static DialogOverlayView()
        {
            SynchronizeAlignmentsProperty.Changed.AddClassHandler<Layoutable>(SynchronizeAlignmentsProperty_Changed);
        }

        private static void SynchronizeAlignmentsProperty_Changed(Layoutable control, AvaloniaPropertyChangedEventArgs e)
        {
            ContentControl parent = null;
            Layoutable templatedParent = control;
            while (templatedParent != null)
            {
                if (!(templatedParent.TemplatedParent is Layoutable lParent))
                    break;
                
                templatedParent = lParent;
                
                if (lParent is ContentControl templatedParentCC)
                {
                    if (templatedParentCC.Name == "DialogContainer")
                    {
                        parent = templatedParentCC;
                        break;
                    }
                }
            }
            if (parent == null)
                throw new NullReferenceException("DialogContainer");
            /*
            if (!e.GetNewValue<bool>())
                return;
            * /
            
            parent[!HorizontalAlignmentProperty] = control[!HorizontalAlignmentProperty];
            parent[!VerticalAlignmentProperty] = control[!VerticalAlignmentProperty];
            /*
            var boundsObs = control.GetObservable(BoundsProperty);
            parent[!WidthProperty] = boundsObs.Select(x => x.Width).ToBinding();
            parent[!HeightProperty] = boundsObs.Select(x => x.Height).ToBinding();
            * /
        }
        */

        //ContentControl _dialogContainer = null;
        public DialogOverlayView()
        {
            InitializeComponent();
            //_dialogContainer = this.FindControl<ContentControl>("DialogContainer");

            DataContext = new DialogOverlayViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}