using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MVVM.Other {
    class MouseButtonEventArgsToPointConverter : IEventArgsConverter {
        private double nodeSize = 30;
        public object Convert(object value, object parameter) {
            var args = (MouseEventArgs)value;
            var element = (FrameworkElement)parameter;
            Point realPoint = args.GetPosition(element);
            Point addNodePoint = realPoint;
            addNodePoint.X -= nodeSize / 2;
            addNodePoint.Y -= nodeSize / 2;
            return new List<Point> { realPoint, addNodePoint };
        }
    }
}
