namespace Gamma2024.Server.ViewModels
{
	public class CarteCreditInfoVM
	{
		public string Dernier4Numero { get; set; } = null!;
		public string ExpirationDate { get; set; } = null!;
		public string Marque { get; set; } = null!;
		public string? PaymentMethodId { get; set; } = null!;
	}
}
