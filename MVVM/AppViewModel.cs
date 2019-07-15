using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MVVM {
    class AppViewModel {
        Graph graph = new Graph();
        public ObservableCollection<NodeVM> NodesVM { get; set; } = new ObservableCollection<NodeVM>();
        public bool AllowNode { get; set; }
        public bool AllowEdge { get; set; }
        public bool AllowSelect { get; set; }
        public double MouseX { get; set; }
        public double MouseY { get; set; }
        private RelayCommand canvasAction;
        public RelayCommand CanvasAction {
            get {
                return canvasAction ?? (canvasAction = new RelayCommand(obj => {
                    if (AllowNode) {
                        Node newNode = new Node();
                        graph.AddNode(newNode);
                        NodesVM.Add(new NodeVM(newNode.Name, MouseX, MouseY));
                    }
                    if (AllowEdge) {

                    }
                    if (AllowSelect) {

                    }
                }));
            }
        }
    }

    class NodeVM {
        public double CircleDiameter { get; set; } = 25.0; // can VM know size of UI elements?
        public string Node { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }

        public NodeVM(string node, double x, double y) {
            Node = node;
            X = x;
            Y = y;
        }
    }

    class EdgeVM {
        public int Cost { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public bool Selected { get; set; }

        public EdgeVM(int cost) {
            Cost = cost;
        }
    }
}
