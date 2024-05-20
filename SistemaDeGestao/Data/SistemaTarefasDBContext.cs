using Microsoft.EntityFrameworkCore;
using SistemaDeGestao.Data.Map;
using SistemaDeGestao.Models;

namespace SistemaDeGestao.Data
{
    public class SistemaTarefasDBContext : DbContext
    {
        public SistemaTarefasDBContext(DbContextOptions<SistemaTarefasDBContext> options): base(options) 
        { 
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<TarefaModel> Tarefas { get; set; }
        public DbSet<AvaliacaoModel> Avaliacoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new TarefaMap());
            modelBuilder.ApplyConfiguration(new AvaliacaoMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
