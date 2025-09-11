namespace Gamma2024.Server.Extensions.Objets
{
    public class AcheteurEncan
    {
        public string Lot { get; set; } = default!;
        public double PrixAchete { get; set; }
        public bool Livraison { get; set; }
        public string Pseudonyme { get; set; } = default!;
        public int NumeroEncan { get; set; }
    }
}
