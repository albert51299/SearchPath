using System.Windows;
using System.Windows.Controls;

namespace MVVM.Other {
    class GridHelpers {
        public static readonly DependencyProperty ColumnCountProperty =
       DependencyProperty.RegisterAttached(
           "ColumnCount", typeof(int), typeof(GridHelpers),
           new PropertyMetadata(-1, ColumnCountChanged));

        public static int GetColumnCount(DependencyObject obj) {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject obj, int value) {
            obj.SetValue(ColumnCountProperty, value);
        }

        public static void ColumnCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            Grid grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < (int)e.NewValue; i++)
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = GridLength.Auto });
        }
    }
}
