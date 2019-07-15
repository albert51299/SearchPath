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
        private List<Node> nodes = new List<Node>();
        private List<Edge> edges = new List<Edge>();

        public void AddNode(Node node) {
            nodes.Add(node);
        }

        public void AddEdge(Edge edge) {
            edges.Add(edge);
        } 
    }
}
