using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM.ViewModel {
    class EdgeVM : INotifyPropertyChanged {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }
        //public int AppId { get; set; }
        private int cost;
        public int Cost {
            get { return cost; }
            set {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        //public EdgeVM(int id, double x1, double y1, double x2, double y2) {
        public EdgeVM(double x1, double y1, double x2, double y2) {
            double widthForRectangle = 25;
            double heightForRectangle = 15;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X = (x1 + x2) / 2 - widthForRectangle / 2 + widthForRectangle / 5;
            Y = (y1 + y2) / 2 - heightForRectangle / 2;
            Selected = true;
            //AppId = id;
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
