using Gamma2024.Server.Models;

namespace Gamma2024.Server.ViewModels
{
    public class FactureAffichageMembreVM
    {
        public int Id { get; set; }
        public string IdClient { get; set; } = null!;
        public int Encan { get; set; }
        public DateTime DateAchat { get; set; }
        public ICollection<Lot> Lots { get; set; } = [];
        public bool? Livraison { get; set; }
        public int? IdFactureLivraison { get; set; } = null!;
    }
}