namespace Gamma2024.Server.Models
{
    public class Facture
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }

        private double _prixFinal;
        public double PrixFinal { get => _prixFinal; set => _prixFinal = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _sousTotal;
        public double SousTotal { get => _sousTotal; set => _sousTotal = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _prixLots;
        public double PrixLots { get => _prixLots; set => _prixLots = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _fraisEncanteur;
        public double FraisEncanteur { get => _fraisEncanteur; set => _fraisEncanteur = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _tPS;
        public double TPS { get => _tPS; set => _tPS = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _tVQ;
        public double TVQ { get => _tVQ; set => _tVQ = double.Round(value, 2, MidpointRounding.ToZero); }

        public string IdClient { get; set; } = null!;
        public ICollection<Lot> Lots { get; set; } = [];
        public ApplicationUser Client { get; set; } = null!;
        public bool estPaye { get; set; } = false;
        public int NumeroEncan { get; set; }
        public bool Livrable { get; set; } = false;
        public bool? ChoixLivraison { get; set; } = null!;

        public int? IdFactureLivraison { get; set; }
        public FactureLivraison? FactureLivraison { get; set; }

        public void CalculerFacture()
        {
            PrixLots = Lots.Select(l => l.Mise.Value).Sum();
            FraisEncanteur = PrixLots * 0.15;
            SousTotal = PrixLots + FraisEncanteur;


            TPS = SousTotal * 0.05;
            TVQ = SousTotal * 0.09975;

            PrixFinal = SousTotal + TPS + TVQ;

            if (Lots.Where(l => l.EstLivrable == false).Any())
            {
                Livrable = false;
                ChoixLivraison = false;
            }
            else
            {
                Livrable = true;
            }
        }
    }
}
