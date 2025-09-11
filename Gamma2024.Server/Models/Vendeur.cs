namespace Gamma2024.Server.Models
{
    public class Vendeur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Courriel { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public int AdresseId { get; set; }
        
        public  ICollection<Lot> Lots { get; set; } = [];
        public Adresse Adresse { get; set; } = null!;
    }
}