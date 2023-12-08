using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //No meu DbSet carregará todos os Produtos salvos no banco de dados
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set;}
        public DbSet<Banner> Banners {get; set;}
    }
}