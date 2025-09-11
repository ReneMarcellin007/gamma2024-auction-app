namespace Gamma2024.Server.Models
{
    public class EncanLot
    {
        public int IdEncan { get; set; }
        public int IdLot { get; set; }

        public Encan Encan { get; set; } = null!;
        public Lot Lot { get; set; } = null!;
    }
}
