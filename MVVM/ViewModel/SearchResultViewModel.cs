using MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM.ViewModel {
    class SearchResultViewModel {
        public ObservableCollection<ResultNodeVM> NodesVM { get; set; } = new ObservableCollection<ResultNodeVM>();
        public ObservableCollection<EdgeVM> EdgesVM { get; set; }
        public ObservableCollection<ResultNodeVM> PathNodesVM { get; set; } = new ObservableCollection<ResultNodeVM>();
        public bool PathExist { get; set; }
        public string Start { get; set; }
        public string Terminal { get; set; }
        public string Length { get; set; }

        public SearchResultViewModel(ObservableCollection<NodeVM> nodes, ObservableCollection<EdgeVM> edges, SearchResult searchResult) {
            PathExist = searchResult.PathExist;
            if (PathExist) {
                Start = searchResult.Path.First().Name;
                Terminal = searchResult.Path.Last().Name;
                Length = searchResult.PathLength.ToString();

                EdgesVM = edges;

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
            }
        }
    }
}
