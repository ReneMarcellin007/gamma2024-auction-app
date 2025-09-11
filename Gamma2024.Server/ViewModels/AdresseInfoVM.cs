namespace Gamma2024.Server.ViewModels
{
	public class AdresseInfoVM
	{
		public int Id { get; set; }
		public int NumeroCivique { get; set; }
		public string Rue { get; set; } = null!;
		public string Appartement { get; set; } = null!;
		public string Ville { get; set; } = null!;
		public string Province { get; set; } = null!;
		public string Pays { get; set; } = null!;
		public string CodePostal { get; set; } = null!;
	}
}
