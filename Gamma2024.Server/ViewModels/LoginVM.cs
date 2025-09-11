using System.ComponentModel.DataAnnotations;

namespace Gamma2024.Server.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string EmailOuPseudo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}