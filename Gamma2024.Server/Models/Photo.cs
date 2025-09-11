namespace Gamma2024.Server.Models{

    public class Photo
    {
    public int Id { get; set; }
    public string Lien { get; set; } = null!;
    public int IdLot { get; set; }
    
    public  Lot Lot { get; set; } = null!;
}

}
