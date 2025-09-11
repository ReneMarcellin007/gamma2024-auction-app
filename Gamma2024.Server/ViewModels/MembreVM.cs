namespace Gamma2024.Server.ViewModels
{
	public class MembreVM
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string Telephone { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Avatar { get; set; } = null!;
		public bool EstBloque { get; set; }
		public List<CarteCreditInfoVM> CartesCredit { get; set; } = null!;
		public List<AdresseInfoVM> Adresses { get; set; } = null!;

	}
}
