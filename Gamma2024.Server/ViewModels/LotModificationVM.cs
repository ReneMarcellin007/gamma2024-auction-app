namespace Gamma2024.Server.ViewModels
{
    public class LotModificationVM : LotCreationVM
    {
        public int Id { get; set; }
        public new int? IdEncanModifie { get; set; }
        public List<IFormFile> NouvellesPhotos { get; set; } = new List<IFormFile>();
        public List<int> PhotosASupprimer { get; set; } = new List<int>();
        public new List<PhotoVM> PhotosModifie { get; set; } = new List<PhotoVM>();

        // Propriétés supplémentaires pour l'affichage
        public string? Categorie { get; set; }
        public string? Vendeur { get; set; }
        public string? Medium { get; set; }
        public string? NumeroEncan { get; set; }
        public string? NomClientMise { get; set; }
    }
}
