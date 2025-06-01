namespace CarrinhoCompras.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // FK para o usuário dono da categoria
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<ItemCompra> Itens { get; set; }
    }
}
     