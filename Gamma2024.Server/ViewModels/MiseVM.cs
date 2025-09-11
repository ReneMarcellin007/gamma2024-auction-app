namespace Gamma2024.Server.ViewModels
{
   public class MiseVM
{
    public string UserId { get; set; } = null!;
    public int LotId { get; set; }  // Ajoutez cette propriété
    public decimal Montant { get; set; }
    public decimal? MontantMaximal { get; set; }
}
}