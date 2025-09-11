namespace Gamma2024.Server.Models{
    public class Categorie
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    
    public  ICollection<Lot> Lots { get; set; } = [];
}
}