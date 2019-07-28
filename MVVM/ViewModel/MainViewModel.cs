using Microsoft.EntityFrameworkCore;
using MVVM.Model;
using MVVM.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MVVM.ViewModel {
    class MainViewModel : INotifyPropertyChanged {
        private Graph graph = new Graph();

        private ObservableCollection<NodeVM> nodesVM = new ObservableCollection<NodeVM>();
        public ObservableCollection<NodeVM> NodesVM {
            get { return nodesVM; }
            set {
                nodesVM = value;
                OnPropertyChanged("NodesVM");
            }
        }

        private ObservableCollection<EdgeVM> edgesVM = new ObservableCollection<EdgeVM>();
        public ObservableCollection<EdgeVM> EdgesVM {
            get { return edgesVM; }
            set {
                edgesVM = value;
                OnPropertyChanged("EdgesVM");
            }
        }

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
        public IDialogService dialogService;

        public MainViewModel(IDialogService dialogService) {
            this.dialogService = dialogService;
        }

        public Edge EdgeWithoutCost { get; set; }
        public List<EdgeVM> EdgesVMWithoutCost { get; set; } = new List<EdgeVM>();

        private bool awaitCost;
        public bool AwaitCost {
            get { return awaitCost; }
            set {
                awaitCost = value;
                OnPropertyChanged("AwaitCost");
            }
        } 
            
        private string costField;
        public string CostField {
            get { return costField; }
            set {
                costField = value;
                OnPropertyChanged("CostField");
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
                OnPropertyChanged("FirstSelected");
            }
        }
        private Node secondSelected;
        public Node SecondSelected {
            get { return secondSelected; }
            set {
                secondSelected = value;
                UpdateAllowSearch();
                OnPropertyChanged("SecondSelected");
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
                        graph.Nodes.Add(newNode);
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
                        FirstNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Name);
                        FirstX = MouseX;
                        FirstY = MouseY;
                    }
                    if (AllowSelect) {
                        NodesVM.FirstOrDefault(o => o.Name == obj.Name).InvertSelected();
                        Node currentNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Name);
                        if (currentNode == FirstSelected) {
                            FirstSelected = null;
                        }
                        else if (currentNode == SecondSelected) {
                            SecondSelected = null;
                        }
                        else {
                            if ((FirstSelected != null) && (SecondSelected != null)) {
                                NodesVM.FirstOrDefault(o => o.Name == FirstSelected.Name).InvertSelected();
                                NodesVM.FirstOrDefault(o => o.Name == SecondSelected.Name).InvertSelected();
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
                        Node secondNode = graph.Nodes.FirstOrDefault(o => o.Name == obj.Name);
                        if ((FirstNode != secondNode) && (FirstNode != null)) {
                            EdgeWithoutCost = new Edge(FirstNode.Index, secondNode.Index);
                            EdgeVM edgeVM = new EdgeVM(FirstX, FirstY, MouseX, MouseY);
                            edgeVM.InvertSelected();
                            EdgesVMWithoutCost.Add(edgeVM);
                            EdgesVM.Add(edgeVM);
                            FirstNode = null;
                            AwaitCost = true;
                        }
                    }
                }));
            }
        }

        private MyRelayCommand setEdgeCost;
        public MyRelayCommand SetEdgeCost {
            get {
                return setEdgeCost ?? (setEdgeCost = new MyRelayCommand(obj => {
                    EdgeVM current = EdgesVMWithoutCost.Last();
                    try {
                        int cost = Convert.ToInt32(obj as string);
                        if (cost < 0) {
                            throw new Exception();
                        }
                        EdgeWithoutCost.Cost = cost;
                        graph.AddEdge(EdgeWithoutCost);
                        current.Cost = cost;
                        current.InvertSelected();
                    }
                    catch (Exception) {
                        EdgesVM.Remove(current);
                    }
                    finally {
                        EdgeWithoutCost = null;
                        EdgesVMWithoutCost.Remove(current);
                        CostField = null;
                        AwaitCost = false;
                    }
                }));
            }
        }

        private MyRelayCommand lostFocusCommand;
        public MyRelayCommand LostFocusCommand {
            get {
                return lostFocusCommand ?? (lostFocusCommand = new MyRelayCommand(obj => {
                    foreach (var item in EdgesVMWithoutCost) {
                        EdgesVM.Remove(item);
                    }
                    EdgeWithoutCost = null;
                    CostField = null;
                    AwaitCost = false;
                }));
            }
        }

        private MyRelayCommand clearCommand;
        public MyRelayCommand ClearCommand {
            get {
                return clearCommand ?? (clearCommand = new MyRelayCommand(obj => {
                    graph.Nodes.Clear();
                    graph.Edges.Clear();
                    graph.MatrixReset();
                    NodesVM.Clear();
                    EdgesVM.Clear();
                    FirstNode = null;
                    FirstSelected = null;
                    SecondSelected = null;
                    Node.Number = 0;
                }));
            }
        }

        private MyRelayCommand searchCommand;
        public MyRelayCommand SearchCommand {
            get {
                return searchCommand ?? (searchCommand = new MyRelayCommand(obj => {
                    if (dialogService.ShowSearchResultWindow(NodesVM, EdgesVM, graph.SearchPath(FirstSelected, SecondSelected)) == true) { }
                }));
            }
        }

        private MyRelayCommand saveCommand;
        public MyRelayCommand SaveCommand {
            get {
                return saveCommand ?? (saveCommand = new MyRelayCommand(obj => {
                    if (dialogService.ShowSaveWindow() == true) { }
                    if (dialogService.IsConfirmed) {
                        if (dialogService.SessionName == "") {
                            dialogService.ShowMessage("Session name cannot be empty");
                        }
                        else {
                            using (SearchPathContext db = new SearchPathContext()) {
                                Session sessionWithSameName = db.Sessions.FirstOrDefault(o => o.Name == dialogService.SessionName);
                                if (sessionWithSameName != null) {
                                    dialogService.ShowMessage("Name already used");
                                }
                                else {
                                    Session session = new Session(dialogService.SessionName);
                                    ModelState modelState = new ModelState();
                                    modelState.Session = session;

                                    for (int i = 0; i < NodesVM.Count; i++) {
                                        NodesVM[i].Session = session;
                                        graph.Nodes[i].ModelState = modelState;
                                    }

                                    for (int i = 0; i < EdgesVM.Count; i++) {
                                        EdgesVM[i].Session = session;
                                        graph.Edges[i].ModelState = modelState;
                                    }

                                    db.Sessions.Add(session);
                                    db.ModelStates.Add(modelState);
                                    db.NodeVMs.AddRange(NodesVM);
                                    db.EdgeVMs.AddRange(EdgesVM);
                                    db.Nodes.AddRange(graph.Nodes);
                                    db.Edges.AddRange(graph.Edges);
                                    db.SaveChanges();
                                }
                            }
                            dialogService.ShowMessage("Session saved");
                        }
                    }
                }));
            }
        }

        private MyRelayCommand loadCommand;
        public MyRelayCommand LoadCommand {
            get {
                return loadCommand ?? (loadCommand = new MyRelayCommand(obj => {
                    if (dialogService.ShowLoadWindow() == true) { }
                    if (dialogService.IsConfirmed) {
                        Session session;
                        using (SearchPathContext db = new SearchPathContext()) {
                            session = db.Sessions
                                .Include(o => o.ModelState)
                                    .ThenInclude(o => o.Nodes)
                                .Include(o => o.ModelState)
                                    .ThenInclude(o => o.Edges)
                                .Include(o => o.NodeVMs)
                                .Include(o => o.EdgeVMs)
                                .FirstOrDefault(o => o.Name == dialogService.SessionName);
                        }
                        NodesVM.Clear();
                        EdgesVM.Clear();
                        graph.Nodes.Clear();
                        graph.Edges.Clear();
                        graph.MatrixReset();
                        for (int i = 0; i < session.NodeVMs.Count; i++) {
                            NodesVM.Add(session.NodeVMs.ElementAt(i));
                            graph.Nodes.Add(session.ModelState.Nodes.ElementAt(i));
                        }
                        for (int i = 0; i < session.EdgeVMs.Count; i++) {
                            EdgesVM.Add(session.EdgeVMs.ElementAt(i));
                            graph.AddEdge(session.ModelState.Edges.ElementAt(i));
                        }
                        FirstNode = null;
                        FirstSelected = null;
                        SecondSelected = null;
                        Node.Number = NodesVM.Count;
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void UpdateAllowSearch() {
            AllowSearch = (FirstSelected != null) && (SecondSelected != null);
        }
    }
}
