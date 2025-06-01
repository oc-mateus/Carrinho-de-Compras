namespace CarrinhoCompras.Models
{
    public class ItemCompra
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public bool Comprado { get; set; } = false;
        public string Imagem { get; set; }

        // FK para Categoria
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // FK para o usuário dono do item (opcional, se quiser associar direto)
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
