namespace MVVM.ViewModel {
    class ResultNodeVM {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }
        public bool BelongPath { get; set; }
        public int PathIndex { get; set; }
        public int Index { get; set; }

        public ResultNodeVM(int index, string name, double x, double y, bool selected, bool belongPath) {
            Index = index;
            Name = name;
            X = x;
            Y = y;
            Selected = selected;
            BelongPath = belongPath;
        }
    }
}
