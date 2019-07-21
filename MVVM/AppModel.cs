using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    class Node {
        public string Name { get; set; }
        private static int number;

        public Node() {
            Name = (++number).ToString();
        }
    }

    class Edge {
        public int Cost { get; set; }
        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }

        public Edge(Node first, Node second) {
            FirstNode = first;
            SecondNode = second;
            Cost = (new Random()).Next(1, 20);
        }
    }

    class Graph {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public void AddNode(Node node) {
            Nodes.Add(node);
        }

        public void AddEdge(Edge edge) {
            Edges.Add(edge);
        }

        public SearchResult SearchPath(Node start, Node end) {
            int length = 0;
            List<Node> path = new List<Node>();



            return new SearchResult(length, path);
        }
    }

    class SearchResult {
        public int PathLength { get; }
        public List<Node> Path { get; }

        public SearchResult(int length, List<Node> path) {
            PathLength = length;
            Path = path;
        }
    }
}
