using System.ComponentModel.DataAnnotations;

namespace Gamma2024.Server.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string? Appartement { get; set; }
        public string Rue { get; set; } = null!;
        public string Ville { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Pays { get; set; } = null!;
        [StringLength(6)]
        public string CodePostal { get; set; } = null!;
        public bool EstDomicile { get; set; }
        public string? IdApplicationUser { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<FactureLivraison> FacturesLivraisons { get; set; } = [];
    }
}
