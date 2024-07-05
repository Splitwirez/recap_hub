using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReCap.CommonUI
{
    public abstract class GridHelper : AvaloniaObject
    {
        public static readonly AttachedProperty<ColumnDefinitions> BindColumnsProperty =
            AvaloniaProperty.RegisterAttached<GridHelper, Grid, ColumnDefinitions>("BindColumns");
        public static ColumnDefinitions GetBindColumns(Grid grid)
            => grid.GetValue(BindColumnsProperty);
        public static void SetBindColumns(Grid grid, ColumnDefinitions value)
            => grid.SetValue(BindColumnsProperty, value);
        
        public static readonly AttachedProperty<RowDefinitions> BindRowsProperty =
            AvaloniaProperty.RegisterAttached<GridHelper, Grid, RowDefinitions>("BindRows");
        public static RowDefinitions GetBindRows(Grid grid)
            => grid.GetValue(BindRowsProperty);
        public static void SetBindRows(Grid grid, RowDefinitions value)
            => grid.SetValue(BindRowsProperty, value);
        
        static GridHelper()
        {
            BindColumnsProperty.Changed.AddClassHandler<Grid>
            (
                (Grid s, AvaloniaPropertyChangedEventArgs e) => s.ColumnDefinitions = e.GetNewValue<ColumnDefinitions>()
            );

            BindRowsProperty.Changed.AddClassHandler<Grid>
            (
                (Grid s, AvaloniaPropertyChangedEventArgs e) => s.RowDefinitions = e.GetNewValue<RowDefinitions>()
            );
        }
    }

    public abstract class ThicknessToAxisDefinitionsConverterBase<TDef, TDefsList> : IValueConverter where TDef : DefinitionBase where TDefsList : DefinitionList<TDef>
    {
        public abstract TDefsList GetDefinitions(double near, double far);
        public abstract (double near, double far) GetRelevantSides(Thickness value);
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness val)
            {
                var relevant = GetRelevantSides(val);
                return GetDefinitions(relevant.near, relevant.far);
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ThicknessToColumnDefinitionsConverter : ThicknessToAxisDefinitionsConverterBase<ColumnDefinition, ColumnDefinitions>
    {
        public override ColumnDefinitions GetDefinitions(double near, double far)
            => ColumnDefinitions.Parse($"{near},*,{far}");
        public override (double near, double far) GetRelevantSides(Thickness value)
            => (value.Left, value.Right);
    }

    public class ThicknessToRowDefinitionsConverter : ThicknessToAxisDefinitionsConverterBase<RowDefinition, RowDefinitions>
    {
        public override RowDefinitions GetDefinitions(double near, double far)
            => RowDefinitions.Parse($"{near},*,{far}");
        public override (double near, double far) GetRelevantSides(Thickness value)
            => (value.Left, value.Right);
    }
}
