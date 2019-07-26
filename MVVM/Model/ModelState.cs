using MVVM.Other;
using System.Collections.Generic;

namespace MVVM.Model {
    class ModelState {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public ICollection<Node> Nodes { get; set; }
        public ICollection<Edge> Edges { get; set; }
    }
}
