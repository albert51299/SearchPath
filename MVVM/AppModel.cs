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
        const int max = 2147483647;
        const int n = 100;
        int[,] matrix = new int[n, n];
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Graph() {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    matrix[i, j] = -1;
                }
            }
        }

        public void AddNode(Node node) {
            Nodes.Add(node);
        }

        public void AddEdge(Edge edge) {
            Edges.Add(edge);
        }

        public SearchResult SearchPath(Node start, Node end) {
            int length = 0;
            List<Node> path = new List<Node>();

            int N = Nodes.Count;
            int[,] A = new int[N, N];

            // filling adjacency matrix
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < N; j++) {
                    if (i == j) {
                        A[i, j] = 0;
                    }
                    else {
                        if (matrix[i, j] == -1) {
                            A[i, j] = max;
                        }
                        else {
                            A[i, j] = matrix[i, j];
                        }
                    }
                }
            }

            /*  Node y;
                int[] d = new int[N];
                bool[] isConst = new bool[N];
                // mark all nodes besides start node as not visited and give them max weight
                for (int i = 0; i < N; i++) {
                    d[i] = max;
                    isConst[i] = false;
                }
                // algorithm of Dijkstra
                d[firstSelected.GetIndex()] = 0;
                isConst[firstSelected.GetIndex()] = true;
                y = firstSelected;
                while (y != secondSelected) { // O(N)
                    // updating of the marks
                    for (int i = 0; i < N; i++) { // O(N)
                        if ((A[i, y.GetIndex()] != max) && (A[i, y.GetIndex()] != 0) && (isConst[i] == false)) {
                            int dx = d[i];
                            int sum = d[y.GetIndex()] + A[i, y.GetIndex()];
                            if (sum < dx) {
                                d[i] = sum;
                            }
                        }
                    }
                    // checking of exist of the path
                    //
                    // count of not marked nodes
                    int count = 0;
                    foreach (var el in isConst) { // O(N)
                        if (!el) {
                            count++;
                        }
                    }
                    // count of not marked nodes with max cost
                    int countOfMax = 0;
                    for (int j = 0; j < nodes.Count; j++) { // O(N)
                        if ((d[j] == max) && (!isConst[j])) {
                            countOfMax++;
                        }
                    }
                    if (count == countOfMax) {
                        NoPath noPath = new NoPath();
                        noPath.ShowDialog();
                        firstSelected.InvertSelected();
                        firstSelected = null;
                        secondSelected.InvertSelected();
                        secondSelected = null;
                        return;
                    }
                    //
                    // search node with minimum weight and make it visited
                    int min = max;
                    for (int i = 0; i < N; i++) { // O(N)
                        if ((d[i] < min) && (isConst[i] == false)) {
                            min = d[i];
                        }
                    }
                    for (int i = 0; i < N; i++) { // O(N)
                        if ((min == d[i]) && (isConst[i] == false)) {
                            y = nodes[i]; // O(N)
                            isConst[i] = true;
                            break;
                        }
                    }
                }
                string str = secondSelected.GetName().ToString();
                Node endNode = secondSelected;
                int dst = d[secondSelected.GetIndex()];
                // restoring of the path
                while (endNode != firstSelected) {
                    for (int i = 0; i < N; i++) {
                        if (i != endNode.GetIndex()) {
                            int dsv = d[i];
                            int avt = A[i, endNode.GetIndex()];
                            if (dsv + avt == dst) {
                                dst = dsv;
                                str += " " + nodes[i].GetName().ToString();
                                endNode = nodes[i];
                                break;
                            }
                        }
                    }
                }
                // output data
                string str1 = firstSelected.GetName().ToString();
                string str2 = secondSelected.GetName().ToString();
                string str3 = d[secondSelected.GetIndex()].ToString();
                string str4 = StringReverse(str);
                Output output = new Output(str1, str2, str3, str4);
                output.ShowDialog();
            }*/

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
