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
using System.Windows.Shapes;

namespace MVVM.View {
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window {
        public bool Confirmed { get; set; } = false;

        public ConfirmWindow(string question, string header) {
            InitializeComponent();
            Title = header;
            tbQ.Text = question;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Confirmed = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
