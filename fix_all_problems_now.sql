-- ===== CORRECTION DÉFINITIVE DE TOUS LES PROBLÈMES =====

-- 1. CORRIGER TOUTES LES URLS D'IMAGES TRONQUÉES
UPDATE photos SET "Lien" = 'https://via.placeholder.com/800x600/FF6B6B/' || "Lien" WHERE "Lien" NOT LIKE 'http%';

-- Corrections spécifiques pour les patterns cassés
UPDATE photos SET "Lien" = REPLACE("Lien", 'https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=', 'https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=');
UPDATE photos SET "Lien" = REPLACE("Lien", 'https://via.placeholder.com/800x600/FF6B6B/000000?text=', 'https://via.placeholder.com/800x600/4ECDC4/000000?text=');

-- 2. VÉRIFIER LES UTILISATEURS ET LEURS MOTS DE PASSE
SELECT
    "Email",
    "UserName",
    "EmailConfirmed",
    LENGTH("PasswordHash") as hash_length,
    "Name",
    "FirstName"
FROM "AspNetUsers";

-- 3. CRÉER/METTRE À JOUR UTILISATEUR client@example.com AVEC LE BON HASH
-- Hash pour "MotDePasseClient123!" (avec "Client", pas juste "MotDePasse")
INSERT INTO "AspNetUsers" (
    "Id",
    "UserName",
    "NormalizedUserName",
    "Email",
    "NormalizedEmail",
    "EmailConfirmed",
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumber",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnd",
    "LockoutEnabled",
    "AccessFailedCount",
    "Name",
    "FirstName",
    "Avatar",
    "StripeCustomer"
) VALUES (
    '1d8ac862-e54d-4f10-b6f8-638808c02967',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
    '860b65a9-156b-473d-b11b-be6f787cf1e4',
    'ff176598-423c-4557-bfc4-48e928f579e9',
    '455-555-5555',
    false,
    false,
    NULL,
    true,
    0,
    'Dupont',
    'Jean',
    '/Avatars/Defaut.png',
    ''
) ON CONFLICT ("Id") DO UPDATE SET
    "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
    "EmailConfirmed" = true;

-- 4. CRÉER/METTRE À JOUR admin@example.com AUSSI
INSERT INTO "AspNetUsers" (
    "Id",
    "UserName",
    "NormalizedUserName",
    "Email",
    "NormalizedEmail",
    "EmailConfirmed",
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumber",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnd",
    "LockoutEnabled",
    "AccessFailedCount",
    "Name",
    "FirstName",
    "Avatar",
    "StripeCustomer"
) VALUES (
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'f1f3de20-69b9-491d-bc82-b484b44cd47f',
    '466-666-6666',
    false,
    false,
    NULL,
    true,
    0,
    'Admin',
    'Super',
    '/Avatars/Defaut.png',
    ''
) ON CONFLICT ("Id") DO UPDATE SET
    "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    "EmailConfirmed" = true;

-- 5. VÉRIFIER QUE LES IMAGES SONT MAINTENANT CORRECTES
SELECT
    "Id",
    "IdLot",
    LEFT("Lien", 50) as url_debut,
    CASE
        WHEN "Lien" LIKE 'http%' THEN 'OK'
        ELSE 'ERREUR'
    END as statut
FROM photos
LIMIT 10;

-- 6. AFFICHER LES CREDENTIALS CORRECTS
SELECT
    '========================================' as separator;

SELECT
    'CONNEXION CLIENT' as type,
    'client@example.com' as email,
    'MotDePasseClient123!' as password;

SELECT
    'CONNEXION ADMIN' as type,
    'admin@example.com' as email,
    'AdminPassword123!' as password;

-- 7. STATISTIQUES FINALES
SELECT
    'ENCANS: ' || COUNT(*) as stat FROM encans
UNION ALL
SELECT
    'LOTS: ' || COUNT(*) FROM lots
UNION ALL
SELECT
    'PHOTOS: ' || COUNT(*) FROM photos
UNION ALL
SELECT
    'PHOTOS AVEC URL VALIDE: ' || COUNT(*) FROM photos WHERE "Lien" LIKE 'http%';