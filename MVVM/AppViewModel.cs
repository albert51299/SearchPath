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
        public ObservableCollection<NodeVM> NodesVM { get; set; }
        public bool AllowNode { get; set; }
        public bool AllowEdge { get; set; }
        public bool AllowSelect { get; set; }
        public double MouseX { get; set; }
        public double MouseY { get; set; }
        private RelayCommand canvasAction;
        public RelayCommand CanvasAction {
            get {
                return canvasAction ?? (canvasAction = new RelayCommand(obj => {
                    // some actions
                }));
            }
        }

        public AppViewModel() {
            NodesVM = new ObservableCollection<NodeVM> {
                new NodeVM("A"),
                new NodeVM("B"),
                new NodeVM("C")
            };
        }
    }

    class NodeVM {
        public string Node { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }

        public NodeVM(string node) {
            Node = node;
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
