using Microsoft.AspNetCore.Identity;

namespace Gamma2024.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Avatar { get; set; }

        public ICollection<Adresse> Adresses { get; set; } = [];
        public string StripeCustomer { get; set; } = null!;
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}