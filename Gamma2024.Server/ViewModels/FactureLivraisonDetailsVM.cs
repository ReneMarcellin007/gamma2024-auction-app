namespace Gamma2024.Server.ViewModels
{
    public class FactureLivraisonDetailsVM
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }
        public double PrixFinal { get; set; }
        public double SousTotal { get; set; }
        public double FraisLivraison { get; set; }
        public double TPS { get; set; }
        public double TVQ { get; set; }
        public double? Don { get; set; } = 0;
        public ClientFactureVM Client { get; set; } = null!;
        public int NumeroEncan { get; set; }
        public string? Charite { get; set; } = default!;
    }
}
