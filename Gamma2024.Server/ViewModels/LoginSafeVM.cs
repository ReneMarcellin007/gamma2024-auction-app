using System.ComponentModel.DataAnnotations;

namespace Gamma2024.Server.ViewModels
{
    public class LoginSafeVM
    {
        [Required]
        public string EmailOuPseudo { get; set; }

        [Required]
        public string PasswordBase64 { get; set; }
    }
}