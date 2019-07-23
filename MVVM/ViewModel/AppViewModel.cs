using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MVVM {
    class AppViewModel : INotifyPropertyChanged {
        private Graph graph = new Graph();
        public ObservableCollection<NodeVM> NodesVM { get; set; } = new ObservableCollection<NodeVM>();
        public ObservableCollection<EdgeVM> EdgesVM { get; set; } = new ObservableCollection<EdgeVM>();
        public bool AllowNode { get; set; }
        public bool AllowEdge { get; set; }
        public bool AllowSelect { get; set; }
        public Node FirstNode { get; set; }
        public double FirstX { get; set; }
        public double FirstY { get; set; }
        public double AddNodeMouseX { get; set; }
        public double AddNodeMouseY { get; set; }
        public double MouseX { get; set; }
        public double MouseY { get; set; }
        // temp
        public string strStartNode { get; set; }
        public string strEndNode { get; set; }
        public string length { get; set; }
        public string path { get; set; }
        //
        public SearchResult SearchResult { get; set; }
        public IDialogService dialogService;

        public AppViewModel(IDialogService dialogService) {
            this.dialogService = dialogService;
        }

        private bool changingCost;
        public bool ChangingCost {
            get { return changingCost; }
            set {
                changingCost = value;
                OnPropertyChanged("ChangingCost");
            }
        }

        private bool allowSearch;
        public bool AllowSearch {
            get { return allowSearch; }
            set {
                allowSearch = value;
                OnPropertyChanged("AllowSearch");
            }
        }
        private Node firstSelected;
        public Node FirstSelected {
            get { return firstSelected; }
            set {
                firstSelected = value;
                UpdateAllowSearch();
            }
        }
        private Node secondSelected;
        public Node SecondSelected {
            get { return secondSelected; }
            set {
                secondSelected = value;
                UpdateAllowSearch();
            }
        }

        private MyRelayCommand canvasMouseMove;
        public MyRelayCommand CanvasMouseMove {
            get {
                return canvasMouseMove ?? (canvasMouseMove = new MyRelayCommand(obj => {
                    List<Point> twoPoints = obj as List<Point>;
                    MouseX = twoPoints[0].X;
                    MouseY = twoPoints[0].Y;
                    AddNodeMouseX = twoPoints[1].X;
                    AddNodeMouseY = twoPoints[1].Y;
                }));
            }
        }

        private MyRelayCommand canvasMouseDown;
        public MyRelayCommand CanvasMouseDown {
            get {
                return canvasMouseDown ?? (canvasMouseDown = new MyRelayCommand(obj => {
                    if (AllowNode) {
                        Node newNode = new Node();
                        graph.AddNode(newNode);
                        NodesVM.Add(new NodeVM(newNode.Name, AddNodeMouseX, AddNodeMouseY));
                    }
                }));
            }
        }

        private RelayCommand<NodeVM> nodeMouseDown;
        public ICommand NodeMouseDown {
            get {
                return nodeMouseDown ?? (nodeMouseDown = new RelayCommand<NodeVM>(obj => {
                    if (AllowEdge) {
                        FirstNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Node);
                        FirstX = MouseX;
                        FirstY = MouseY;
                    }
                    if (AllowSelect) {
                        NodesVM.FirstOrDefault(o => o.Node == obj.Node).InvertSelected();
                        Node currentNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Node);
                        if (currentNode == FirstSelected) {
                            FirstSelected = null;
                        }
                        else if (currentNode == SecondSelected) {
                            SecondSelected = null;
                        }
                        else {
                            if ((FirstSelected != null) && (SecondSelected != null)) {
                                NodesVM.FirstOrDefault(o => o.Node == FirstSelected.Name).InvertSelected();
                                NodesVM.FirstOrDefault(o => o.Node == SecondSelected.Name).InvertSelected();
                                FirstSelected = null;
                                SecondSelected = null;
                            }
                            if (FirstSelected == null) {
                                FirstSelected = currentNode;
                            }
                            else {
                                SecondSelected = currentNode;
                            }
                        }
                    }
                }));
            }
        }

        private RelayCommand<NodeVM> nodeMouseUp;
        public ICommand NodeMouseUp {
            get {
                return nodeMouseUp ?? (nodeMouseUp = new RelayCommand<NodeVM>(obj => {
                    if (AllowEdge) {
                        Node secondNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Node);
                        if (FirstNode != secondNode) {
                            Edge edge = new Edge(FirstNode, secondNode);
                            graph.AddEdge(edge);
                            EdgeVM edgeVM = new EdgeVM(edge.Cost, FirstX, FirstY, MouseX, MouseY);
                            EdgesVM.Add(edgeVM);
                            UpdateChangingCost();
                        }
                    }
                }));
            }
        }

        private RelayCommand<EdgeVM> edgeMouseDown;
        public ICommand EdgeMouseDown {
            get {
                return edgeMouseDown ?? (edgeMouseDown = new RelayCommand<EdgeVM>(obj => {
                    obj.InvertSelected();
                    UpdateChangingCost();
                }));
            }
        }

        private MyRelayCommand clearCommand;
        public MyRelayCommand ClearCommand {
            get {
                return clearCommand ?? (clearCommand = new MyRelayCommand(obj => {
                    
                }));
            }
        }

        private MyRelayCommand searchCommand;
        public MyRelayCommand SearchCommand {
            get {
                return searchCommand ?? (searchCommand = new MyRelayCommand(obj => {
                    SearchResult = graph.SearchPath(FirstSelected, SecondSelected);
                    // temp
                    strStartNode = firstSelected.Name;
                    strEndNode = secondSelected.Name;
                    length = SearchResult.PathLength.ToString();
                    foreach (var item in SearchResult.Path) {
                        path += item.Name + " ";
                    }
                    //
                    if (dialogService.Show(this) == true) { }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void UpdateChangingCost() {
            var selected = EdgesVM.Where(o => o.Selected);
            ChangingCost = (selected.Count() != 0);
        }

        private void UpdateAllowSearch() {
            AllowSearch = (FirstSelected != null) && (SecondSelected != null);
        }
    }

    class NodeVM : INotifyPropertyChanged {
        public string Node { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }

        public NodeVM(string node, double x, double y) {
            Node = node;
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

    class EdgeVM : INotifyPropertyChanged {
        public int Cost { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }

        public EdgeVM(int cost, double x1, double y1, double x2, double y2) {
            double widthForRectangle = 25;
            double heightForRectangle = 15;
            Cost = cost;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X = (x1 + x2) / 2 - widthForRectangle / 2 + widthForRectangle / 5;
            Y = (y1 + y2) / 2 - heightForRectangle / 2;
            Selected = true;
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
