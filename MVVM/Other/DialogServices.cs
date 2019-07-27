using MVVM.View;
using System.Windows;

namespace MVVM.Other {
    interface IDialogService {
        string SessionName { get; set; }
        bool IsConfirmed { get; set; }
        bool ShowSaveWindow();
        //bool ShowLoadWindow();
        void ShowMessage(string message);
        bool ShowSearchResult(object viewModel);
    }

    class DefaultDialogService : IDialogService {
        public bool IsConfirmed { get; set; }
        private Window owner;
        public DefaultDialogService(Window owner) {
            this.owner = owner;
        }
        public string SessionName { get; set; }
        public bool ShowSaveWindow() {
            SaveWindow window = new SaveWindow();
            window.Owner = owner;
            if (window.ShowDialog() == true) {
                return true;
            }
            SessionName = window.SessionName;
            IsConfirmed = window.Confirmed;
            return false;
        }

        /*public bool ShowLoadWindow() {
            
        }*/

        public void ShowMessage(string message) {
            MessageBox.Show(message);
        }

        public bool ShowSearchResult(object viewModel) {
            ShowResultWindow window = new ShowResultWindow();
            window.Owner = owner;
            window.DataContext = viewModel;
            if (window.ShowDialog() == true) {
                return true;
            }
            return false;
        }
    }
}
