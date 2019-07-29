using MVVM.Model;
using MVVM.Other;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM.ViewModel {
    class SearchResultViewModel {
        public ObservableCollection<ResultNodeVM> NodesVM { get; set; } = new ObservableCollection<ResultNodeVM>();
        public ObservableCollection<EdgeVM> EdgesVM { get; set; }
        public ObservableCollection<ResultNodeVM> PathNodesVM { get; set; } = new ObservableCollection<ResultNodeVM>();
        public string Start { get; set; }
        public string Terminal { get; set; }
        public string Length { get; set; }

        public SearchResultViewModel(ObservableCollection<NodeVM> nodes, ObservableCollection<EdgeVM> edges, Graph graph, SearchResult searchResult) {
            Start = searchResult.Path.First().Name;
            Terminal = searchResult.Path.Last().Name;
            Length = searchResult.PathLength.ToString();

            foreach (var item in nodes) {
                var node = searchResult.Path.FirstOrDefault(o => o.Name == item.Name);
                if (node == null) {
                    NodesVM.Add(new ResultNodeVM(item.Name, item.X, item.Y, item.Selected, false));
                }
                else {
                    NodesVM.Add(new ResultNodeVM(item.Name, item.X, item.Y, item.Selected, true));
                    NodesVM.Last().Index = PathNodesVM.Count;
                    PathNodesVM.Add(NodesVM.Last());
                }
            }

            EdgesVM = edges;
            for (int i = 0; i < searchResult.Path.Count - 1; i++) {
                int firstInd = searchResult.Path[i].Index;
                int secondInd = searchResult.Path[i + 1].Index;

                var edge = graph.Edges.FirstOrDefault(o => o.FirstIndex == firstInd && o.SecondIndex == secondInd);
                int edgeInd = graph.Edges.IndexOf(edge);
                EdgesVM[edgeInd].InvertSelected();
            }
        }

        private MyRelayCommand closedCommand;
        public MyRelayCommand ClosedCommand {
            get {
                return closedCommand ?? (closedCommand = new MyRelayCommand(obj => {
                    if (EdgesVM != null) {
                        foreach (var edge in EdgesVM) {
                            edge.Selected = false;
                        }
                    }
                }));
            }
        }
    }
}
