using System.ComponentModel.DataAnnotations;

namespace CarrinhoCompras.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo {2} e no máximo {1} caracteres.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "As senhas não conferem.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmar senha é obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        public string ConfirmPassword { get; set; }
    }
}
