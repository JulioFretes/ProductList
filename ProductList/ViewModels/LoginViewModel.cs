using System.ComponentModel.DataAnnotations;

namespace ProductList.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string User { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Lembrar de mim")]
        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }
}