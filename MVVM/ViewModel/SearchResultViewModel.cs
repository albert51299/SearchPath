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

        public SearchResultViewModel(ObservableCollection<NodeVM> nodes, ObservableCollection<EdgeVM> edges, SearchResult searchResult) {
            Start = searchResult.Path.First().Name;
            Terminal = searchResult.Path.Last().Name;
            Length = searchResult.PathLength.ToString();

            foreach (var item in nodes) {
                var node = searchResult.Path.FirstOrDefault(o => o.Index == item.Index);
                if (node == null) {
                    NodesVM.Add(new ResultNodeVM(item.Index, item.Name, item.X, item.Y, item.Selected, false));
                }
                else {
                    NodesVM.Add(new ResultNodeVM(item.Index, item.Name, item.X, item.Y, item.Selected, true));
                }
            }

            for (int i = 0; i < searchResult.Path.Count; i++) {
                ResultNodeVM node = NodesVM.FirstOrDefault(o => o.Index == searchResult.Path[i].Index);
                node.PathIndex = i;
                PathNodesVM.Add(node);
            }

            EdgesVM = edges;
            for (int i = 0; i < searchResult.Path.Count - 1; i++) {
                int firstInd = searchResult.Path[i].Index;
                int secondInd = searchResult.Path[i + 1].Index;

                EdgeVM edge = EdgesVM.FirstOrDefault(o => o.FirstIndex == firstInd && o.SecondIndex == secondInd);
                if (edge == null) {
                    edge = EdgesVM.FirstOrDefault(o => o.FirstIndex == secondInd && o.SecondIndex == firstInd);
                }
                edge.InvertSelected();
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
