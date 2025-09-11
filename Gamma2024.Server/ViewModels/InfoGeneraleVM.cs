namespace Gamma2024.Server.ViewModels
{
	public class InfoGeneraleVM
	{
		public string Nom { get; set; } = null!;
		public string Prenom { get; set; } = null!;
		public string Courriel { get; set; } = null!;

		public string Telephone { get; set; } = null!;
		public string Pseudo { get; set; } = null!;
		public string MotDePasse { get; set; } = null!;

		public string ConfirmMotPasse { get; set; } = null!;
	}
}
