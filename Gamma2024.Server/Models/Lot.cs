namespace Gamma2024.Server.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ValeurEstimeMin { get; set; }
        public double ValeurEstimeMax { get; set; }
        public double PrixOuverture { get; set; }
        public double? PrixMinPourVente { get; set; } = default!;
        public DateTime DateDepot { get; set; } = DateTime.Now;
        public string Artiste { get; set; } = null!;
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public int IdCategorie { get; set; }
        public string? IdClientMise { get; set; }
        public double? Mise { get; set; }
        public bool EstVendu { get; set; }
        public bool? SeraLivree { get; set; }
        public DateTime? DateFinVente { get; set; }
        public int IdVendeur { get; set; }
        public bool EstLivrable { get; set; }
        public double Largeur { get; set; }
        public double Hauteur { get; set; }
        public Categorie Categorie { get; set; } = null!;
        public ApplicationUser? ClientMise { get; set; }
        public Vendeur Vendeur { get; set; } = null!;
        public ICollection<Photo> Photos { get; set; } = [];
        public ICollection<EncanLot> EncanLots { get; set; } = [];
        public int IdMedium { get; set; }
        public Medium Medium { get; set; } = null!;
        public int? IdFacture { get; set; }
        public Facture? Facture { get; set; } = default!;
        public DateTime? DateDebutDecompteLot { get; set; }
        public DateTime? DateFinDecompteLot { get; set; }
        public virtual ICollection<MiseAutomatique> MisesAutomatiques { get; set; } = new List<MiseAutomatique>();
    }
}
