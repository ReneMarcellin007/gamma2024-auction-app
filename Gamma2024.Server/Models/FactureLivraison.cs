namespace Gamma2024.Server.Models
{
    public class FactureLivraison
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }

        private double _prixFinal;
        public double PrixFinal { get => _prixFinal; set => _prixFinal = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _sousTotal;
        public double SousTotal { get => _sousTotal; set => _sousTotal = double.Round(value, 2, MidpointRounding.ToZero); }
        private double _tPS;
        public double TPS { get => _tPS; set => _tPS = double.Round(value, 2, MidpointRounding.ToZero); }

        private double _tVQ;
        public double TVQ { get => _tVQ; set => _tVQ = double.Round(value, 2, MidpointRounding.ToZero); }

        private double? _don;
        public double? Don
        {
            get => _don;
            set => _don = value.HasValue ? Math.Round(value.Value, 2, MidpointRounding.ToZero) : null;
        }

        public int? IdAdresse { get; set; }
        public int? IdCharite { get; set; }
        public Adresse? Adresse { get; set; }
        public Charite? Charite { get; set; }
        public int IdFacture { get; set; }
        public Facture Facture { get; set; } = null!;
        public bool EstPaye { get; set; } = false;

        public void CalculerFacture()
        {
            CalculerFraisLivraison();

            TPS = SousTotal * 0.05;
            TVQ = SousTotal * 0.09975;

            PrixFinal = SousTotal + TPS + TVQ;
            if (IdCharite.HasValue)
            {
                double donCalcule = PrixFinal * 0.02;
                Don = Math.Round(donCalcule, 2, MidpointRounding.ToZero);
                PrixFinal += Don.Value;
            }
            else
            {
                Don = null;
            }

        }
        private void CalculerFraisLivraison()
        {
            var prix = new List<double>();
            foreach (var item in Facture.Lots)
            {
                if (item.Hauteur <= 20 || item.Largeur <= 20)
                {
                    prix.Add(35);
                }
                else
                {
                    prix.Add(65);
                }
            }
            if (prix.Any())
            {
                prix.OrderByDescending(x => x);
                SousTotal = prix[0];
                prix.RemoveAt(0);
                SousTotal += (prix.Sum()) / 2;
            }
            else
            {
                SousTotal = 0;
            }
        }
    }
}
