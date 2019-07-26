using MVVM.Other;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MVVM.ViewModel {
    class NodeVM : INotifyPropertyChanged {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        [NotMapped]
        public bool Selected { get; set; }

        public NodeVM(string name, double x, double y) {
            Name = name;
            X = x;
            Y = y;
        }

        public void InvertSelected() {
            Selected = !Selected;
            OnPropertyChanged("Selected");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
