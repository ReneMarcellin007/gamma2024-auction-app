using Gamma2024.Server.Models;

namespace Gamma2024.Server.ViewModels
{
    public class FactureLivraisonVM
    {
        public double PrixFinal { get; set; }
        public double PrixFinalSansDon { get; set; }
        public double SousTotal { get; set; }
        public double TPS { get; set; }
        public double TVQ { get; set; }
        public double Don { get; set; }

        public int IdFacture { get; set; }
        public Facture Facture { get; set; } = null!;

        public List<ChariteVM> Charites { get; set; } = new List<ChariteVM>();
    }
}
