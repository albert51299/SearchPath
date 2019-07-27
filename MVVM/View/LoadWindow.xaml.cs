using MVVM.ViewModel;
using System.Windows;

namespace MVVM.View {
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window {
        public LoadWindow() {
            InitializeComponent();
            DataContext = new LoadViewModel();
        }
    }
}
