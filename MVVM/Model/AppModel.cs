﻿using System.Collections.Generic;

namespace MVVM.Model {
    class Graph {
        const int max = 2147483647;
        const int n = 100;
        int[,] matrix = new int[n, n];
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Graph() {
            MatrixReset();
        }

        public void AddEdge(Edge edge) {
            Edges.Add(edge);
            matrix[edge.FirstIndex, edge.SecondIndex] = edge.Cost;
            matrix[edge.SecondIndex, edge.FirstIndex] = edge.Cost;
        }

        public void MatrixReset() {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    matrix[i, j] = -1;
                }
            }
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
            dist[start.Index] = 0;
            visited[start.Index] = true;
            y = start;

            while (y != end) {
                // updating marks
                for (int i = 0; i < N; i++) {
                    if ((A[i, y.Index] != max) && (A[i, y.Index] != 0) && (visited[i] == false)) {
                        int dx = dist[i];
                        int sum = dist[y.Index] + A[i, y.Index];
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
                    return new SearchResult(false);
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
            int length = dist[end.Index];
            List<Node> path = new List<Node>();
            int distST = dist[end.Index];
            path.Add(end);
            Node endNode = end;
            // restoring path
            while (endNode != start) {
                for (int i = 0; i < N; i++) {
                    if (i != endNode.Index) {
                        int distSV = dist[i];
                        int distVT = A[i, endNode.Index];
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

    class SearchResult {
        public bool PathExist { get; set; }
        public int PathLength { get; }
        public List<Node> Path { get; }

        public SearchResult(bool exist) {
            PathExist = exist;
        }

        public SearchResult(bool exist, int length, List<Node> path) {
            PathExist = exist;
            PathLength = length;
            Path = path;
        }
    }
}
