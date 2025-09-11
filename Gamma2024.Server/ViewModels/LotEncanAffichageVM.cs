using Gamma2024.Server.Models;

namespace Gamma2024.Server.ViewModels
{
	public class LotEncanAffichageVM
	{
		public int Id { get; set; }
		public int NumeroEncan { get; set; }
		public string Numero { get; set; }
		public decimal ValeurEstimeMax { get; set; }
		public decimal ValeurEstimeMin { get; set; }
		public decimal PrixOuverture { get; set; }
		public decimal PrixMinPourVente { get; set; }
		public string Artiste { get; set; }
		public double? Mise { get; set; }
		public bool? EstVendu { get; set; }
		public DateTime? DateFinVente { get; set; }
		public ICollection<Photo> Photos { get; set; }
		public string Description { get; set; }
		public bool EstLivrable { get; set; }
		public double Hauteur { get; set; }
		public double Largeur { get; set; }
		public int IdCategorie { get; set; }
		public string Categorie { get; set; }
		public int IdMedium { get; set; }
		public string Medium { get; set; }
		public string IdVendeur { get; set; }
		public string Vendeur { get; set; }
		public string IdClientMise { get; set; }
		public bool SeraLivree { get; set; }
		public int? NombreMises { get; set; }
		public DateTime? DateDebutDecompteLot { get; set; }
		public DateTime? DateFinDecompteLot { get; set; }
	}


}
