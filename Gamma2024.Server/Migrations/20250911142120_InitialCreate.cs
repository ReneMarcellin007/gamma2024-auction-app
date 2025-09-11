using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gamma2024.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: true),
                    StripeCustomer = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Charites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomOrganisme = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Encans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroEncan = table.Column<int>(type: "INTEGER", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateDebutSoireeCloture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstPublie = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasLot = table.Column<int>(type: "INTEGER", nullable: false),
                    PasMise = table.Column<int>(type: "INTEGER", nullable: false),
                    EstTermine = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mediums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mediums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    Appartement = table.Column<string>(type: "TEXT", nullable: true),
                    Rue = table.Column<string>(type: "TEXT", nullable: false),
                    Ville = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Pays = table.Column<string>(type: "TEXT", nullable: false),
                    CodePostal = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    EstDomicile = table.Column<bool>(type: "INTEGER", nullable: false),
                    IdApplicationUser = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adresses_AspNetUsers_IdApplicationUser",
                        column: x => x.IdApplicationUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    EstLu = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreeA = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactureLivraisons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateAchat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrixFinal = table.Column<double>(type: "REAL", nullable: false),
                    SousTotal = table.Column<double>(type: "REAL", nullable: false),
                    TPS = table.Column<double>(type: "REAL", nullable: false),
                    TVQ = table.Column<double>(type: "REAL", nullable: false),
                    Don = table.Column<double>(type: "REAL", nullable: true),
                    IdAdresse = table.Column<int>(type: "INTEGER", nullable: true),
                    IdCharite = table.Column<int>(type: "INTEGER", nullable: true),
                    IdFacture = table.Column<int>(type: "INTEGER", nullable: false),
                    EstPaye = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactureLivraisons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactureLivraisons_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FactureLivraisons_Charites_IdCharite",
                        column: x => x.IdCharite,
                        principalTable: "Charites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vendeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    Courriel = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    AdresseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendeurs_Adresses_AdresseId",
                        column: x => x.AdresseId,
                        principalTable: "Adresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateAchat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrixFinal = table.Column<double>(type: "REAL", nullable: false),
                    SousTotal = table.Column<double>(type: "REAL", nullable: false),
                    PrixLots = table.Column<double>(type: "REAL", nullable: false),
                    FraisEncanteur = table.Column<double>(type: "REAL", nullable: false),
                    TPS = table.Column<double>(type: "REAL", nullable: false),
                    TVQ = table.Column<double>(type: "REAL", nullable: false),
                    IdClient = table.Column<string>(type: "TEXT", nullable: false),
                    estPaye = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumeroEncan = table.Column<int>(type: "INTEGER", nullable: false),
                    Livrable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChoixLivraison = table.Column<bool>(type: "INTEGER", nullable: true),
                    IdFactureLivraison = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factures_AspNetUsers_IdClient",
                        column: x => x.IdClient,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factures_FactureLivraisons_IdFactureLivraison",
                        column: x => x.IdFactureLivraison,
                        principalTable: "FactureLivraisons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ValeurEstimeMin = table.Column<double>(type: "REAL", nullable: false),
                    ValeurEstimeMax = table.Column<double>(type: "REAL", nullable: false),
                    PrixOuverture = table.Column<double>(type: "REAL", nullable: false),
                    PrixMinPourVente = table.Column<double>(type: "REAL", nullable: true),
                    DateDepot = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Artiste = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdCategorie = table.Column<int>(type: "INTEGER", nullable: false),
                    IdClientMise = table.Column<string>(type: "TEXT", nullable: true),
                    Mise = table.Column<double>(type: "REAL", nullable: true),
                    EstVendu = table.Column<bool>(type: "INTEGER", nullable: false),
                    SeraLivree = table.Column<bool>(type: "INTEGER", nullable: true),
                    DateFinVente = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IdVendeur = table.Column<int>(type: "INTEGER", nullable: false),
                    EstLivrable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Largeur = table.Column<double>(type: "REAL", nullable: false),
                    Hauteur = table.Column<double>(type: "REAL", nullable: false),
                    IdMedium = table.Column<int>(type: "INTEGER", nullable: false),
                    IdFacture = table.Column<int>(type: "INTEGER", nullable: true),
                    DateDebutDecompteLot = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateFinDecompteLot = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_AspNetUsers_IdClientMise",
                        column: x => x.IdClientMise,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lots_Categories_IdCategorie",
                        column: x => x.IdCategorie,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lots_Factures_IdFacture",
                        column: x => x.IdFacture,
                        principalTable: "Factures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lots_Mediums_IdMedium",
                        column: x => x.IdMedium,
                        principalTable: "Mediums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lots_Vendeurs_IdVendeur",
                        column: x => x.IdVendeur,
                        principalTable: "Vendeurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EncanLots",
                columns: table => new
                {
                    IdEncan = table.Column<int>(type: "INTEGER", nullable: false),
                    IdLot = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncanLots", x => new { x.IdEncan, x.IdLot });
                    table.ForeignKey(
                        name: "FK_EncanLots_Encans_IdEncan",
                        column: x => x.IdEncan,
                        principalTable: "Encans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncanLots_Lots_IdLot",
                        column: x => x.IdLot,
                        principalTable: "Lots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MiseAutomatiques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontantMaximal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateMise = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstMiseAutomatique = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiseAutomatiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiseAutomatiques_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiseAutomatiques_Lots_LotId",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Lien = table.Column<string>(type: "TEXT", nullable: false),
                    IdLot = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Lots_IdLot",
                        column: x => x.IdLot,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7da4163f-edb4-47b5-86ea-888888888888", "7da4163f-edb4-47b5-86ea-888888888888", "Client", "CLIENT" },
                    { "7da4163f-edb4-47b5-86ea-999999999999", "7da4163f-edb4-47b5-86ea-999999999999", "Administrateur", "ADMINISTRATEUR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StripeCustomer", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1d8ac862-e54d-4f10-b6f8-638808c02967", 0, "/Avatars/Defaut.png", "ff176598-423c-4557-bfc4-48e928f579e9", "client@example.com", true, "Jean", false, null, "Dupont", "CLIENT@EXAMPLE.COM", "CLIENT@EXAMPLE.COM", "AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==", "455-555-5555", false, "860b65a9-156b-473d-b11b-be6f787cf1e4", "", false, "client@example.com" },
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "/Avatars/Defaut.png", "f1f3de20-69b9-491d-bc82-b484b44cd47f", "admin@example.com", true, "Super", false, null, "Admin", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==", "466-666-6666", false, "87162d4b-fc3b-4d65-8341-2f4a480982d1", "", false, "admin@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Adresses",
                columns: new[] { "Id", "Appartement", "CodePostal", "EstDomicile", "IdApplicationUser", "Numero", "Pays", "Province", "Rue", "Ville" },
                values: new object[,]
                {
                    { 1, null, "A1A1A1", true, "8e445865-a24d-4543-a6c6-9443d048cdb9", 123, "Pays Admin", "Québec", "Rue Admin", "Ville Admin" },
                    { 2, null, "A1A1A1", true, "1d8ac862-e54d-4f10-b6f8-638808c02967", 456, "Pays Client", "Québec", "Rue Client", "Ville Client" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7da4163f-edb4-47b5-86ea-888888888888", "1d8ac862-e54d-4f10-b6f8-638808c02967" },
                    { "7da4163f-edb4-47b5-86ea-999999999999", "8e445865-a24d-4543-a6c6-9443d048cdb9" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_IdApplicationUser",
                table: "Adresses",
                column: "IdApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EncanLots_IdLot",
                table: "EncanLots",
                column: "IdLot");

            migrationBuilder.CreateIndex(
                name: "IX_Encans_NumeroEncan",
                table: "Encans",
                column: "NumeroEncan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactureLivraisons_IdAdresse",
                table: "FactureLivraisons",
                column: "IdAdresse");

            migrationBuilder.CreateIndex(
                name: "IX_FactureLivraisons_IdCharite",
                table: "FactureLivraisons",
                column: "IdCharite");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdClient",
                table: "Factures",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdFactureLivraison",
                table: "Factures",
                column: "IdFactureLivraison",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_IdCategorie",
                table: "Lots",
                column: "IdCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_IdClientMise",
                table: "Lots",
                column: "IdClientMise");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_IdFacture",
                table: "Lots",
                column: "IdFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_IdMedium",
                table: "Lots",
                column: "IdMedium");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_IdVendeur",
                table: "Lots",
                column: "IdVendeur");

            migrationBuilder.CreateIndex(
                name: "IX_MiseAutomatiques_LotId",
                table: "MiseAutomatiques",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_MiseAutomatiques_UserId",
                table: "MiseAutomatiques",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IdLot",
                table: "Photos",
                column: "IdLot");

            migrationBuilder.CreateIndex(
                name: "IX_Vendeurs_AdresseId",
                table: "Vendeurs",
                column: "AdresseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EncanLots");

            migrationBuilder.DropTable(
                name: "MiseAutomatiques");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Encans");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "Mediums");

            migrationBuilder.DropTable(
                name: "Vendeurs");

            migrationBuilder.DropTable(
                name: "FactureLivraisons");

            migrationBuilder.DropTable(
                name: "Adresses");

            migrationBuilder.DropTable(
                name: "Charites");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
