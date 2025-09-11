namespace Gamma2024.Server.ViewModels
{
    public class FactureDetailsVM
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }
        public double PrixFinal { get; set; }
        public double SousTotal { get; set; }
        public double FraisEncanteur { get; set; }
        public double TPS { get; set; }
        public double TVQ { get; set; }
        public ICollection<LotFactureVM> Lots { get; set; } = [];
        public ClientFactureVM Client { get; set; } = null!;
        public int NumeroEncan { get; set; }
    }
}
