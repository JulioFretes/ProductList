using System.ComponentModel.DataAnnotations;

namespace ProductList.ViewModels
{
    public class RegisterUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User name")]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        [StringLength(11, ErrorMessage = "The {0} field must have {1} digits.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        [MaxLength(16, ErrorMessage = "The maximum length of the {0} field is {1} characters.")]
        [MinLength(6, ErrorMessage = "The minimum length of the {0} field is {1} characters.")]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} field is mandatory.")]
        [MaxLength(16, ErrorMessage = "The maximum length of the {0} field is {1} characters.")]
        [MinLength(6, ErrorMessage = "The minimum length of the {0} field is {1} characters.")]
        [Compare(nameof(Password), ErrorMessage = "Password confirmation does not match the password.")]
        public string ConfPassword { get; set; }
    }
}