using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new AppViewModel();
        }
    }

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
