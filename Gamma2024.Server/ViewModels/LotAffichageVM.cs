namespace Gamma2024.Server.ViewModels
{
	public class LotAffichageVM
	{
		public int Id { get; set; }
		public string NumeroEncan { get; set; } = null!;
		public string Code { get; set; } = null!;
		public double PrixOuverture { get; set; }
		public double? PrixMinPourVente { get; set; }
		public double ValeurEstimeMin { get; set; }
		public double ValeurEstimeMax { get; set; }
		public string Categorie { get; set; } = null!;
		public string Artiste { get; set; } = null!;
		public double Hauteur { get; set; }
		public double Largeur { get; set; }
		public string Description { get; set; } = null!;
		public string Medium { get; set; } = null!;
		public bool EstLivrable { get; set; }
		public string Vendeur { get; set; } = null!;
		public double? Mise { get; set; }
		public bool EstVendu { get; set; }
		public DateTime? DateFinVente { get; set; }
		public DateTime DateDepot { get; set; }
		public DateTime DateCreation { get; set; }
		public string? IdClientMise { get; set; }
		public bool? SeraLivree { get; set; }
		public ICollection<PhotoVM> Photos { get; set; } = new List<PhotoVM>();
	}
}
