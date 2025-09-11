namespace Gamma2024.Server.Models
{
    public class Medium
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    }
}
