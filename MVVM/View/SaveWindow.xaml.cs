using System.Windows;
using System.Windows.Input;

namespace MVVM.View {
    /// <summary>
    /// Interaction logic for SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window {
        public string SessionName { get; set; } = "";
        public bool Confirmed { get; set; } = false;
        public SaveWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            SessionName = tbName.Text;
            Confirmed = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            Close();
        }

        private void TbName_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SessionName = tbName.Text;
                Confirmed = true;
                Close();
            }
        }
    }
}
