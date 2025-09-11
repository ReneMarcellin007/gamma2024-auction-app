using Gamma2024.Server.Extensions.Objets;
using Microsoft.IdentityModel.Tokens;

namespace Gamma2024.Server.Extensions
{
    public static class AcheteurEncan232Extensions
    {
        public static IEnumerable<AcheteurEncan> GetAcheteurs(this IEnumerable<string> source, int numeroEncan)
        {
            foreach (var line in source)
            {
                var columns = line.Split(';');

                if (!columns[1].IsNullOrEmpty())
                {
                    var livraison = columns[3].ToLower() == "oui";
                    yield return new AcheteurEncan
                    {
                        Pseudonyme = columns[0],
                        Lot = columns[1].Replace("lot #", ""),
                        PrixAchete = double.Parse(columns[2].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                        Livraison = livraison,
                        NumeroEncan = numeroEncan
                    };

                    if (!columns[5].IsNullOrEmpty())
                    {
                        livraison = columns[7] == "oui";
                        yield return new AcheteurEncan
                        {
                            Pseudonyme = columns[0],
                            Lot = columns[5].Replace("lot #", ""),
                            PrixAchete = double.Parse(columns[6].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                            Livraison = livraison,
                            NumeroEncan = numeroEncan
                        };

                        if (!columns[9].IsNullOrEmpty())
                        {
                            livraison = columns[11] == "oui";
                            yield return new AcheteurEncan
                            {
                                Pseudonyme = columns[0],
                                Lot = columns[9].Replace("lot #", ""),
                                PrixAchete = double.Parse(columns[10].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                                Livraison = livraison,
                                NumeroEncan = numeroEncan
                            };
                        }
                    }
                }
            }
        }
    }
}
