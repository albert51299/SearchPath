namespace MVVM.ViewModel {
    class ResultNodeVM {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }
        public bool BelongPath { get; set; }

        public ResultNodeVM(string name, double x, double y, bool selected, bool belongPath) {
            Name = name;
            X = x;
            Y = y;
            Selected = selected;
            BelongPath = belongPath;
        }
    }
}
