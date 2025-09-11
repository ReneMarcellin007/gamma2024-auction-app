namespace Gamma2024.Server.ViewModels
{
    public class FactureLivraisonAjoutVM
    {
        public bool? Don { get; set; }
        public int? IdCharite { get; set; }

        public int IdFacture { get; set; }
        public int? IdAdresse { get; set; }
        public string? PmId { get; set; }
        public AdresseVM? Adresse { get; set; }
    }
}
