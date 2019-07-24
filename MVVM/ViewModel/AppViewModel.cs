﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private string costField;
        public string CostField {
            get { return costField; }
            set {
                costField = value;
                OnPropertyChanged("CostField");
            }
        }
        public Edge CostNotSet { get; set; }
        private bool addingEdge;
        public bool AddingEdge {
            get { return addingEdge; }
            set {
                addingEdge = value;
                OnPropertyChanged("AddingEdge");
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

        private MyRelayCommand canvasMouseUp;
        public MyRelayCommand CanvasMouseUp {
            get {
                return canvasMouseUp ?? (canvasMouseUp = new MyRelayCommand(obj => {
                    if (AllowEdge) {
                        FirstNode = null;
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
                        if ((FirstNode != secondNode) && (FirstNode != null)) {
                            Edge edge = new Edge(FirstNode, secondNode);
                            EdgeVM edgeVM = new EdgeVM(edge.Id, FirstX, FirstY, MouseX, MouseY);
                            EdgesVM.Add(edgeVM);
                            CostNotSet = edge;
                            UpdateAddingEdge();
                            FirstNode = null;
                        }
                    }
                }));
            }
        }

        private MyRelayCommand setEdgeCost;
        public MyRelayCommand SetEdgeCost {
            get {
                return setEdgeCost ?? (setEdgeCost = new MyRelayCommand(obj => {
                    int cost = Convert.ToInt32(obj as string);
                    CostNotSet.Cost = cost;
                    graph.AddEdge(CostNotSet);
                    EdgeVM edgeVM = EdgesVM.FirstOrDefault(o => o.Id == CostNotSet.Id);
                    edgeVM.Cost = cost;
                    edgeVM.InvertSelected();
                    CostNotSet = null;
                    UpdateAddingEdge();
                    CostField = null;
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

        private void UpdateAddingEdge() {
            AddingEdge = (CostNotSet != null);
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
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }
        public int Id { get; set; }
        private int cost = -1;
        public int Cost {
            get { return cost; }
            set {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public EdgeVM(int id, double x1, double y1, double x2, double y2) {
            double widthForRectangle = 25;
            double heightForRectangle = 15;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X = (x1 + x2) / 2 - widthForRectangle / 2 + widthForRectangle / 5;
            Y = (y1 + y2) / 2 - heightForRectangle / 2;
            Selected = true;
            Id = id;
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