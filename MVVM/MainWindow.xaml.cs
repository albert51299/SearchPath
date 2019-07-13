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
        public Point MouseDownPoint { get; set; }
        public Point MouseUpPoint { get; set; }
        public MainWindow() {
            InitializeComponent();
            DataContext = new AppViewModel();
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e) {
            //MouseDownPoint = e.GetPosition(mainCanvas);
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e) {
            //MouseUpPoint = e.GetPosition(mainCanvas);
        }
    }
}
