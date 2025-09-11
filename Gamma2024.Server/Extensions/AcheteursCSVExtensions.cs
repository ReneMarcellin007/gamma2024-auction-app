using Gamma2024.Server.Models;

namespace Gamma2024.Server.Extensions
{
    public static class AcheteursCSVExtensions
    {
        public static IEnumerable<ApplicationUser> ToApplicationUser(this IEnumerable<string> source)
        {
            var listeUsersCourrielActive = new List<string> { "SimCar", "HugoLam", "MelB", "HouraAme", "MCLe" };
            foreach (var line in source)
            {
                var columns = line.Split(';');

                var adresse = columns[3].Split(",").Select(a => a.Trim()).ToList();

                var numCiviqueRue = adresse[0].Split(" ", 2).Select(a => a.Trim()).ToList();

                yield return new ApplicationUser
                {
                    Name = columns[0],
                    FirstName = columns[1],
                    UserName = columns[2],
                    NormalizedUserName = columns[2].ToUpper(),
                    Adresses = new Adresse[]
                    {
                    new() {
                        Numero = int.Parse(numCiviqueRue[0]),
                        Rue = numCiviqueRue[1],
                        Ville = adresse[1],
                        Province = adresse[2],
                        Pays = "Canada",
                        CodePostal = adresse[3].Replace(" ", ""),
                        EstDomicile = true
                    },
                    },
                    PhoneNumber = columns[4],
                    Email = columns[5],
                    NormalizedEmail = columns[5].ToUpper(),
                    EmailConfirmed = listeUsersCourrielActive.Contains(columns[2]),
                    Avatar = "/Avatars/Defaut.png",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

            }
        }
    }
}
