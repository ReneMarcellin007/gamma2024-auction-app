using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Gamma2024.Server.ViewModels
{
    public class LotCreationVM
    {
        public string Numero { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ValeurEstimeMin { get; set; }
        public double ValeurEstimeMax { get; set; }
        public double PrixOuverture { get; set; }
        public double? PrixMinPourVente { get; set; }
        public DateTime DateDepot { get; set; }
        public string Artiste { get; set; } = null!;
        public DateTime DateCreation { get; set; }
        public int IdCategorie { get; set; }
        public string? IdClientMise { get; set; }
        public double? Mise { get; set; }
        public bool EstVendu { get; set; }
        public bool? SeraLivree { get; set; }
        public DateTime? DateFinVente { get; set; }
        public int IdVendeur { get; set; }
        public bool EstLivrable { get; set; }
        public double Largeur { get; set; }
        public double Hauteur { get; set; }
        public int IdMedium { get; set; }
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
        public int IdEncan { get; set; }
    }
}
