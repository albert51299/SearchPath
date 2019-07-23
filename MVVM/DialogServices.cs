using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM {
    public interface IDialogService {
        bool Show(object viewModel);
    }

    class ShowResultDialog : IDialogService {
        public bool Show(object viewModel) {
            ShowResultWindow window = new ShowResultWindow();
            window.DataContext = viewModel;
            if (window.ShowDialog() == true) {
                return true;
            }
            return false;
        }
    }
}
