namespace Gamma2024.Server.Models
{
    public class Charite
    {
        public int Id { get; set; }
        public string NomOrganisme { get; set; } = null!;

        public List<FactureLivraison> FacturesLivraisons { get; set; } = [];
    }
}
