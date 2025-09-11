namespace Gamma2024.Server.ViewModels
{
    public class LotDetailsVM
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ValeurEstimeMin { get; set; }
        public double ValeurEstimeMax { get; set; }
        public string Artiste { get; set; } = null!;
        public double? Mise { get; set; }
        public bool EstVendu { get; set; }
        public bool EstLivrable { get; set; }
        public double Largeur { get; set; }
        public double Hauteur { get; set; }
        public string Categorie { get; set; } = null!;
        public IEnumerable<string> Photos { get; set; } = [];
        public string Medium { get; set; } = null!;
    }
}
