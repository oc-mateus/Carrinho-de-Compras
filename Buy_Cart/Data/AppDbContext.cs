using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarrinhoCompras.Models;

namespace CarrinhoCompras.Data
{
    public class AppDbContext : IdentityDbContext<Usuario>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ItemCompra> Itens { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
