using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ReCap.CommonUI
{
    [TemplatePart(_PART_CloseButton, typeof(Button))]
    public partial class Closeable
        : ContentControl
        , ICommandSource
    {
#nullable enable
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly StyledProperty<ICommand?> CommandProperty =
            Button.CommandProperty.AddOwner<Closeable>();
            //AvaloniaProperty.Register<Closeable, ICommand?>(nameof(Command), enableDataValidation: true);
        /// <summary>
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the button is clicked.
        /// </summary>
        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }




        /// <summary>
        /// Defines the <see cref="CommandParameter"/> property.
        /// </summary>
        public static readonly StyledProperty<object?> CommandParameterProperty =
            Button.CommandParameterProperty.AddOwner<Closeable>();
            //AvaloniaProperty.Register<Closeable, object?>(nameof(CommandParameter));
        /// <summary>
        /// Gets or sets a parameter to be passed to the <see cref="Command"/>.
        /// </summary>
        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }




        /// <summary>
        /// Defines the <see cref="CloseButtonClick"/> event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> CloseButtonClickEvent =
            RoutedEvent.Register<Closeable, RoutedEventArgs>(nameof(CloseButtonClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Raised when the user clicks the close button.
        /// </summary>
        public event EventHandler<RoutedEventArgs>? CloseButtonClick
        {
            add => AddHandler(CloseButtonClickEvent, value);
            remove => RemoveHandler(CloseButtonClickEvent, value);
        }




        /// <summary>
        /// Defines the <see cref="IsOpen"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<Closeable, bool>(nameof(IsOpen), true);
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Closeable"/>
        /// content area is open and visible.
        /// </summary>
        public bool IsOpen
        {
            get => GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }




        /// <summary>
        /// Defines the <see cref="IsCloseButtonVisible"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty =
            AvaloniaProperty.Register<Closeable, bool>(
                nameof(IsCloseButtonVisible)
                , defaultValue: true
                /*
                , defaultBindingMode: BindingMode.TwoWay
                , coerce: CoerceIsExpanded
                */
            );
        public bool IsCloseButtonVisible
        {
            get => GetValue(IsCloseButtonVisibleProperty);
            set => SetValue(IsCloseButtonVisibleProperty, value);
        }
#nullable restore
        
        
        

        const string _PART_CloseButton = "PART_CloseButton";
        
        
        static Closeable()
        {
            //IsVisibleProperty.OverrideDefaultValue<Closeable>(false);
        }


        Button _closeButton = null;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_closeButton != null)
            {
                _closeButton.Click -= CloseButton_Click;
                //_closeButton.Click
                _closeButton = null;
            }
            
            _closeButton = e.NameScope.Get<Button>(_PART_CloseButton);
            //if (_closeButton != null)
            {
                _closeButton.Click += CloseButton_Click;
            }
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            e.RoutedEvent = CloseButtonClickEvent;
            RaiseEvent(e);
            */
            OnCloseButtonClick();
        }

        /// <summary>
        /// Invokes the <see cref="CloseButtonClick"/> event.
        /// </summary>
        void OnCloseButtonClick()
        {
            var e = new RoutedEventArgs(CloseButtonClickEvent);
            RaiseEvent(e);

            if (!e.Handled && Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
                e.Handled = true;
            }

            if (IsOpen)
                IsOpen = false;
        }


        
        //public ICommand Command => throw new NotImplementedException();
        //public object CommandParameter => throw new NotImplementedException();
        public void CanExecuteChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}