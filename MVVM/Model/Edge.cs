﻿namespace MVVM.Model {
    class Edge {
        public int Id { get; set; }
        public int ModelStateId { get; set; }
        public ModelState ModelState { get; set; }
        public int Cost { get; set; }
        public int FirstIndex { get; set; }
        public int SecondIndex { get; set; }

        public Edge() {

        }

        public Edge(int first, int second) {
            FirstIndex = first;
            SecondIndex = second;
        }
    }
}
