namespace Gamma2024.Server.Models
{
    public class Encan
    {
        public int Id { get; set; }
        public int NumeroEncan { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateDebutSoireeCloture { get; set; }
        public ICollection<EncanLot> EncanLots { get; set; } = [];
        public bool EstPublie { get; set; } = false;
        public int PasLot { get; set; }
        public int PasMise { get; set; }
        public bool EstTermine { get; set; } = false;

        public bool EstEnSoireeCloture()
        {
            var maintenant = DateTime.Now;
            return maintenant >= DateDebutSoireeCloture && 
                   EncanLots.Any(el => !el.Lot.EstVendu && el.Lot.DateFinDecompteLot > maintenant);
        }
    }
}
