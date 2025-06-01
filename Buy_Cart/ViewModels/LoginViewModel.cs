using System.ComponentModel.DataAnnotations;

namespace CarrinhoCompras.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Lembrar da conta")]
        public bool RememberMe { get; set; }
    }
}
