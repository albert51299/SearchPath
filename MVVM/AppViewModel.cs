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
        public ObservableCollection<NodeVM> Nodes { get; set; }

        public AppViewModel() {
            Nodes = new ObservableCollection<NodeVM> {
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
