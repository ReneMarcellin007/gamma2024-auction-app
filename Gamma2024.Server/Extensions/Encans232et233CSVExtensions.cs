using Gamma2024.Server.Extensions.Objets;
using Gamma2024.Server.Models;
using Microsoft.IdentityModel.Tokens;

namespace Gamma2024.Server.Extensions
{
    public static class Encans232et233CSVExtensions
    {
        public static IEnumerable<Categorie> GetCategories(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(';');

                if (!string.IsNullOrEmpty(columns[1]))
                {
                    yield return new Categorie
                    {
                        Nom = columns[6]
                    };
                }

            }
        }

        public static IEnumerable<Medium> GetMediums(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(';');

                if (!string.IsNullOrEmpty(columns[1]))
                {
                    yield return new Medium
                    {
                        Type = columns[10]
                    };
                }

            }
        }

        public static IEnumerable<LotEncan> ToLot(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(';');

                if (!string.IsNullOrEmpty(columns[1]))
                {
                    var livrable = false;
                    if (columns[11].ToLower() == "oui")
                    {
                        livrable = true;
                    }

                    var prixMinVente = 0.00;
                    if (!columns[3].IsNullOrEmpty())
                    {
                        var priceStr = columns[3].Replace("$", "").Trim().Replace(" ", "").Replace(",", ".");
                        prixMinVente = double.Parse(priceStr, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    var dimensions = columns[8].Split("x");
                    var hauteur = 0.0;
                    var largeur = 0.0;

                    try
                    {
                        hauteur = double.Parse(dimensions[0].Trim());
                    }
                    catch
                    {
                        var fraction = ConvertirCaractereFraction(dimensions[0].Trim().Last());
                        var entier = double.Parse(dimensions[0].Trim().Remove(dimensions[0].Trim().Length - 1));

                        hauteur = fraction + entier;
                    }

                    try
                    {
                        largeur = double.Parse(dimensions[1].Trim());
                    }
                    catch
                    {
                        var fraction = ConvertirCaractereFraction(dimensions[1].Trim().Last());
                        var entier = double.Parse(dimensions[1].Trim().Remove(dimensions[1].Trim().Length - 1));

                        largeur = fraction + entier;
                    }

                    yield return new LotEncan
                    {
                        Lot = new Lot
                        {
                            Numero = columns[1],
                            PrixOuverture = double.Parse(columns[2].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                            PrixMinPourVente = prixMinVente,
                            ValeurEstimeMin = double.Parse(columns[4].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                            ValeurEstimeMax = double.Parse(columns[5].Replace("$", "").Trim().Replace(" ", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                            Categorie = new Categorie
                            {
                                Nom = columns[6]
                            },
                            Artiste = columns[7],
                            Hauteur = hauteur,
                            Largeur = largeur,
                            Description = columns[9],
                            Medium = new Medium
                            {
                                Type = columns[10]
                            },
                            EstLivrable = livrable,
                            Mise = 0,
                        },
                        NumeroEncan = int.Parse(columns[0])
                    };
                }

            }
        }

        public static IEnumerable<LotImages> GetLotImages(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(';');

                if (!string.IsNullOrEmpty(columns[1]))
                {
                    var images = new List<string>();

                    for (int i = 12; i < columns.Length && i < 16; i++)
                    {
                        if (!columns[i].IsNullOrEmpty())
                        {
                            images.Add(columns[i]);
                        }
                    }

                    yield return new LotImages
                    {
                        NomImages = images,
                        NumeroLot = columns[1],
                        NumeroEncan = int.Parse(columns[0])
                    };
                }

            }
        }

        private static double ConvertirCaractereFraction(char caractereFraction)
        {
            if (caractereFraction == '½')
            {
                return 0.5;
            }
            else if (caractereFraction == '¼')
            {
                return 0.25;
            }
            else if (caractereFraction == '¾')
            {
                return 0.75;
            }
            return 0;
        }
    }
}
