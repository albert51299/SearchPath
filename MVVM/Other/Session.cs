using MVVM.Model;
using MVVM.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVVM.Other {
    class Session {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<NodeVM> NodeVMs { get; set; }
        public ICollection<EdgeVM> EdgeVMs { get; set; }
        public ModelState ModelState { get; set; }

        public Session(string name) {
            Name = name;
        }
    }
}
