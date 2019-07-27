using MVVM.Other;
using MVVM.ViewModel;
using System.Windows;

namespace MVVM {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainViewModel(new DefaultDialogService(this));
        }
    }
}
