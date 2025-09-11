using Gamma2024.Server.Data;
using Gamma2024.Server.Hub;
using Gamma2024.Server.Models;
using Gamma2024.Server.Validations;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Gamma2024.Server.Services
{
    public class LotService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<LotMiseHub, ILotMiseHub> _hubContext;
        private readonly ILogger<LotService> _logger;
        private readonly EncanService _encanService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NotificationService _notificationService;

        public LotService(ApplicationDbContext context, IWebHostEnvironment environment, IHubContext<LotMiseHub,
            ILotMiseHub> hubContext, ILogger<LotService> logger, EncanService encanService,
            IHttpClientFactory httpClientFactory, NotificationService notificationService)
        {
            _context = context;
            _environment = environment;
            _hubContext = hubContext;
            _logger = logger;
            _encanService = encanService;
            _httpClientFactory = httpClientFactory;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<LotAffichageVM>> ObtenirTousLots()
        {
            return await _context.Lots
                .Include(l => l.Photos)
                .Include(l => l.Categorie)
                .Include(l => l.Medium)
                .Include(l => l.Vendeur)
                .Include(l => l.EncanLots)
                .ThenInclude(el => el.Encan)
                .Select(l => new LotAffichageVM
                {
                    Id = l.Id,
                    NumeroEncan = l.EncanLots.FirstOrDefault().Encan.NumeroEncan.ToString(),
                    Code = l.Numero,
                    PrixOuverture = l.PrixOuverture,
                    PrixMinPourVente = l.PrixMinPourVente,
                    ValeurEstimeMin = l.ValeurEstimeMin,
                    ValeurEstimeMax = l.ValeurEstimeMax,
                    Categorie = l.Categorie.Nom,
                    Artiste = l.Artiste,
                    Hauteur = l.Hauteur,
                    Largeur = l.Largeur,
                    Description = l.Description,
                    Medium = l.Medium.Type,
                    EstLivrable = l.EstLivrable,
                    Vendeur = $"{l.Vendeur.Prenom} {l.Vendeur.Nom}",
                    Mise = l.Mise,
                    EstVendu = l.EstVendu,
                    DateFinVente = l.DateFinVente,
                    DateDepot = l.DateDepot,
                    DateCreation = l.DateCreation,
                    IdClientMise = l.IdClientMise,
                    SeraLivree = l.SeraLivree,
                    Photos = l.Photos.Select(p => new PhotoVM
                    {
                        Id = p.Id,
                        Url = p.Lien
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<LotModificationVM> ObtenirLot(int id)
        {
            var lot = await _context.Lots
                .Include(l => l.Categorie)
                .Include(l => l.Photos)
                .Include(l => l.Vendeur)
                .Include(l => l.Medium)
                .Include(l => l.EncanLots)
                    .ThenInclude(el => el.Encan)
                .Include(l => l.ClientMise)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lot == null)
            {
                return null;
            }

            var encanLot = lot.EncanLots.FirstOrDefault();

            return new LotModificationVM
            {
                Id = lot.Id,
                Numero = lot.Numero,
                Description = lot.Description,
                ValeurEstimeMin = lot.ValeurEstimeMin,
                ValeurEstimeMax = lot.ValeurEstimeMax,
                PrixOuverture = lot.PrixOuverture,
                PrixMinPourVente = lot.PrixMinPourVente,
                DateDepot = lot.DateDepot,
                Artiste = lot.Artiste,
                DateCreation = lot.DateCreation,
                IdCategorie = lot.IdCategorie,
                Categorie = lot.Categorie?.Nom,
                IdVendeur = lot.IdVendeur,
                Vendeur = $"{lot.Vendeur?.Prenom} {lot.Vendeur?.Nom}",
                EstLivrable = lot.EstLivrable,
                Hauteur = lot.Hauteur,
                Largeur = lot.Largeur,
                IdMedium = lot.IdMedium,
                Medium = lot.Medium?.Type,
                IdEncanModifie = encanLot?.IdEncan,
                NumeroEncan = encanLot?.Encan?.NumeroEncan.ToString(),
                PhotosModifie = lot.Photos.Select(p => new PhotoVM
                {
                    Id = p.Id,
                    Url = p.Lien
                }).ToList(),
                IdClientMise = lot.IdClientMise,
                NomClientMise = $"{lot.ClientMise?.FirstName} {lot.ClientMise?.Name}",
                Mise = lot.Mise,
                EstVendu = lot.EstVendu,
                SeraLivree = lot.SeraLivree,
                DateFinVente = lot.DateFinVente
            };
        }

        public async Task<(bool Success, string Message, LotAffichageVM Lot)> CreerLot(LotCreationVM lotVM)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var (isValid, errorMessage) = await LotValidation.ValidateLot(lotVM, _context);
                if (!isValid)
                {
                    return (false, errorMessage, null);
                }

                var encan = await _context.Encans
                    .Include(e => e.EncanLots)
                        .ThenInclude(el => el.Lot)
                    .FirstOrDefaultAsync(e => e.Id == lotVM.IdEncan);

                if (encan == null)
                {
                    return (false, "L'encan spécifié n'existe pas", null);
                }


                // Définir les dates du lot
                var dateDebutDecompte = encan.DateDebutSoireeCloture;
                DateTime dateFinDecompte;

                // Chercher le dernier lot de l'encan
                var dernierLot = encan.EncanLots
                    .Select(el => el.Lot)
                    .Where(l => l.DateFinDecompteLot.HasValue)
                    .OrderByDescending(l => l.DateFinDecompteLot)
                    .FirstOrDefault();

                if (dernierLot != null)
                {
                    // Ajouter le pas lot après la dernière date de fin
                    dateFinDecompte = dernierLot.DateFinDecompteLot.Value.AddSeconds(encan.PasLot);
                }
                else
                {
                    // Premier lot : date début + 30 secondes
                    dateFinDecompte = dateDebutDecompte.AddSeconds(30);
                }

                var lotsVendeurEncan = _context.EncanLots
                                            .Include(el => el.Lot)
                                            .Where(el => el.IdEncan == lotVM.IdEncan)
                                            .Select(el => el.Lot)
                                            .Where(el => el.IdVendeur == lotVM.IdVendeur)
                                            .ToList();

                char[] az = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();

                if (lotsVendeurEncan.Count() > 0)
                {
                    var dernierLotVendeur = lotsVendeurEncan.OrderBy(el => el.Numero).Last().Numero;
                    var num = 0;

                    try
                    {
                        num = int.Parse(dernierLotVendeur);
                        lotVM.Numero = num + "a";
                    }
                    catch
                    {
                        var dernLettre = dernierLotVendeur.Last();
                        string nb = Regex.Replace(dernierLotVendeur, "[A-Za-z ]", "");
                        var indexArray = Array.IndexOf(az, dernLettre);

                        lotVM.Numero = nb + az[indexArray + 1];
                    }

                }
                else
                {
                    var ordre = _context.EncanLots
                                            .Include(el => el.Lot)
                                            .Where(el => el.IdEncan == lotVM.IdEncan)
                                            .Select(el => el.Lot)
                                            .AsEnumerable();

                    if (ordre.Count() > 0)
                    {
                        var dernierLotEncan = ordre.OrderBy(x =>
                                 {
                                     var nb = Regex.Replace(x.Numero, "[A-Za-z ]", "");

                                     return int.Parse(nb);
                                 }).Last().Numero;
                        string nbDern = Regex.Replace(dernierLotEncan, "[A-Za-z ]", "");
                        lotVM.Numero = (int.Parse(nbDern) + 1).ToString();
                    }
                    else
                    {
                        lotVM.Numero = "1";
                    }
                }


                var lot = new Lot
                {
                    Numero = lotVM.Numero,
                    Description = lotVM.Description,
                    ValeurEstimeMin = lotVM.ValeurEstimeMin,
                    ValeurEstimeMax = lotVM.ValeurEstimeMax,
                    PrixOuverture = lotVM.PrixOuverture,
                    PrixMinPourVente = lotVM.PrixMinPourVente,
                    Artiste = lotVM.Artiste,
                    IdCategorie = lotVM.IdCategorie,
                    IdVendeur = lotVM.IdVendeur,
                    EstLivrable = lotVM.EstLivrable,
                    Largeur = lotVM.Largeur,
                    Hauteur = lotVM.Hauteur,
                    IdMedium = lotVM.IdMedium,
                    DateDebutDecompteLot = dateDebutDecompte,
                    DateFinDecompteLot = dateFinDecompte
                };

                _context.Lots.Add(lot);
                await _context.SaveChangesAsync();

                // Associer le lot à l'encan spécifié
                var encanLot = new EncanLot
                {
                    IdEncan = encan.Id,
                    IdLot = lot.Id
                };

                _context.EncanLots.Add(encanLot);

                // Gérer l'enregistrement des photos
                if (lotVM.Photos != null && lotVM.Photos.Any())
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "Images", $"ImagesEncan{encan.NumeroEncan}");
                    Directory.CreateDirectory(uploadPath);

                    foreach (var photo in lotVM.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            string fileName = $"{lot.Numero}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                            string filePath = Path.Combine(uploadPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }

                            lot.Photos.Add(new Photo { Lien = $"/Images/ImagesEncan{encan.NumeroEncan}/{fileName}" });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var lotModification = await ObtenirLot(lot.Id);
                var lotAffichage = ConvertirEnLotAffichageVM(lotModification);
                return (true, "Lot créé avec succès", lotAffichage);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Erreur lors de la création du lot : {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, LotAffichageVM Lot)> ModifierLot(int id, LotModificationVM lotVM)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var (isValid, errorMessage) = await LotValidation.ValidateLot(lotVM, _context);
                if (!isValid)
                {
                    return (false, errorMessage, null);
                }

                var lot = await _context.Lots
                    .Include(l => l.Photos)
                    .Include(l => l.EncanLots)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lot.IdVendeur != lotVM.IdVendeur || lot.EncanLots.FirstOrDefault(el => el.IdEncan == lotVM.IdEncan) == null)
                {
                    var lotsVendeurEncan = _context.EncanLots
                                            .Include(el => el.Lot)
                                            .Where(el => el.IdEncan == lotVM.IdEncanModifie)
                                            .Select(el => el.Lot)
                                            .Where(el => el.IdVendeur == lotVM.IdVendeur)
                                            .ToList();

                    char[] az = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();

                    if (lotsVendeurEncan.Count() > 0)
                    {
                        var dernierLotVendeur = lotsVendeurEncan.OrderBy(el => el.Numero).Last().Numero;
                        var num = 0;

                        try
                        {
                            num = int.Parse(dernierLotVendeur);
                            lotVM.Numero = num + "a";
                        }
                        catch
                        {
                            var dernLettre = dernierLotVendeur.Last();
                            string nb = Regex.Replace(dernierLotVendeur, "[A-Za-z ]", "");
                            var indexArray = Array.IndexOf(az, dernLettre);

                            lotVM.Numero = nb + az[indexArray + 1];
                        }

                    }
                    else
                    {
                        var ordre = _context.EncanLots
                                                .Include(el => el.Lot)
                                                .Where(el => el.IdEncan == lotVM.IdEncanModifie)
                                                .Select(el => el.Lot)
                                                .ToList();

                        if (ordre.Count() > 0)
                        {
                            var dernierLotEncan = ordre.OrderBy(x =>
                            {
                                var nb = Regex.Replace(x.Numero, "[A-Za-z ]", "");

                                return int.Parse(nb);
                            }).Last().Numero;
                            string nbDern = Regex.Replace(dernierLotEncan, "[A-Za-z ]", "");
                            lotVM.Numero = (int.Parse(nbDern) + 1).ToString();
                        }
                        else
                        {
                            lotVM.Numero = "1";
                        }
                    }
                }

                if (lot == null)
                {
                    return (false, "Lot non trouvé", null);
                }

                // Mettre à jour les propriétés du lot
                lot.Numero = lotVM.Numero;
                lot.Description = lotVM.Description;
                lot.ValeurEstimeMin = lotVM.ValeurEstimeMin;
                lot.ValeurEstimeMax = lotVM.ValeurEstimeMax;
                lot.PrixOuverture = lotVM.PrixOuverture;
                lot.PrixMinPourVente = lotVM.PrixMinPourVente;
                lot.Artiste = lotVM.Artiste;
                lot.IdCategorie = lotVM.IdCategorie;
                lot.IdVendeur = lotVM.IdVendeur;
                lot.EstLivrable = lotVM.EstLivrable;
                lot.Largeur = lotVM.Largeur;
                lot.Hauteur = lotVM.Hauteur;
                lot.IdMedium = lotVM.IdMedium;

                // Mettre à jour l'encan associé si nécessaire
                if (lotVM.IdEncanModifie.HasValue)
                {
                    var encanLot = lot.EncanLots.FirstOrDefault();
                    if (encanLot == null || encanLot.IdEncan != lotVM.IdEncanModifie.Value)
                    {
                        if (encanLot != null)
                        {
                            _context.EncanLots.Remove(encanLot);
                        }
                        lot.EncanLots.Add(new EncanLot { IdEncan = lotVM.IdEncanModifie.Value, IdLot = lot.Id });
                    }
                }

                // Gérer l'ajout de nouvelles photos
                if (lotVM.NouvellesPhotos != null && lotVM.NouvellesPhotos.Any())
                {
                    var encan = await _context.EncanLots
                        .Where(el => el.IdLot == lot.Id)
                        .Select(el => el.Encan)
                        .FirstOrDefaultAsync();

                    if (encan != null)
                    {
                        string uploadPath = Path.Combine(_environment.WebRootPath, "Images", $"ImagesEncan{encan.NumeroEncan}");
                        Directory.CreateDirectory(uploadPath);

                        foreach (var photo in lotVM.NouvellesPhotos)
                        {
                            if (photo.Length > 0)
                            {
                                string fileName = $"{lot.Numero}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                                string filePath = Path.Combine(uploadPath, fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await photo.CopyToAsync(stream);
                                }

                                lot.Photos.Add(new Photo { Lien = $"/Images/ImagesEncan{encan.NumeroEncan}/{fileName}" });
                            }
                        }
                    }
                }

                // Gérer la suppression des photos
                if (lotVM.PhotosASupprimer != null && lotVM.PhotosASupprimer.Any())
                {
                    foreach (var photoId in lotVM.PhotosASupprimer)
                    {
                        var photo = lot.Photos.FirstOrDefault(p => p.Id == photoId);
                        if (photo != null)
                        {
                            string fullPath = Path.Combine(_environment.WebRootPath, photo.Lien.TrimStart('/'));
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                            lot.Photos.Remove(photo);
                            _context.Photos.Remove(photo);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var lotModification = await ObtenirLot(lot.Id);
                var lotAffichage = ConvertirEnLotAffichageVM(lotModification);
                return (true, "Lot modifié avec succès", lotAffichage);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Erreur lors de la modification du lot : {ex.Message}", null);
            }
        }

        public ICollection<LotEncanAffichageVM> ChercherTousLotsRecherche()
        {
            try
            {
                var lots = _context.Lots
                    .Include(l => l.EncanLots)
                    .Include(l => l.Photos)
                    .Select(l => new LotEncanAffichageVM()
                    {
                        Id = l.Id,
                        NumeroEncan = _context.Encans.Where(e => e.Id == (_context.EncanLots.Where(e => e.IdLot == l.Id).Max(e => e.IdEncan))).Single().NumeroEncan,
                        Numero = l.Numero,
                        ValeurEstimeMax = (decimal)l.ValeurEstimeMax,
                        ValeurEstimeMin = (decimal)l.ValeurEstimeMin,
                        Artiste = l.Artiste,
                        IdCategorie = l.IdCategorie,
                        Categorie = l.Categorie.Nom,
                        Mise = l.Mise,
                        EstVendu = l.EstVendu,
                        DateFinVente = l.DateFinVente,
                        Photos = l.Photos,
                        Description = l.Description,
                        EstLivrable = l.EstLivrable,
                        Hauteur = l.Hauteur,
                        Largeur = l.Largeur,
                        IdMedium = l.IdMedium,
                        Medium = l.Medium.Type,
                    })
                    .ToList();

                Console.WriteLine($"Lots trouvés dans le service: {lots.Count}");
                return lots;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans le service: {ex.Message}");
                throw;
            }
        }

        public ICollection<LotEncanAffichageVM> ChercherTousLotsParEncan(int idEncan)
        {
            try
            {
                var lots = _context.Lots
                    .Include(l => l.EncanLots)
                    .Include(l => l.Photos)
                    .Include(l => l.MisesAutomatiques)
                    .Where(l => l.EncanLots.Any(el => el.IdEncan == idEncan))
                    .Select(l => new LotEncanAffichageVM()
                    {
                        Id = l.Id,
                        Numero = l.Numero,
                        ValeurEstimeMax = (decimal)l.ValeurEstimeMax,
                        ValeurEstimeMin = (decimal)l.ValeurEstimeMin,
                        PrixOuverture = (decimal)l.PrixOuverture,
                        PrixMinPourVente = (decimal)l.PrixMinPourVente,
                        Artiste = l.Artiste,
                        Mise = l.Mise,
                        EstVendu = l.EstVendu,
                        DateFinVente = l.DateFinVente,
                        Photos = l.Photos.ToList(),
                        Description = l.Description,
                        EstLivrable = l.EstLivrable,
                        Hauteur = l.Hauteur,
                        Largeur = l.Largeur,
                        IdCategorie = l.IdCategorie,
                        Categorie = l.Categorie.Nom,
                        IdMedium = l.IdMedium,
                        Medium = l.Medium.Type,
                        IdVendeur = l.IdVendeur.ToString(),
                        Vendeur = $"{l.Vendeur.Prenom} {l.Vendeur.Nom}",
                        IdClientMise = l.IdClientMise ?? "",
                        SeraLivree = l.SeraLivree ?? false,
                        NombreMises = l.MisesAutomatiques.Count(),
                        DateDebutDecompteLot = l.DateDebutDecompteLot,
                        DateFinDecompteLot = l.DateFinDecompteLot
                    })
                    .ToList();

                return lots;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans le service: {ex.Message}");
                throw;
            }
        }


        public LotDetailsVM ChercherDetailsLotParId(int idLot)
        {
            var lot = _context.Lots
                .Include(l => l.Photos)
                .Include(l => l.Categorie)
                .Include(l => l.Medium)
                .FirstOrDefault(l => l.Id == idLot);
            if (lot != null)
            {
                var retourLot = new LotDetailsVM()
                {
                    Id = lot.Id,
                    Numero = lot.Numero,
                    ValeurEstimeMax = lot.ValeurEstimeMax,
                    ValeurEstimeMin = lot.ValeurEstimeMin,
                    Artiste = lot.Artiste,
                    Mise = lot.Mise,
                    EstVendu = lot.EstVendu,
                    Photos = lot.Photos.Select(p => p.Lien),
                    Description = lot.Description,
                    EstLivrable = lot.EstLivrable,
                    Hauteur = lot.Hauteur,
                    Largeur = lot.Largeur,
                    Medium = lot.Medium.Type,
                    Categorie = lot.Categorie.Nom
                };
                return retourLot;
            }
            return null;
        }

        public async Task<(bool Success, string Message)> SupprimerLot(int id)
        {
            try
            {
                var lot = await _context.Lots
                    .Include(l => l.Photos)
                    .Include(l => l.EncanLots)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lot == null)
                {
                    return (false, "Lot non trouvé");
                }


                // Supprimer les EncanLots associés
                foreach (var encanLot in lot.EncanLots)
                {
                    _context.EncanLots.Remove(encanLot);
                }

                // Supprimer les photos associées
                foreach (var photo in lot.Photos)
                {
                    string fullPath = Path.Combine(_environment.WebRootPath, photo.Lien.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    _context.Photos.Remove(photo);
                }

                _context.Lots.Remove(lot);

                await _context.SaveChangesAsync();

                return (true, "Lot supprimé avec succès");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du lot: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Détails de l'erreur interne: " + ex.InnerException.Message);
                }
                return (false, "Erreur lors de la suppression du lot : " + ex.Message);
            }
        }

        // Ajoutez cette nouvelle méthode pour convertir LotModificationVM en LotAffichageVM
        private LotAffichageVM ConvertirEnLotAffichageVM(LotModificationVM lotModification)
        {
            return new LotAffichageVM
            {
                Id = lotModification.Id,
                NumeroEncan = lotModification.NumeroEncan,
                Code = lotModification.Numero,
                PrixOuverture = lotModification.PrixOuverture,
                PrixMinPourVente = lotModification.PrixMinPourVente,
                ValeurEstimeMin = lotModification.ValeurEstimeMin,
                ValeurEstimeMax = lotModification.ValeurEstimeMax,
                Categorie = lotModification.Categorie,
                Artiste = lotModification.Artiste,
                Hauteur = lotModification.Hauteur,
                Largeur = lotModification.Largeur,
                Description = lotModification.Description,
                Medium = lotModification.Medium,
                EstLivrable = lotModification.EstLivrable,
                Vendeur = lotModification.Vendeur,
                Mise = lotModification.Mise,
                EstVendu = lotModification.EstVendu,
                DateFinVente = lotModification.DateFinVente,
                DateDepot = lotModification.DateDepot,
                DateCreation = lotModification.DateCreation,
                IdClientMise = lotModification.IdClientMise,
                SeraLivree = lotModification.SeraLivree,
                Photos = lotModification.PhotosModifie
            };
        }

        public async Task<(bool success, string message)> PlacerMise(MiseVM mise)
        {
            try
            {
                // Vérification de l'utilisateur
                var user = await _context.Users.FindAsync(mise.UserId);
                if (user == null)
                {
                    return (false, "Utilisateur non trouvé");
                }

                if (user.LockoutEnd != null)
                {
                    return (false, "Votre compte est temporairement bloqué");
                }

                // Vérification du lot
                var lot = await _context.Lots
                    .Include(l => l.MisesAutomatiques)
                    .Include(l => l.EncanLots)
                        .ThenInclude(el => el.Encan)
                    .FirstOrDefaultAsync(l => l.Id == mise.LotId);

                if (lot == null)
                {
                    return (false, "Lot non trouvé");
                }

                // Vérification si le lot est déjà vendu
                if (lot.EstVendu)
                {
                    return (false, "Ce lot est déjà vendu");
                }

                // NOUVELLE VÉRIFICATION : Date de fin du décompte
                if (lot.DateFinDecompteLot.HasValue && DateTime.Now > lot.DateFinDecompteLot.Value)
                {
                    return (false, "Le temps pour miser sur ce lot est écoulé");
                }

                // Vérification si l'utilisateur est déjà le plus haut enchérisseur
                if (lot.IdClientMise == mise.UserId)
                {
                    return (false, "Vous êtes déjà le plus haut enchérisseur");
                }

                // Vérification de l'encan
                var encan = lot.EncanLots.FirstOrDefault()?.Encan;
                if (encan == null)
                {
                    return (false, "Encan non trouvé");
                }

                var maintenant = DateTime.Now;
                if (maintenant < encan.DateDebut)
                {
                    return (false, "L'encan n'a pas encore commencé");
                }

                // Vérification du pas d'enchère
                if (!EstMiseValide(lot, lot.Mise.HasValue ? (decimal)lot.Mise.Value : 0m, mise.Montant, mise.MontantMaximal ?? 0, mise.MontantMaximal.HasValue))
                {
                    string message = mise.MontantMaximal.HasValue
                        ? "La mise automatique doit être d'au moins 2 pas d'enchère"
                        : "La mise ne respecte pas le pas d'enchère";
                    return (false, message);
                }

                // Création de l'entrée dans l'historique des mises
                var nouvelleMise = new MiseAutomatique
                {
                    LotId = mise.LotId,
                    UserId = mise.UserId,
                    Montant = mise.Montant,
                    DateMise = DateTime.UtcNow,
                    EstMiseAutomatique = mise.MontantMaximal.HasValue,
                    MontantMaximal = mise.MontantMaximal
                };

                _context.MiseAutomatiques.Add(nouvelleMise);

                // Mise à jour du lot
                lot.Mise = Convert.ToDouble(mise.Montant);
                lot.IdClientMise = mise.UserId;
                _context.Lots.Update(lot);

                await _context.SaveChangesAsync();

                // Traitement des mises automatiques
                await TraiterMisesAutomatiques(lot, encan);

                // Notification
                var derniereMise = await _context.MiseAutomatiques
                    .Where(m => m.LotId == lot.Id)
                    .OrderByDescending(m => m.DateMise)
                    .FirstOrDefaultAsync();

                //envoyer une notification 
                var avantDerniereMise = await _context.MiseAutomatiques
                    .Where(m => m.LotId == lot.Id)
                    .OrderByDescending(m => m.DateMise)
                    .Take(2)
                    .ToListAsync();

                if (avantDerniereMise.LastOrDefault() != null && avantDerniereMise.Count == 2)
                {
                    var lelot = await _context.Lots.FindAsync(mise.LotId);
                    var messageMise = $"Nouvelle mise sur le lot {lelot.Numero}";
                    await _notificationService.SendBidNotification(avantDerniereMise.LastOrDefault().UserId, messageMise);
                }

                await _hubContext.Clients.All.ReceiveNewBid(new
                {
                    idLot = lot.Id,
                    montant = derniereMise.Montant,
                    userId = derniereMise.UserId,
                    userLastBid = new
                    {
                        userId = mise.UserId,
                        montant = mise.Montant
                    },
                    nombreMises = await _context.MiseAutomatiques.CountAsync(m => m.LotId == lot.Id),
                    timestamp = DateTime.Now
                });

                // Vérifier si le lot est en soirée de clôture
                if (lot != null)
                {
                    var encanLot = lot.EncanLots.FirstOrDefault()?.Encan;
                    if (encanLot?.EstEnSoireeCloture() == true)
                    {
                        if (lot.DateFinDecompteLot.HasValue)
                        {
                            var tempsRestant = (lot.DateFinDecompteLot.Value - DateTime.Now).TotalSeconds;
                            if (tempsRestant < 60)
                            {
                                lot.DateFinDecompteLot = lot.DateFinDecompteLot.Value.AddSeconds(encanLot.PasMise);
                                await _context.SaveChangesAsync();

                                // Notifier tous les clients du nouveau temps
                                await _hubContext.Clients.All.ReceiveNewBid(new
                                {
                                    type = "tempsLotMisAJour",
                                    lotId = lot.Id,
                                    nouveauTemps = lot.DateFinDecompteLot.Value,
                                    // Ajouter l'ordre des lots pour optimiser
                                    ordreLotsActuel = await _context.Lots
                                            .Where(l => !l.EstVendu)
                                            .OrderBy(l => l.DateFinDecompteLot)
                                            .Select(l => l.Id)
                                            .ToListAsync()
                                });
                            }
                        }
                    }
                }

                return (true, "Mise placée avec succès");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise pour le lot {LotId}", mise.LotId);
                return (false, "Une erreur est survenue lors de la mise");
            }
        }

        private bool EstMiseValide(Lot lot, decimal miseActuelle, decimal nouvelleMise, decimal miseMaximale, bool estMiseAutomatique = false)
        {
            // Vérifier d'abord si le temps est écoulé
            if (lot.DateFinDecompteLot.HasValue && DateTime.Now > lot.DateFinDecompteLot.Value)
            {
                return false;
            }

            if (miseActuelle == 0m)
            {
                // Si c'est une mise automatique, on vérifie juste que le montant maximal est suffisant
                // La mise effective sera le prix d'ouverture
                return nouvelleMise >= (decimal)lot.PrixOuverture;
            }

            decimal pasEnchere = CalculerPasEnchere(miseActuelle);
            if (estMiseAutomatique)
            {
                return miseMaximale >= (miseActuelle + (pasEnchere * 2));
            }
            return nouvelleMise >= (miseActuelle + pasEnchere);
        }

        private decimal CalculerPasEnchere(decimal montant)
        {
            if (montant <= 199.0m)
            {
                return 10.0m;
            }

            if (montant <= 499.0m)
            {
                return 25.0m;
            }

            if (montant <= 999.0m)
            {
                return 50.0m;
            }

            if (montant <= 1999.0m)
            {
                return 100.0m;
            }

            if (montant <= 4999.0m)
            {
                return 200.0m;
            }

            if (montant <= 9999.0m)
            {
                return 250.0m;
            }

            if (montant <= 19999.0m)
            {
                return 500.0m;
            }

            if (montant <= 49999.0m)
            {
                return 1000.0m;
            }

            if (montant <= 99999.0m)
            {
                return 2000.0m;
            }

            if (montant <= 499999.0m)
            {
                return 5000.0m;
            }

            return 10000.0m;
        }

        private async Task TraiterMisesAutomatiques(Lot lot, Encan encan)
        {
            bool continuerMises = true;
            while (continuerMises)
            {
                // Vérifier si le temps est écoulé
                if (lot.DateFinDecompteLot.HasValue && DateTime.Now > lot.DateFinDecompteLot.Value)
                {
                    break;
                }

                var misesAutomatiques = await _context.MiseAutomatiques
                    .Where(m => m.LotId == lot.Id &&
                               m.MontantMaximal.HasValue &&
                               m.MontantMaximal > (decimal)lot.Mise &&
                               m.UserId != lot.IdClientMise)
                    .OrderByDescending(m => m.MontantMaximal)
                    .ThenBy(m => m.DateMise)
                    .ToListAsync();

                if (!misesAutomatiques.Any())
                {
                    continuerMises = false;
                    continue;
                }

                var miseGagnante = misesAutomatiques.First();
                decimal nouvelleMise = (decimal)lot.Mise + CalculerPasEnchere((decimal)lot.PrixOuverture);

                if (nouvelleMise <= miseGagnante.MontantMaximal)
                {
                    var nouvelleMiseAuto = new MiseAutomatique
                    {
                        LotId = lot.Id,
                        UserId = miseGagnante.UserId,
                        Montant = nouvelleMise,
                        DateMise = DateTime.UtcNow,
                        EstMiseAutomatique = true,
                        MontantMaximal = miseGagnante.MontantMaximal
                    };

                    _context.MiseAutomatiques.Add(nouvelleMiseAuto);

                    // Correction ici : conversion explicite de decimal vers double
                    lot.Mise = Convert.ToDouble(nouvelleMise);
                    lot.IdClientMise = miseGagnante.UserId;
                    _context.Lots.Update(lot);

                    await _context.SaveChangesAsync();

                    // Vérifier si le temps est écoulé et gérer le pas de mise
                    if (lot.DateFinDecompteLot.HasValue)
                    {
                        var tempsRestant = (lot.DateFinDecompteLot.Value - DateTime.Now).TotalSeconds;
                        if (tempsRestant <= -60)
                        {
                            // Ajouter le pas de mise
                            lot.DateFinDecompteLot = lot.DateFinDecompteLot.Value.AddSeconds(encan.PasMise);
                            await _context.SaveChangesAsync();

                            // Notifier les clients du nouveau temps
                            await _hubContext.Clients.All.ReceiveNewBid(new
                            {
                                type = "tempsLotMisAJour",
                                lotId = lot.Id,
                                nouveauTemps = lot.DateFinDecompteLot.Value,
                                ordreLotsActuel = await _context.Lots
                                    .Where(l => !l.EstVendu)
                                    .OrderBy(l => l.DateFinDecompteLot)
                                    .Select(l => l.Id)
                                    .ToListAsync()
                            });
                        }
                    }
                }
                else
                {
                    continuerMises = false;
                }
            }
        }

        // Méthode pour récupérer l'historique des mises d'un lot
        public async Task<IEnumerable<MiseHistoriqueVM>> GetLotBidHistory(int lotId)
        {
            return await _context.MiseAutomatiques
                .Where(m => m.LotId == lotId)
                .OrderByDescending(m => m.DateMise)
                .Select(m => new MiseHistoriqueVM
                {
                    UserId = m.UserId,
                    Montant = m.Montant,
                    DateMise = m.DateMise,
                    EstMiseAutomatique = m.EstMiseAutomatique
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserBidVM>> GetUserBids(string userId)
        {
            _logger.LogInformation($"Service - Recherche des mises pour l'utilisateur {userId}");

            try
            {
                var userBids = await _context.MiseAutomatiques
                    .Where(m => m.UserId == userId)
                    .GroupBy(m => m.LotId)
                    .Select(g => new UserBidVM
                    {
                        LotId = g.Key,
                        IsLastBidder = _context.Lots
                            .Where(l => l.Id == g.Key)
                            .Select(l => l.IdClientMise == userId)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                _logger.LogInformation($"Service - Trouvé {userBids.Count()} lots avec mises pour l'utilisateur {userId}");
                return userBids;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service - Erreur lors de la récupération des mises pour {userId}");
                throw;
            }
        }


        // Logique pour récupérer les utilisateurs ayant déjà misé sur le lot
        public List<string> GetUsersWhoBidOnLot(int lotId)
        {
            return new List<string> { /* Liste des IDs utilisateur */ };
        }

        public async Task<double?> GetUserLastBid(int lotId, string userId)
        {
            var lastBid = await _context.MiseAutomatiques
                .Where(m => m.LotId == lotId && m.UserId == userId)
                .OrderByDescending(m => m.DateMise)
                .Select(m => (double?)Convert.ToDouble(m.Montant))  // Conversion explicite de decimal vers double
                .FirstOrDefaultAsync();

            return lastBid;
        }

        public ICollection<ArtisteVM> ObtenirTousArtistes()
        {
            var artistes = _context.Lots
                .GroupBy(x => x.Artiste)
                .Select(l => new ArtisteVM()
                {
                    NomArtiste = l.Key,
                }).ToList();
            return artistes;
        }

        public async Task<int> GetNombreMises(int lotId)
        {
            return await _context.MiseAutomatiques
                .Where(m => m.LotId == lotId)
                .CountAsync();
        }


        // Version publique avec sa propre transaction
        public async Task MarquerLotVendu(int lotId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await MarquerLotVenduInterne(lotId);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Erreur lors du marquage du lot comme vendu");
                throw;
            }
        }

        // Version interne sans transaction
        public async Task MarquerLotVenduInterne(int lotId)
        {
            var lot = await _context.Lots
                .Include(l => l.EncanLots)
                    .ThenInclude(el => el.Encan)
                .FirstOrDefaultAsync(l => l.Id == lotId);

            if (lot == null || lot.EstVendu)
            {
                return;
            }

            var maintenant = DateTime.Now;
            if (!lot.DateFinDecompteLot.HasValue || maintenant < lot.DateFinDecompteLot.Value)
            {
                return;
            }

            lot.EstVendu = true;
            lot.DateFinVente = maintenant;

            await _context.SaveChangesAsync();

            // Notifier tous les clients
            await _hubContext.Clients.All.ReceiveNewBid(new
            {
                type = "lotVendu",
                lotId = lotId,
                timestamp = maintenant
            });

        }

        public async Task FinaliserEncan(int numeroEncan)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                
                // Log l'URL de base du client
                _logger.LogInformation($"BaseAddress du client: {client.BaseAddress}");

                // Construction des URLs complètes pour le logging
                var urlFactures = $"{client.BaseAddress}api/factures/CreerFacturesParEncan/{numeroEncan}";
                var urlPaiements = $"{client.BaseAddress}api/paiement/ChargerClients/{numeroEncan}";

                _logger.LogInformation($"Tentative d'appel à l'URL des factures: {urlFactures}");
                var factureResponse = await client.PostAsync($"api/factures/CreerFacturesParEncan/{numeroEncan}", null);
                var factureContent = await factureResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Réponse création factures: {factureResponse.StatusCode} - {factureContent}");

                _logger.LogInformation($"Tentative d'appel à l'URL des paiements: {urlPaiements}");
                var paiementResponse = await client.PostAsync($"api/paiement/ChargerClients/{numeroEncan}", null);
                var paiementContent = await paiementResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Réponse paiements: {paiementResponse.StatusCode} - {paiementContent}");

                // Marquer l'encan comme terminé
                var encan = await _context.Encans
                    .FirstOrDefaultAsync(e => e.NumeroEncan == numeroEncan);

                if (encan != null)
                {
                    encan.EstTermine = true;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Encan #{numeroEncan} marqué comme terminé");
                }

                // Notifier les clients
                await _hubContext.Clients.All.ReceiveNewBid(new
                {
                    type = "encanTermine",
                    numeroEncan = numeroEncan,
                    message = "L'encan est terminé et les factures ont été générées."
                });

                _logger.LogInformation($"Finalisation de l'encan #{numeroEncan} terminée avec succès");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la finalisation de l'encan {numeroEncan}");
                throw;
            }
        }

        public async Task<IEnumerable<MisesParEncanVM>> GetUserBidsGroupedByEncan(string userId)
        {
            try
            {
                _logger.LogInformation("Début de la récupération des mises pour {UserId}", userId);

                var lots = await _context.Lots
                    .Where(l => l.MisesAutomatiques.Any(m => m.UserId == userId))
                    .Include(l => l.EncanLots)
                        .ThenInclude(el => el.Encan)
                    .Include(l => l.Photos)
                    .Include(l => l.Categorie)
                    .Include(l => l.Medium)
                    .Include(l => l.Vendeur)
                    .ToListAsync();

                if (!lots.Any())
                {
                    _logger.LogInformation("Aucun lot trouvé pour l'utilisateur {UserId}", userId);
                    return new List<MisesParEncanVM>();
                }

                var misesParEncan = lots
                    .Where(l => l.EncanLots.Any())
                    .GroupBy(l => l.EncanLots.FirstOrDefault()?.Encan)
                    .Where(g => g.Key != null)
                    .OrderByDescending(g => g.Key.DateFin)
                    .Select(g => new MisesParEncanVM
                    {
                        NumeroEncan = g.Key.NumeroEncan,
                        DateEncan = g.Key.DateFin,
                        Lots = g.Select(l => new LotMiseVM
                        {
                            LotId = l.Id,
                            Numero = l.Numero,
                            Artiste = l.Artiste,
                            Hauteur = l.Hauteur,
                            Largeur = l.Largeur,
                            DerniereMise = l.Mise.HasValue ? (decimal)l.Mise.Value : 0m,
                            ValeurEstimeMin = (decimal)l.ValeurEstimeMin,
                            ValeurEstimeMax = (decimal)l.ValeurEstimeMax,
                            PrixOuverture = (decimal)l.PrixOuverture,
                            PrixMinPourVente = l.PrixMinPourVente.HasValue ? (decimal)l.PrixMinPourVente.Value : 0m,
                            EstPlusHautEncherisseur = l.IdClientMise == userId,
                            EstVendu = l.EstVendu,
                            PhotoPrincipale = l.Photos.FirstOrDefault()?.Lien ?? "",
                            DateFinDecompteLot = l.DateFinDecompteLot,
                            EstLivrable = l.EstLivrable,
                            Description = l.Description,
                            IdCategorie = l.IdCategorie,
                            Categorie = l.Categorie.Nom,
                            IdMedium = l.IdMedium,
                            Medium = l.Medium.Type,
                            IdVendeur = l.IdVendeur,
                            Vendeur = $"{l.Vendeur.Prenom} {l.Vendeur.Nom}",
                            SeraLivree = l.SeraLivree ?? false
                        }).ToList()
                    })
                    .ToList();

                return misesParEncan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des mises pour {UserId}", userId);
                throw;
            }
        }

    }
}