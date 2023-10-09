using System.ComponentModel.DataAnnotations;

namespace ProductList.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "User")]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        public string User { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember me")]
        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }
}