namespace Gamma2024.Server.ViewModels
{
    public class UpdateClientInfoVM
    {
        public string Name { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Pseudonym { get; set; } = null!;
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
        public string CivicNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Apartment { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Country { get; set; } = "Canada";
        public string PostalCode { get; set; } = null!;
        public string? Photo { get; set; }
        public string? CurrentPassword { get; set; }
    }
}
