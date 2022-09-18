using System.ComponentModel.DataAnnotations;

namespace App.DTO
{
    public class LoginViewModel
    {

        [Required(ErrorMessage ="This fild is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This fild is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
