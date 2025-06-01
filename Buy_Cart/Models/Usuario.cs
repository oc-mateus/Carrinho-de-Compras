using Microsoft.AspNetCore.Identity;

namespace CarrinhoCompras.Models
{
    public class Usuario : IdentityUser
    {
        public string FullName ; 
        public string NomeCompleto { get; set; }
    }
}
