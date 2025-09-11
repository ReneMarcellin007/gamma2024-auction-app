namespace Gamma2024.Server.ViewModels
{
    public class EncanAffichageAdminVM
    {
        public int Id { get; set; }
        public int NumeroEncan { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateDebutSoireeCloture { get; set; }
        public bool EstPublie { get; set; }
        public int NbLots { get; set; }
    }
}
