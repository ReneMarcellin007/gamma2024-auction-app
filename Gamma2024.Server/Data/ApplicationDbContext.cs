using Gamma2024.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gamma2024.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Encan> Encans { get; set; } = default!;
        public DbSet<EncanLot> EncanLots { get; set; } = default!;
        public DbSet<Lot> Lots { get; set; } = default!;
        public DbSet<Categorie> Categories { get; set; } = default!;
        public DbSet<Photo> Photos { get; set; } = default!;
        public DbSet<Facture> Factures { get; set; } = default!;
        public DbSet<FactureLivraison> FactureLivraisons { get; set; } = default!;
        public DbSet<Vendeur> Vendeurs { get; set; } = default!;
        public DbSet<Adresse> Adresses { get; set; } = default!;
        public DbSet<Medium> Mediums { get; set; } = default!;
        public DbSet<Charite> Charites { get; set; } = default!;
        public DbSet<MiseAutomatique> MiseAutomatiques { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureRelationships(builder);
            CreateRolesAndUsers(builder);
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            builder.Entity<Encan>()
                .HasIndex(e => e.NumeroEncan)
                .IsUnique();

            // ApplicationUser
            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Adresses)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey(a => a.IdApplicationUser)
                .OnDelete(DeleteBehavior.NoAction);

            // Vendeur
            builder.Entity<Vendeur>()
                .HasMany(v => v.Lots)
                .WithOne(l => l.Vendeur)
                .HasForeignKey(l => l.IdVendeur)
                .OnDelete(DeleteBehavior.NoAction);

            // Lot
            builder.Entity<Lot>()
                .HasOne(l => l.Categorie)
                .WithMany(c => c.Lots)
                .HasForeignKey(l => l.IdCategorie)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Lot>()
                .HasOne(l => l.ClientMise)
                .WithMany()
                .HasForeignKey(l => l.IdClientMise)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Lot>()
                .HasOne(l => l.Medium)
                .WithMany(m => m.Lots)
                .HasForeignKey(l => l.IdMedium)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Lot>()
                .HasOne(l => l.Facture)
                .WithMany(f => f.Lots)
                .HasForeignKey(l => l.IdFacture)
                .OnDelete(DeleteBehavior.NoAction);

            // EncanLot
            builder.Entity<EncanLot>()
                .HasKey(el => new { el.IdEncan, el.IdLot });

            builder.Entity<EncanLot>()
                .HasOne(el => el.Encan)
                .WithMany(e => e.EncanLots)
                .HasForeignKey(el => el.IdEncan);

            builder.Entity<EncanLot>()
                .HasOne(el => el.Lot)
                .WithMany(l => l.EncanLots)
                .HasForeignKey(el => el.IdLot)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Facture>()
                .HasOne(f => f.Client)
                .WithMany()
                .HasForeignKey(f => f.IdClient)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FactureLivraison>()
                .HasOne(fl => fl.Facture)
                .WithOne(f => f.FactureLivraison)
                .HasForeignKey<Facture>(f => f.IdFactureLivraison)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FactureLivraison>()
                .HasOne(fl => fl.Charite)
                .WithMany(fl => fl.FacturesLivraisons)
                .HasForeignKey(fl => fl.IdCharite)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FactureLivraison>()
                .HasOne(fl => fl.Adresse)
                .WithMany(a => a.FacturesLivraisons)
                .HasForeignKey(f => f.IdAdresse)
                .OnDelete(DeleteBehavior.NoAction);


            // Photo
            builder.Entity<Photo>()
                .HasOne(p => p.Lot)
                .WithMany(l => l.Photos)
                .HasForeignKey(p => p.IdLot)
                .OnDelete(DeleteBehavior.Cascade);

            // Medium
            builder.Entity<Medium>()
                .HasMany(m => m.Lots)
                .WithOne(l => l.Medium)
                .HasForeignKey(l => l.IdMedium)
                .OnDelete(DeleteBehavior.Restrict);


            // Configuration pour MiseAutomatique
            builder.Entity<MiseAutomatique>()
                .HasOne(m => m.Lot)
                .WithMany(l => l.MisesAutomatiques)
                .HasForeignKey(m => m.LotId);

            builder.Entity<MiseAutomatique>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);

            //notification
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Notifications)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(e => e.ApplicationUserId)
                .IsRequired();
        }

        private void CreateRolesAndUsers(ModelBuilder builder)
        {
            var adminId = "8e445865-a24d-4543-a6c6-9443d048cdb9";
            var clientId = "1d8ac862-e54d-4f10-b6f8-638808c02967";

            // Création des rôles
            string roleIdAdmin = "7da4163f-edb4-47b5-86ea-999999999999";
            string roleIdClient = "7da4163f-edb4-47b5-86ea-888888888888";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = ApplicationRoles.ADMINISTRATEUR,
                    NormalizedName = ApplicationRoles.ADMINISTRATEUR.ToUpper(),
                    Id = roleIdAdmin,
                    ConcurrencyStamp = roleIdAdmin
                },
                new IdentityRole
                {
                    Name = ApplicationRoles.CLIENT,
                    NormalizedName = ApplicationRoles.CLIENT.ToUpper(),
                    Id = roleIdClient,
                    ConcurrencyStamp = roleIdClient
                }
            );

            // Création des adresses
            builder.Entity<Adresse>().HasData(
                new Adresse
                {
                    Id = 1,
                    Numero = 123,
                    Rue = "Rue Admin",
                    Ville = "Ville Admin",
                    CodePostal = "A1A1A1",
                    Pays = "Pays Admin",
                    Province = "Québec",
                    IdApplicationUser = adminId,
                    EstDomicile = true,
                },
                new Adresse
                {
                    Id = 2,
                    Numero = 456,
                    Rue = "Rue Client",
                    Ville = "Ville Client",
                    CodePostal = "A1A1A1",
                    Pays = "Pays Client",
                    Province = "Québec",
                    IdApplicationUser = clientId,
                    EstDomicile = true,
                }
            );

            // Création de l'administrateur et du client
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = "87162d4b-fc3b-4d65-8341-2f4a480982d1",
                Name = "Admin",
                FirstName = "Super",
                Avatar = "/Avatars/Defaut.png",
                PhoneNumber = "466-666-6666",
                ConcurrencyStamp = "f1f3de20-69b9-491d-bc82-b484b44cd47f",
                PasswordHash = "AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==",
                StripeCustomer = ""
            };
            //clientUser.PasswordHash = passwordHasher.HashPassword(clientUser, "MotDePasseClient123!");


            var clientUser = new ApplicationUser
            {
                Id = clientId,
                UserName = "client@example.com",
                NormalizedUserName = "CLIENT@EXAMPLE.COM",
                Email = "client@example.com",
                NormalizedEmail = "CLIENT@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = "860b65a9-156b-473d-b11b-be6f787cf1e4",
                Name = "Dupont",
                FirstName = "Jean",
                Avatar = "/Avatars/Defaut.png",
                PhoneNumber = "455-555-5555",
                ConcurrencyStamp = "ff176598-423c-4557-bfc4-48e928f579e9",
                PasswordHash = "AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==",
                StripeCustomer = ""
            };
            //adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "MotDePasseAdmin123!");

            builder.Entity<ApplicationUser>().HasData(adminUser, clientUser);

            // Attribution des rôles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = roleIdAdmin,
                    UserId = adminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = roleIdClient,
                    UserId = clientId
                }
            );
        }
    }
}
