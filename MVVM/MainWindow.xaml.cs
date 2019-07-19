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
        private double nodeSize = 25;
        public MainWindow() {
            Resources.Add("NodeSize", nodeSize);
            InitializeComponent();
            DataContext = new AppViewModel();
        }
    }

    public class MouseButtonEventArgsToPointConverter : IEventArgsConverter {
        public object Convert(object value, object parameter) {
            var args = (MouseEventArgs)value;
            var element = (FrameworkElement)parameter;
            var point = args.GetPosition(element);
            return point;
        }
    }
}
