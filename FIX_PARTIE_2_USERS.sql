-- ===== PARTIE 2: CORRIGER LES UTILISATEURS =====
-- Exécutez ce script APRÈS le script PARTIE 1

-- 1. Créer/Corriger client@example.com
-- Hash pour "MotDePasseClient123!" (avec "Client")
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
) VALUES (
    '1d8ac862-e54d-4f10-b6f8-638808c02967',
    'client@example.com', 'CLIENT@EXAMPLE.COM',
    'client@example.com', 'CLIENT@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
    '860b65a9-156b-473d-b11b-be6f787cf1e4',
    'ff176598-423c-4557-bfc4-48e928f579e9',
    '455-555-5555', false, false, NULL, true, 0,
    'Dupont', 'Jean', '/Avatars/Defaut.png', ''
) ON CONFLICT ("Id") DO UPDATE SET
    "UserName" = 'client@example.com',
    "Email" = 'client@example.com',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==';

-- 2. Créer/Corriger admin@example.com
-- Hash pour "AdminPassword123!"
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
) VALUES (
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'admin@example.com', 'ADMIN@EXAMPLE.COM',
    'admin@example.com', 'ADMIN@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'f1f3de20-69b9-491d-bc82-b484b44cd47f',
    '466-666-6666', false, false, NULL, true, 0,
    'Admin', 'Super', '/Avatars/Defaut.png', ''
) ON CONFLICT ("Id") DO UPDATE SET
    "UserName" = 'admin@example.com',
    "Email" = 'admin@example.com',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==';

-- 3. Afficher les logins disponibles
SELECT '==============================' as info;
SELECT 'CONNEXIONS DISPONIBLES:' as titre;
SELECT '==============================' as info;
SELECT 'client@example.com' as email, 'MotDePasseClient123!' as password;
SELECT 'admin@example.com' as email, 'AdminPassword123!' as password;