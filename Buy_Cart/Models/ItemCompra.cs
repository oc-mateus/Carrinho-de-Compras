using System;

namespace CarrinhoComprasPDF.Models
{
    public class ItemCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public bool Comprado { get; set; }
    }
}
