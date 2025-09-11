namespace Gamma2024.Server.ViewModels
{
	public class ClientFactureVM
	{
		public string Nom { get; set; } = default!;
		public string AdresseLigne1 { get; set; } = default!;
		public string? AdresseLigne2 { get; set; }
		public string AdresseLigne3 { get; set; } = default!;
		public string CodePostal { get; set; } = default!;
		public string Courriel { get; set; } = default!;
		public string Telephone { get; set; } = default!;
		public string ClientId { get; set; } = default!;
	}
}
