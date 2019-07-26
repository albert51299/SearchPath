using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM.Model {
    class Node {
        public int Id { get; set; }
        public int ModelStateId { get; set; }
        public ModelState ModelState { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public static int Number { get; set; }

        public Node() {
            Index = Number;
            Name = (++Number).ToString();
        }
    }
}
