using System;

namespace CarrinhoCompras.Models

{
    public class ItemCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; } // alimentos, eletrodomesticos, moveis, vestuario
        public bool Comprado { get; set; }
        public string ImagemUrl { get; set; } // Caminho ou URL da imagem do produto
    }
}
