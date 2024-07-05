using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using TemplateBuildFunc = System.Func<Avalonia.Controls.Primitives.TemplatedControl, Avalonia.Controls.INameScope, Avalonia.Controls.Control>;

namespace ReCap.CommonUI
{
    public delegate bool MatchIconResourceFunc(object resource, out TemplateBuildFunc buildFunc);
    public class IconPresenter
        : IconElement
    {
        /// <summary>
        /// Defines the <see cref="IconKey"/> property.
        /// </summary>
        public static readonly StyledProperty<object> IconKeyProperty =
            AvaloniaProperty.Register<IconPresenter, object>(nameof(IconKey));
        /// <summary>
        /// Gets or sets the resource key for the icon that will be displayed.
        /// </summary>
        public object IconKey
        {
            get => GetValue(IconKeyProperty);
            set => SetValue(IconKeyProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="IconResource"/> property.
        /// </summary>
        public static readonly StyledProperty<object> IconResourceProperty =
            AvaloniaProperty.Register<IconPresenter, object>(nameof(IconResource));
        /*
        public static readonly DirectProperty<IconPresenter, object> IconResourceProperty =
            AvaloniaProperty.RegisterDirect<IconPresenter, object>(nameof(IconResource), o => o._iconResource);
        */
        /// <summary>
        /// Gets or sets the icon that will be displayed.
        /// </summary>
        public object IconResource
        {
            get => GetValue(IconResourceProperty);
            set => SetValue(IconResourceProperty, value);
        }


        object _iconResource = null;
        static IconPresenter()
        {
            Extensions.MakeControlTypeNonInteractive<IconPresenter>();
            
            IconKeyProperty.Changed.AddClassHandler<IconPresenter>(IconKey_Changed);
            IconResourceProperty.Changed.AddClassHandler<IconPresenter>(IconResource_Changed);
        }

        IDisposable _bound = null;
        private static void IconKey_Changed(IconPresenter presenter, AvaloniaPropertyChangedEventArgs args)
            => presenter?.OnIconKeyChanged(args);
        void OnIconKeyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            _bound?.Dispose();

            var newKey = e.NewValue;
            if (newKey != null)
                _bound = Bind(IconResourceProperty, Resources.GetResourceObservable(newKey));
        }




        static bool Matcher_PathIcon(object resource, out TemplateBuildFunc buildFunc)
        {
            buildFunc = default;
            if (!(resource is Geometry geom))
                return false;
            
            buildFunc = (c, n) => new PathIcon()
            {
                Data = geom
            };
            return true;
        }


        static readonly IEnumerable<MatchIconResourceFunc> _matchers = new List<MatchIconResourceFunc>()
        {
            Matcher_PathIcon,
        }.AsReadOnly();
        static void IconResource_Changed(IconPresenter presenter, AvaloniaPropertyChangedEventArgs args)
            => presenter?.OnIconResourceChanged(args);
        void OnIconResourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var iconRes = e.NewValue;

            if (iconRes is IControlTemplate ctrlTemplate)
            {
                Template = ctrlTemplate;
                return;
            }
            
            TemplateBuildFunc func = null;
            foreach (var matcher in _matchers)
            {
                if (matcher(iconRes, out func) && (func != null))
                {
                    Template = new FuncControlTemplate(func);
                    return;
                }
            }
            Template = EmptyTemplate;
        }
        static readonly FuncControlTemplate EmptyTemplate = new FuncControlTemplate((c, n) => new Control());
    }

    public class TranslationMatrixExtension
        : MarkupExtension
    {
        readonly Vector _translation;
        public TranslationMatrixExtension(double x, double y)
            : this(new Vector(x, y))
        {}
        public TranslationMatrixExtension(string vectorStr)
            : this(Vector.Parse(vectorStr))
        {}
        public TranslationMatrixExtension(Vector translation)
        {
            _translation = translation;
        }


        public object ProvideValue()
            => new MatrixTransform(Matrix.CreateTranslation(_translation));
        public override object ProvideValue(IServiceProvider serviceProvider)
            => ProvideValue();
    }
}