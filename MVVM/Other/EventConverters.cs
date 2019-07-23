using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM.Other {
    public class MouseButtonEventArgsToPointConverter : IEventArgsConverter {
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
