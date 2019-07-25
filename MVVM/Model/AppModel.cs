using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    public class Node {
        public int Id { get; set; }
        public string Name { get; set; }
        private static int number;

        public Node() {
            Id = number;
            Name = (++number).ToString();
        }
    }

    public class Edge {
        public int Cost { get; set; }
        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
        public int Id { get; set; }
        private static int number;

        public Edge(Node first, Node second) {
            FirstNode = first;
            SecondNode = second;
            Id = ++number;
        }
    }

    public class Graph {
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
            matrix[edge.FirstNode.Id, edge.SecondNode.Id] = edge.Cost;
            matrix[edge.SecondNode.Id, edge.FirstNode.Id] = edge.Cost;
        }

        public SearchResult SearchPath(Node start, Node end) {
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
            Node y;
            int[] dist = new int[N];
            bool[] visited = new bool[N];

            // mark all nodes as not visited and give them max weight
            for (int i = 0; i < N; i++) {
                dist[i] = max;
                visited[i] = false;
            }
            dist[start.Id] = 0;
            visited[start.Id] = true;
            y = start;

            while (y != end) {
                // updating marks
                for (int i = 0; i < N; i++) { 
                    if ((A[i, y.Id] != max) && (A[i, y.Id] != 0) && (visited[i] == false)) {
                        int dx = dist[i];
                        int sum = dist[y.Id] + A[i, y.Id];
                        if (sum < dx) {
                            dist[i] = sum;
                        }
                    }
                }
                // checking exist of path
                bool pathExist = false;
                for (int j = 0; j < N; j++) {
                    if ((!visited[j]) && (dist[j] != max)) {
                        pathExist = true;
                        break;
                    }
                }
                if (!pathExist) {
                    return new SearchResult(false, 0, new List<Node>());
                    
                }
                // search node with minimum weight and make it visited
                int min = max;
                for (int i = 0; i < N; i++) { 
                    if ((dist[i] < min) && (!visited[i])) {
                        min = dist[i];
                    }
                }
                for (int i = 0; i < N; i++) {
                    if ((min == dist[i]) && (!visited[i])) {
                        y = Nodes[i];
                        visited[i] = true;
                        break;
                    }
                }
            }
            int length = dist[end.Id];
            List<Node> path = new List<Node>();
            int distST = dist[end.Id];
            path.Add(end);
            Node endNode = end;
            // restoring path
            while (endNode != start) {
                for (int i = 0; i < N; i++) {
                    if (i != endNode.Id) {
                        int distSV = dist[i];
                        int distVT = A[i, endNode.Id];
                        if (distSV + distVT == distST) {
                            distST = distSV;
                            path.Insert(0, Nodes[i]);
                            endNode = Nodes[i];
                            break;
                        }
                    }
                }
            }
            return new SearchResult(true, length, path);
        }
    }

    public class SearchResult {
        public bool PathExist { get; set; }
        public int PathLength { get; }
        public List<Node> Path { get; }

        public SearchResult(bool exist, int length, List<Node> path) {
            PathExist = exist;
            PathLength = length;
            Path = path;
        }
    }
}
