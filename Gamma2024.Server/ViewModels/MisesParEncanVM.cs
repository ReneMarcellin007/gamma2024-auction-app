public class MisesParEncanVM
{
    public int NumeroEncan { get; set; }
    public DateTime DateEncan { get; set; }
    public ICollection<LotMiseVM> Lots { get; set; } = new List<LotMiseVM>();
}

public class LotMiseVM
{
    public int LotId { get; set; }
    public string Numero { get; set; }
    public string Artiste { get; set; }
    public double Hauteur { get; set; }
    public double Largeur { get; set; }
    public decimal DerniereMise { get; set; }
    public decimal ValeurEstimeMin { get; set; }
    public decimal ValeurEstimeMax { get; set; }
    public decimal PrixOuverture { get; set; }
    public decimal PrixMinPourVente { get; set; }
    public bool EstPlusHautEncherisseur { get; set; }
    public bool EstVendu { get; set; }
    public string PhotoPrincipale { get; set; }
    public DateTime? DateFinDecompteLot { get; set; }
    public bool EstLivrable { get; set; }
    public string Description { get; set; }
    public int IdCategorie { get; set; }
    public string Categorie { get; set; }
    public int IdMedium { get; set; }
    public string Medium { get; set; }
    public int IdVendeur { get; set; }
    public string Vendeur { get; set; }
    public bool SeraLivree { get; set; }
} 