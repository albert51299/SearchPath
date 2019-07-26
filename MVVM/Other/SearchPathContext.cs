using Microsoft.EntityFrameworkCore;
using MVVM.Model;
using MVVM.ViewModel;
using System.Configuration;

namespace MVVM.Other {
    class SearchPathContext : DbContext {
        public DbSet<Session> Sessions { get; set; }
        public DbSet<NodeVM> NodeVMs { get; set; }
        public DbSet<EdgeVM> EdgeVMs { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Edge> Edges { get; set; }

        public SearchPathContext() {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}
